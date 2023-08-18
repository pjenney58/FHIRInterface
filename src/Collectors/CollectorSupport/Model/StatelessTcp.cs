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

namespace Hl7Harmonizer.Transport.Model
{
    /// <summary>
    /// TCP Socket level transport for HL7 v2 support
    /// </summary>
    public class StatelessTcp : ITransport
    {
        private TransportContext? Context;
        private TcpListener? tcpListener;
        private ManualResetEvent? tcpClientConnected;

        public StatelessTcp(TransportContext context)
        {
            if (context == null || context.Security == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Context = context;

            tcpListener = context.Security.listener;
            tcpClientConnected = context.Security.tcpClientConnected;
        }

        /// <summary>
        /// <c> ListenAsync </c> Listen for client connections, had off to stream reader
        /// </summary>
        public void ListenAsync(string? port)
        {
            if (string.IsNullOrEmpty(port))
            {
                throw new ArgumentNullException(nameof(port));
            }

            tcpClientConnected = new ManualResetEvent(false);
            tcpListener = new TcpListener(IPAddress.Any, int.Parse(port));
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(AcceptClient, tcpListener);

            while (true)
            {
                tcpClientConnected.Reset();
                tcpListener.BeginAcceptTcpClient(AcceptClient, tcpListener);
                tcpClientConnected.WaitOne();
            }
        }

        private void AcceptClient(IAsyncResult asyncResult)
        {
            tcpListener = asyncResult.AsyncState as TcpListener;

            var workingClient = new WorkingClient()
            {
                TcpClient = tcpListener?.EndAcceptTcpClient(asyncResult),
                IsSecure = Context.IsSecure,
            };

            // Release the listener
            tcpClientConnected?.Set();

            if (workingClient == null || workingClient.TcpClient == null)
            {
                throw new InvalidOperationException(nameof(workingClient));
            }

            var waiting = 1000;
            while (workingClient.TcpClient.Available == 0 && waiting > 0)
            {
                Thread.Sleep(10);
                waiting = waiting - 10;
            }

            Context?.logger.ReportInfo($"Waited for {1000 - waiting}ms");

            if (workingClient.TcpClient.Available > 0)
            {
                // TODO: Create Record Processor Delegate
            }
        }

        public Task<T?> Delete<T>(string route, string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <c> Get </c> Generic TCP Read
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="route"> </param>
        /// <param name="id"> </param>
        /// <returns> </returns>
        /// <exception cref="NotImplementedException"> </exception>
        public Task<T?> Get<T>(string route, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>?> Get<T>(string route)
        {
            throw new NotImplementedException();
        }

        public Task<T?> Post<T>(T data, string route)
        {
            throw new NotImplementedException();
        }

        public Task<T?> Put<T>(T data, string route, string id)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Listen(string port, bool isSecure = false)
        {
            throw new NotImplementedException();
        }

        public void ListenAsync(string port, bool isSecure = false)
        {
            throw new NotImplementedException();
        }
    }
}