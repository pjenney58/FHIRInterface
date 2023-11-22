using System;
using Collector.Interface;
using Confluent.Kafka;

namespace Collector.Model
{
	public class Fhir5Collector : ICollector
	{
        ConsumerConfig kconfig = new()
        {
            GroupId = "Collectors",
            BootstrapServers = "localhost:9002",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

		public Fhir5Collector()
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

