using System;
using Collector.Interface;
using Collectors.CollectorBase.Model;

namespace Collector.Model
{
	public class Fhir2Collector : ICollector
    {
		public Fhir2Collector()
		{
		}

        public Task Configure()
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

        public Task<string> Retrieve()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task Connect()
        {
            throw new NotImplementedException();
        }

        Task<string> ICollector.Retrieve()
        {
            throw new NotImplementedException();
        }
    }
}

