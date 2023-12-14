using Confluent.Kafka;
using Support.Interface;
using Support.Model;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using Task = System.Threading.Tasks.Task;

namespace Transporters.Model
{
    public class Fhir4Client : Transporter
    {
        private IBaseEventLogger logger = new BaseEventLogger(nameof(Fhir4Client));
        private FhirClient client;

        public bool secure { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? address { get; set; }
        public string? port { get; set; }

        public byte[] ApiKey { get; set; }
        public X509Certificate2 Certificate { get; set; }

        public Fhir4Client()
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
                                    await producer.ProduceAsync("fhir4records", new Message<string, string> { Key = GetHash(message), Value = message });
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
        
            return; //Task.CompletedTask;
        }

        private FhirClient InternalConnect()
        {
            try
            {
                if (secure)
                {
                    // register certificate
                }

                var settings = new FhirClientSettings
                {
                    Timeout = 0,
                    PreferredFormat = ResourceFormat.Json,
                    VerifyFhirVersion = true,
                    ReturnPreference = ReturnPreference.Minimal
                };

                client = new FhirClient(new Uri(address), settings);
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
        public Bundle? Read(string bundleid)
        {
            using (var client = InternalConnect())
            {
                return client.Read<Bundle>(bundleid);
            }
        }

        public override void Dispose()
        {
            client.Dispose();
        }
    }
}