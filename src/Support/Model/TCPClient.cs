/*
* Software License - WorkingClient.cs
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
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace Support.Model
{
    public class TCPClient : IDisposable
    {
        private readonly IBaseEventLogger logger = new BaseEventLogger("WorkingClient");
        private const int buflen = 4096;
        private bool _isSecure;

        public bool IsSecure
        {
            get => _isSecure;
            set
            {
                if (tcpClient == null)
                {
                    throw new InvalidOperationException("NullClientSecurity");
                }

                _isSecure = value;

                Stream = _isSecure
                    ? new SslStream(tcpClient.GetStream())
                    : tcpClient.GetStream();

                CanRead = Stream.CanRead;
                CanWrite = Stream.CanWrite;
                Stream.WriteTimeout = 10000;
                Stream.ReadTimeout = 10000;
            }
        }

        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool InUse { get; set; }

        public EndPoint? RemoteEndPoint { get; set; }
        public EndPoint? LocalEndPoint { get; set; }

        /// <summary>
        /// Generic stream representing Celear or Secure connection
        /// </summary>
        public Stream? Stream;

        private byte[] byteIoBuffer;
        private int bytesRead;
        private int bytesWritten;
        private int bytesAvailable;
        private bool dataAvailable;
        private bool disposedValue;

        /// <summary>
        /// Connection Name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// <c> WorkingClient </c> Abstraction for a Encrypted or Cleartext TcpClient. Instation Example:
        ///
        /// using (var client = new WorkingClient("localhost", "8080", true, "Fred"))) { var stream
        /// = client.GetStream(); // DoWork }
        /// </summary>
        /// <param name="address"> Actual address as URL or IPv4 or IPv6 </param>
        /// <param name="secure"> </param>
        /// <param name="name"> </param>
        public TCPClient(string? address, string? port, bool secure = false, string? name = null)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(port))
            {
                throw new ArgumentNullException($"{nameof(address)} or {nameof(port)}");
            }

            try
            {
                //IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
                //IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, int.Parse(port));

                var tcpClient = new TcpClient(address, int.Parse(port));
                IsSecure = secure;

                LocalEndPoint = tcpClient.Client.LocalEndPoint;
                RemoteEndPoint = tcpClient.Client.RemoteEndPoint;
            }
            catch (Exception ex)
            {
                logger.ReportError($"FailedClientCreate {ex.Message}");
                throw;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = Guid.NewGuid().ToString();
            }

            byteIoBuffer = new byte[buflen];
        }

        public TCPClient()
        {
            byteIoBuffer = new byte[buflen];
        }

        private TcpClient? tcpClient;

        /// <summary>
        /// Actual TCP client
        /// </summary>
        public TcpClient? TcpClient
        {
            get => tcpClient;
            set => tcpClient = value ?? new TcpClient();
        }

        private void SecureWriteCallback(IAsyncResult ar)
        {
            if (ar == null || ar.AsyncState == null)
            {
                throw new ArgumentNullException(nameof(ar));
            }

            logger.ReportTrace("WritingSecureStream");

            SslStream? stream = ar.AsyncState as SslStream;

            if (stream == null)
            {
                bytesWritten = 0;
                dataAvailable = false;
                return;
            }

            stream.AuthenticateAsClient("localhost");
            stream.EndWrite(ar);
        }

        private void SecureReadCallback(IAsyncResult ar)
        {
            logger.ReportTrace("ReadingSecureStream");

            SslStream? stream = ar.AsyncState as SslStream;

            if (stream == null)
            {
                bytesRead = 0;
                dataAvailable = false;
                return;
            }

            stream.AuthenticateAsClient(stream.TargetHostName);

            var byteCount = stream.EndRead(ar);

            if (byteIoBuffer != null && byteCount > 0)
            {
                dataAvailable = byteCount == byteIoBuffer.Length;
                bytesRead = byteCount;
            }
        }

        private void ClearWriteCallback(IAsyncResult ar)
        {
            logger.ReportTrace("WritingClearStream");

            NetworkStream? stream = ar.AsyncState as NetworkStream;

            if (stream == null)
            {
                bytesRead = 0;
                dataAvailable = false;
                return;
            }

            stream.EndWrite(ar);
        }

        private void ClearReadCallback(IAsyncResult ar)
        {
            logger.ReportTrace("ReadingClearStream");
            dataAvailable = false;

            NetworkStream? stream = ar.AsyncState as NetworkStream;

            if (stream == null || !stream.CanRead)
            {
                bytesRead = 0;
                dataAvailable = false;
                return;
            }

            var byteCount = stream.EndRead(ar);
            if (byteIoBuffer != null && byteCount > 0)
            {
                bytesRead = byteCount;
                dataAvailable = byteCount == byteIoBuffer.Length;
            }
        }

        public string Read()
        {
            if (Stream == null)
            {
                throw new InvalidOperationException("IoStreamNull");
            }

            lock (byteIoBuffer)
            {
                var result = Stream.BeginRead(byteIoBuffer, 0, byteIoBuffer.Length, IsSecure ? SecureReadCallback : ClearReadCallback, Stream);
                result.AsyncWaitHandle.WaitOne();
            }

            if (dataAvailable)
            {
                return Encoding.UTF8.GetString(byteIoBuffer);
            }

            return string.Empty;
        }

        public int Write(string data)
        {
            if (Stream == null)
            {
                throw new InvalidOperationException("IoStreamNull");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("DataArgNull");
            }

            lock (byteIoBuffer)
            {
                byteIoBuffer = Encoding.UTF8.GetBytes(data);
                var result = Stream.BeginWrite(byteIoBuffer, 0, byteIoBuffer.Length, IsSecure ? SecureWriteCallback : ClearWriteCallback, Stream);
                result.AsyncWaitHandle.WaitOne();
            }

            return bytesWritten;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Stream != null)
                {
                    Stream.Dispose();
                }

                if (tcpClient != null)
                {
                    tcpClient.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}