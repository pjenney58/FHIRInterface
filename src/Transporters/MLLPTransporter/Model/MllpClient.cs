using Confluent.Kafka;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PalisaidMeta.Model;
using Support.Interface;
using Support.Model;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Transporters.Model
{
    public class MllpClient : Transporter
    {
        private IBaseEventLogger logger = new BaseEventLogger(nameof(MllpClient));
        private TCPClient? client;

        public bool secure { get; set; }
        public string? username { get; set; } = string.Empty;
        public string? password { get; set; } = string.Empty;
        public string? address { get; set; }
        public string? port { get; set; }

        public byte[] ApiKey { get; set; } = null!;
        public X509Certificate2? Certificate { get; set; }

        public MllpClient(CollectorConfig cconfig, Guid tenantid, string commandbus, string payloadbus) 
                       : base(cconfig, tenantid, commandbus, payloadbus)
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
     /*   public  override IEnumerable<string> Read()
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
        */
        public async override Task<string> Write(string message)
        {
            using (var client = InternalConnect())
            {
                client.Write($"\xB{message}\x1C\x0D");
            }

            return message;
        }

        public override void Dispose()
        {
            client.Dispose();
        }
    }
}