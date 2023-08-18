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

namespace Hl7Harmonizer.Transport.Model
{
    public class WorkingClient : IDisposable
    {
        private readonly IBaseEventLogger logger = new BaseEventLogger("WorkingClient");

        /// <summary>
        /// <c> WorkingClient </c> Abstraction for a Encrypted or Cleartext TcpClient. Instation Example:
        ///
        /// using (var client = new WorkingClient("localhost", "8080", true, "Fred"))) { var stream
        /// = client.GetStream(); // DoWork }
        /// </summary>
        /// <param name="address"> Actual address as URL or IPv4 or IPv6 </param>
        /// <param name="secure"> </param>
        /// <param name="name"> </param>
        public WorkingClient(string? address, string? port, bool secure = false, string? name = null)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(port))
            {
                throw new ArgumentNullException($"{nameof(address)} or {nameof(port)}");
            }

            try
            {
                //IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
                //IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, int.Parse(port));

                tcpClient = new TcpClient(address, int.Parse(port));
                IsSecure = secure;

                LocalEndPoint = tcpClient.Client.LocalEndPoint;
                RemoteEndPoint = tcpClient.Client.RemoteEndPoint;
            }
            catch (Exception ex)
            {
                logger.ReportError($"Failed to create WorkingClient: {ex.Message}");
                throw;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = Guid.NewGuid().ToString();
            }

            byteIoBuffer = new byte[4096];
        }

        public WorkingClient()
        {
            byteIoBuffer = new byte[4096];
        }

        /// <summary>
        /// Connection Name
        /// </summary>
        public string? Name { get; set; }

        private TcpClient? tcpClient;

        /// <summary>
        /// Actual TCP client
        /// </summary>
        public TcpClient? TcpClient
        {
            get => tcpClient;
            set => tcpClient = value ?? new TcpClient();
        }

        private bool issecure;

        public bool IsSecure
        {
            get => issecure;
            set
            {
                if (tcpClient == null)
                {
                    throw new InvalidOperationException("Attempt to set security with null client");
                }

                issecure = value;
                Stream = issecure ? new SslStream(tcpClient.GetStream()) : tcpClient.GetStream();
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

        private void SecureWriteCallback(IAsyncResult ar)
        {
            if (ar == null || ar.AsyncState == null)
            {
                throw new ArgumentNullException(nameof(ar));
            }

            logger.ReportTrace("Writing to SecureStream");

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
            logger.ReportTrace("Reading from SecureStream");

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
            logger.ReportTrace("Writing to ClearStream");

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
            logger.ReportTrace("Reading from ClearStream");
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

        public string ReadAsync()
        {
            if (Stream == null)
            {
                throw new InvalidOperationException("I/O stream is null");
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

        public int WriteAsync(string data)
        {
            if (Stream == null)
            {
                throw new InvalidOperationException("I/O stream is null");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("Data argument is null");
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
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}