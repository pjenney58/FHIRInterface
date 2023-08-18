/*
* Software License - TCP.cs
*
*  Copyright (c) 2016 - 2022 by Sand Drift Software, LLC
*
* Permission is hereby granted, in consideration of a license fee or agreement,
* to any person obtaining a copy of this software and associated documentation
* files (the "Software"), to deal in the Software without restriction, including
* without limitation the rights to use, copy, modify, merge, publish, distribute,
* and/or sell copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
*The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Hl7Harmonizer.Transport.Model
{
    public class StatefulTcp : ITransport
    {
        #region Stateful Listener

        private TransportContext? Context;
        private IBaseEventLogger EventLogger;

        private TcpListener? tcpListener;

        private List<WorkingClient> tcpClients = new();
        private bool listening = true;
        private readonly List<WorkingClient> tcpClientList = new();

        public StatefulTcp(TransportContext context)
        {
            Context = context;
            EventLogger = context.logger;
        }

        // Checks if a socket has disconnected Adapted from -- http://stackoverflow.com/questions/722240/instantly-detect-client-disconnection-from-server-socket
        private static bool IsDisconnected(WorkingClient thisClient)
        {
            if (thisClient == null || thisClient.TcpClient == null)
            {
                throw new InvalidOperationException(nameof(thisClient));
            }

            try
            {
                Socket s = thisClient.TcpClient.Client;
                return s.Poll(10 * 1000, SelectMode.SelectRead) && (s.Available == 0);
            }
            catch (SocketException)
            {
                // We got a socket error, assume it's disconnected
                return true;
            }
        }

        // Sees if any of the clients have left the chat server
        private void CheckForDisconnects()
        {
            // Check the viewers first
            foreach (var workingClient in tcpClientList.ToArray())
            {
                if (workingClient != null && workingClient.TcpClient != null)
                {
                    if (IsDisconnected(workingClient))
                    {
                        EventLogger.ReportInfo($"\t\t\tClient {workingClient.TcpClient.Client.RemoteEndPoint} has disconnected.");

                        // cleanup on our end
                        tcpClientList.Remove(workingClient);     // Remove from list
                        ReleaseClient(workingClient);
                    }
                }
            }
        }

        // cleans up resources for a TcpClient
        private static void ReleaseClient(WorkingClient workingClient)
        {
            if (workingClient == null || workingClient.Stream == null)
            {
                throw new InvalidOperationException(nameof(workingClient));
            }

            workingClient.Stream.Close(); // Close network stream
        }

        private void HandleNewConnection(bool isSecure = false)
        {
            var workingClient = new WorkingClient()
            {
                TcpClient = tcpListener?.AcceptTcpClient(),
                IsSecure = isSecure,
            };

            if (workingClient == null || workingClient.TcpClient == null || workingClient.Stream == null)
            {
                throw new InvalidOperationException(nameof(workingClient));
            }

            workingClient.RemoteEndPoint = workingClient.TcpClient.Client.RemoteEndPoint;
            workingClient.CanRead = workingClient.Stream.CanRead;
            workingClient.CanWrite = workingClient.Stream.CanWrite;
            workingClient.Stream.WriteTimeout = 10000;
            workingClient.Stream.ReadTimeout = 10000;

            try
            {
                tcpClientList.Add(workingClient);

                EventLogger.ReportInfo($"\t\t\tAccepted new client from {workingClient.RemoteEndPoint}");
            }
            catch (Exception ex)
            {
                EventLogger.ReportWarning($"Failed to add endpoint {workingClient.RemoteEndPoint} to list.  {ex.Message}");
            }
        }

        // See if any of our messengers have sent us a new message, put it in the queue
        private void CheckForNewMessages()
        {
            foreach (WorkingClient tcpClient in tcpClientList)
            {
                if (tcpClient != null && tcpClient.TcpClient != null)
                {
                    if (tcpClient.TcpClient.Available > 0 && !tcpClient.InUse)
                    {
                        EventLogger.ReportDebug($"{tcpClient.CanRead} and {IsDisconnected(tcpClient)} and {tcpClient.InUse}");

                        try
                        {
                            // TODO:  Implement Record Processing Delegate
                        }
                        catch
                        {
                        }
                        //Task.Run(() => ProcessRecord(tcpClient));
                    }
                }
            }
        }

        public void StopListening()
        {
            listening = false;
        }

        /// <summary>
        /// <c> Listen </c> Synchronus listener on a clear text port
        /// </summary>
        public void Listen(string port, bool isSecure = false)
        {
            string data = string.Empty;

            tcpListener = new TcpListener(IPAddress.Any, int.Parse(port));

            try
            {
                tcpListener.Start();

                while (listening)
                {
                    // Check for new clients
                    if (tcpListener.Pending())
                    {
                        HandleNewConnection(isSecure);
                    }

                    // Do the rest
                    CheckForDisconnects();
                    CheckForNewMessages();

                    // Use less CPU
                    Thread.Sleep(10);
                }
            }
            catch (SocketException se)
            {
                EventLogger.ReportWarning($"A socket conflict occurred: {se.Message}");
                EventLogger.ReportWarning($"Port Failure: {port}.");
                throw;
            }
            catch (Exception se)
            {
                EventLogger.ReportWarning($"An error occurred while running the socket listener: {se.Message}");
                throw;
            }
            finally
            {
                // Stop the server, and clean up any connected clients
                foreach (WorkingClient tcpClient in tcpClientList)
                {
                    ReleaseClient(tcpClient);
                }

                tcpListener.Stop();
            }
        }

        public Task<T?> Get<T>(string route, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>?> Get<T>(string route)
        {
            throw new NotImplementedException();
        }

        public Task<T?> Put<T>(T data, string route, string id)
        {
            throw new NotImplementedException();
        }

        public Task<T?> Post<T>(T data, string route)
        {
            throw new NotImplementedException();
        }

        public Task<T?> Delete<T>(string route, string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void ListenAsync(string port, bool isSecure = false)
        {
            throw new NotImplementedException();
        }

        #endregion Stateful Listener
    }
}