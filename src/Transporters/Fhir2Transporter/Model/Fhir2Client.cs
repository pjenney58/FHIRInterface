using Confluent.Kafka;
using Support.Interface;
using Support.Model;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Hl7.Fhir.Rest;
using Hl7.Fhir;
using Hl7.Fhir.Model;

namespace Transporters.Model
{
    public class Fhir2Client : Transporter
    {
        private IBaseEventLogger logger = new BaseEventLogger(nameof(Fhir2Client));
        private FhirClient client;

        public bool secure { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? address { get; set; }
        public string? port { get; set; }

        public byte[] ApiKey { get; set; }
        public X509Certificate2 Certificate { get; set; }

        public Fhir2Client()
        { }

        private async Task WaitForCommand()
        {
            bool cancelled = false;
            CancellationToken cancellationToken = new CancellationToken();

            using (var consumer = new ConsumerBuilder<Null, string>(kcconfig).Build())
            {
                // Wait for a call to action, the collector will send a message to poll
                consumer.Subscribe("fhir2schedular");

                while (!cancelled)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    switch (consumeResult.Message.Value.ToLower())
                    {
                        // Collect all records
                        case "poll":
                            using (var producer = new ProducerBuilder<string, string>(kpconfig).Build())
                            {
                                foreach (var message in Read())
                                {
                                    await producer.ProduceAsync("fhir2records", new Message<string, string> { Key = GetHash(message), Value = message });
                                }
                            }
                            break;


                        // Shutdown the transporter
                        case "shutdown":
                            Environment.Exit(0);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private FhirClient InternalConnect()
        {
           try
            {
                if (secure)
                {
                    // register certificate
                }

                client = new FhirClient(new Uri(address), true);
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
        public Resource Read(Uri id)
        {
            var client = InternalConnect();
           return client.Read<Resource>(id);                      
        }

        public override Task Write(string message)
        {
            client = InternalConnect();
            client.Update(message);
           
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            client.Dispose();
        }
    }
}