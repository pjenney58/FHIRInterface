/*
* Software License - TCPServer.cs
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

using Support.Interface;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Support.Model
{
    internal class TCPServer : IDisposable
    {
        private readonly IBaseEventLogger logger = new BaseEventLogger("WorkingServer");

        //private MessageService _local;

        private TcpListener listener;
        public EndPoint RemoteEndPoint;
        public EndPoint LocalEndPoint;

        private bool IsSecure;

        /// <summary>
        /// <c> tcpClientConnected </c> Instance listener reset manager
        /// </summary>
        public ManualResetEvent TcpClientConnected;

        private X509Certificate x509Certificate;
        private X509Certificate2Collection x509Collection;
        private X509Store x509Store;
        private X509Certificate2Collection x509ValidCollection;

        /// <summary>
        /// Statefull or Stateless
        /// </summary>

        //public ConnectionType ConnectionType { get; set; }

        public TCPServer(string port, bool secure)
        {
            IsSecure = secure;
        }

        #region Async Listener

        private void AcceptStream(IAsyncResult asyncResult)
        {
            logger.ReportInfo("AcceptingIncomingClient");

            try
            {
                var tcpListenerAs = asyncResult.AsyncState as TcpListener;
                if (tcpListenerAs == null)
                {
                    TcpClientConnected.Set();
                    return;
                }

                var workingClient = new TCPClient()
                {
                    TcpClient = tcpListenerAs.EndAcceptTcpClient(asyncResult),
                    IsSecure = IsSecure
                };

                var waiting = 1000;
                while (workingClient.TcpClient.Available == 0 && waiting > 0)
                {
                    Thread.Sleep(10);
                    waiting -= 10;
                }

                logger.ReportInfo($"Waited for {1000 - waiting}ms");

                if (workingClient.TcpClient.Available == 0)
                {
                    //HandleTcpPing(workingClient.TcpClient.GetStream());
                    return;
                }

                if (workingClient.TcpClient.Available > 0)
                {
                    logger.ReportTrace("StartProcessRecord");
                    var data = workingClient.Read();
                    // Now do something with it
                }

                logger.ReportTrace("Leaving AcceptStream.");
            }
            catch (Exception ex)
            {
                logger.ReportWarning($"Error in AcceptStream: {ex.Message}");
            }
            finally
            {
                logger.ReportTrace("Calling TcpClientConnected.Set()");
                TcpClientConnected.Set();
            }
        }

        /// <summary>
        /// <c> ListenAsync </c> Async listener, hands off to AsyncHandler on tap
        /// </summary>
        public void ListenAsync(string port)
        {
            logger.ReportInfo($"Starting ListenAsync");

            TcpClientConnected = new ManualResetEvent(false);
            listener = new TcpListener(IPAddress.Any, int.Parse(port));

            try
            {
                listener.Start();

                while (listening)
                {
                    logger.ReportTrace("TcpClientConnected.Reset()");
                    TcpClientConnected.Reset();

                    logger.ReportTrace($"_listener.BeginAcceptTcpClient(new AsyncCallback(AcceptStream), _listener)");
                    listener.BeginAcceptTcpClient(AcceptStream, listener);

                    logger.ReportTrace("TcpClientConnected.WaitOne();");
                    TcpClientConnected.WaitOne();

                    logger.ReportInfo("Started successfully.");
                }
            }
            catch (SocketException se)
            {
                logger.ReportWarning($"A socket conflict occurred: {se.Message}");
                logger.ReportWarning($"Port Failure: {port}.");
                throw;
            }
            catch (Exception se)
            {
                logger.ReportWarning($"An error occurred while running the socket listener: {se.Message}");
                throw;
            }
            finally
            {
                logger.ReportTrace("Stopping ListenAsync");
                listener.Stop();
            }
        }

        #endregion Async Listener

        #region Stateful Listener

        private readonly List<TCPClient> tcpClientList = new List<TCPClient>();

        // Checks if a socket has disconnected Adapted from -- http://stackoverflow.com/questions/722240/instantly-detect-client-disconnection-from-server-socket
        private static bool IsDisconnected(TCPClient workingClient)
        {
            try
            {
                Socket? s = workingClient.TcpClient?.Client;
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
            foreach (TCPClient workingClient in tcpClientList.ToArray())
            {
                if (IsDisconnected(workingClient))
                {
                    logger.ReportInfo($"\t\t\tClient {workingClient.TcpClient.Client.RemoteEndPoint} has left.");

                    // cleanup on our end
                    tcpClientList.Remove(workingClient);     // Remove from list
                    ReleaseClient(workingClient);
                }
            }
        }

        // cleans up resources for a TcpClient
        private static void ReleaseClient(TCPClient workingClient)
        {
            workingClient.TcpClient.GetStream().Close(); // Close network stream
            workingClient.TcpClient.Close();             // Close client
        }

        private List<TcpClient> tcpClients = new List<TcpClient>();
        private TcpListener tcpListener;
        private bool listening = true;

        private void HandleNewConnection(bool isSecure = false)
        {
            var newClient = new TCPClient()
            {
                TcpClient = tcpListener.AcceptTcpClient(),
                IsSecure = isSecure
            };

            try
            {
                tcpClientList.Add(newClient);
                logger.ReportInfo($"\t\t\tAccepted new client from {newClient.RemoteEndPoint}");
            }
            catch (Exception ex)
            {
                logger.ReportWarning($"Failed to add endpoint {newClient.RemoteEndPoint} to list.  {ex.Message}");
            }
        }

        // See if any of our messengers have sent us a new message, put it in the queue
        private void CheckForNewMessages()
        {
            foreach (var tcpClient in tcpClientList)
            {
                if (tcpClient.TcpClient.Available > 0 && !tcpClient.InUse)
                {
                    //logger.ReportDebug($"{tcpClient.CanRead} and {tcpClient.ClearStream.DataAvailable} and {IsDisconnected(tcpClient)} and {tcpClient.InUse}");
                    var data = tcpClient.Read();
                    //Task.Run(() => ProcessRecord(tcpClient));
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
                logger.ReportWarning($"A socket conflict occurred: {se.Message}");
                logger.ReportWarning($"Port Failure: {port}.");
                throw;
            }
            catch (Exception se)
            {
                logger.ReportWarning($"An error occurred while running the socket listener: {se.Message}");
                throw;
            }
            finally
            {
                // Stop the server, and clean up any connected clients
                foreach (TCPClient tcpClient in tcpClientList)
                {
                    ReleaseClient(tcpClient);
                }

                tcpListener.Stop();
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion Stateful Listener
    }
}