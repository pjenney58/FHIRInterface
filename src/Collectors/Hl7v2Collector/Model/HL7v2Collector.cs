
using Collectors.Interface;
using Confluent.Kafka;
using Transporters.Model;

namespace Collectors.Model
{
	public class HL7v2Collector : Collector<HL7v2Collector>
	{
        private readonly MllpClient? client;

        public HL7v2Collector() : base(Guid.Empty)
		{
		}

		public HL7v2Collector(Guid tenantid) : base(tenantid)
		{
            GetApplicationConfig("hl7v2settings.json");
            
            client = new MllpClient();     
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

