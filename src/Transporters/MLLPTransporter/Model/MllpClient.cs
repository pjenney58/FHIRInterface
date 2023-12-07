using Confluent.Kafka;
using Support.Interface;
using Support.Model;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Transporters.Model
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

        private async Task WaitForCommand()
        {
            bool cancelled = false;
            CancellationToken cancellationToken = new CancellationToken();

            using (var consumer = new ConsumerBuilder<Null, string>(kcconfig).Build())
            {
                // Wait for a call to action, the collector will send a message to poll
                consumer.Subscribe("schedular");

                while (!cancelled)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    switch (consumeResult.Message.Value.ToLower())
                    {
                        case "poll":
                            using (var producer = new ProducerBuilder<string, string>(kpconfig).Build())
                            {
                                foreach (var message in Read())
                                {
                                    await producer.ProduceAsync("hl7records", new Message<string, string> { Key = GetHash(message), Value = message });
                                }
                            }
                            break;

                        case "shutdown":
                            Environment.Exit(0);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

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

        public override void Dispose()
        {
            client.Dispose();
        }
    }
}