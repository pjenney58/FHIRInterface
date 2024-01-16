using Confluent.Kafka;
using Support.Interface;
using Support.Model;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Hl7.Fhir.Rest;
using Hl7.Fhir;
using Hl7.Fhir.Model;
using PalisaidMeta.Model;

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
        private CollectorConfig cconfig;

        public Fhir2Client(CollectorConfig cconfig, Guid tenantid, string commandbus, string payloadbus)
            : base(cconfig, tenantid, commandbus, payloadbus)
        {
            this.cconfig = cconfig;
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
    }
}