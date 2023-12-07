
using Collectors.Interface;
using Transporters.Model;

namespace Collectors.Model
{
	public class HL7v2Collector : ICollector
	{
        private readonly MllpClient client;

		public HL7v2Collector()
		{
            client = new MllpClient();     
		}

        public Task Configure()
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

