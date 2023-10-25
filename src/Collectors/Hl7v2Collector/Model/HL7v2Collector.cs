using System;
using Collector.Interface;

namespace Collector.Model
{
	public class HL7v2Collector : ICollector
	{
		public HL7v2Collector()
		{
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

        public Task<string> Retrieve()
        {
            throw new NotImplementedException();
        }
    }
}

