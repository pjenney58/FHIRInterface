using Support.Interface;
using Support.Model;
using System.Security.Cryptography.X509Certificates;

namespace Transporter.Model
{
    public class MllpClient : Transporter
    {
        private IBaseEventLogger logger = new BaseEventLogger(nameof(MllpClient));
        private TCPClient client;

        public bool secure { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? address { get; set; }
        public string? port { get; set; }

        public byte[] ApiKey { get; set; }
        public X509Certificate2 Certificate { get; set; }

        public MllpClient()
        { }

        private TCPClient InternalConnect()
        {
            try
            {
                if (secure)
                {
                    // register certificate
                }

                client = new TCPClient(address, port, secure);
            }
            catch (Exception ex)
            {
                throw new Exception(logger.ReportError(ex.Message, false));
            }

            return client;
        }

        public Task Authenticate()
        {
            throw new NotImplementedException();
        }

        public Task Cancel(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Connect()
        {
            client = InternalConnect();
            return Task.CompletedTask;
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return IEnumerable<string> of messages if the message contains '\xB' and '\x1C', else just return the message.
        /// </summary>
        /// <returns>IEnumerable[string]</returns>
        public override IEnumerable<string> Read()
        {
            using (var client = InternalConnect())
            {
                var message = client.Read();

                if (message.Contains('\x1C') && message.Contains('\xB'))
                {
                    var messages = message.Split('\xB', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var msg in messages)
                    {
                        yield return msg.Remove(msg.IndexOf('\x1C'));
                    }
                }

                yield return message;
            }
        }

        public override Task Write(string message)
        {
            using (var client = InternalConnect())
            {
                client.Write($"\xB{message}\x1C\x0D");
            }

            return Task.CompletedTask;
        }
    }
}