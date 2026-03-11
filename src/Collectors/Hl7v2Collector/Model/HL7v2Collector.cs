using PalisaidMeta.Model;
using Transporters.Model;

namespace Collectors.Model
{
    public class HL7v2Collector : Collector<HL7v2Collector>
    {
        private readonly MllpClient? client;
        private readonly PalisaidMetaContext? context = new PalisaidMetaContext();

        public HL7v2Collector()
            : base(Guid.Empty, nameof(HL7v2Collector))
        {
        }

        public HL7v2Collector(Guid tenantid)
            : base(tenantid, nameof(HL7v2Collector))
        {
            GetApplicationConfig("hl7v2settings.json");

            var collectorconfig = context?.Collectors.Find(tenantid);

            if (collectorconfig != null && collectorconfig.DataProtocolIn == DataProtocol.HL7v2 && collectorconfig.IsActive)
            {
                client = new MllpClient(collectorconfig,
                                        tenantid,
                                        $"{nameof(HL7v2Collector)}Command-{DateTimeOffset.Now.Millisecond}",
                                        $"{nameof(HL7v2Collector)}Payload-{DateTimeOffset.Now.Millisecond}");
            }
            else
            {
                throw new Exception($"Collector Configuration {nameof(HL7v2Collector)} not found for {tenantid}");
            }
        }

        public override async Task Configure()
        {
            throw new NotImplementedException();
        }

        public Task Connect()
        {
            throw new NotImplementedException();
        }

        public Task Deploy()
        {
            throw new NotImplementedException();
        }

        public Task Destroy()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task Panic()
        {
            throw new NotImplementedException();
        }

        public Task Persist()
        {
            throw new NotImplementedException();
        }

        public Task<string> Retrieve()
        {
            throw new NotImplementedException();
        }
    }
}