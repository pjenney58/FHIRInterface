
using Support.Interface;
using Support.Model;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using Task = System.Threading.Tasks.Task;
using PalisaidMeta.Model;
using Confluent.Kafka;

namespace Transporters.Model
{ 
    public class Fhir4Client : Transporter
    {
        private IBaseEventLogger logger = new BaseEventLogger(nameof(Fhir4Client));
        private FhirClient client;

        CollectorConfig cconfig;

        public Fhir4Client(CollectorConfig cconfig, Guid tenantid, string commandbus, string payloadbus) 
            : base(cconfig, tenantid, commandbus, payloadbus)
        {
            this.cconfig = cconfig;
        }

        public bool secure { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? address { get; set; }
        public string? port { get; set; }

        public byte[]? ApiKey { get; set; }
        public X509Certificate2 Certificate { get; set; }

        public async override Task Connect()
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
                    ReturnPreference = ReturnPreference.Minimal,
                    
                };

                await Task.Run(() => client = new FhirClient(cconfig.TargetUri, settings));
            }
            catch (Exception ex)
            {
                throw new Exception(logger.ReportError(ex.Message, false));
            }
        }

        public async override Task<IEnumerable<string>> Read()
        {
            // Break the bundle into individual messages -- Should we do it here or in the transformer?
            throw new NotImplementedException();
        }

        public async Task Authenticate()
        {
            throw new NotImplementedException();
        }

        public async Task Cancel(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            if(client != null)
            {
                client.Dispose();
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Return IEnumerable<string> of messages if the message contains '\xB' and '\x1C', else just return the message.
        /// </summary>
        /// <returns>IEnumerable[string]</returns>
        public Bundle? Read(string bundleid)
        {        
            if(client != null)
            {
                return client.Read<Bundle>(bundleid);    
            }

            return default;    
        }

        public override void Dispose()
        {
            client.Dispose();
        }
    }

}