using DataShapes.Model;
using Confluent.Kafka;

namespace Collectors.CollectorBase.Model
{
    public abstract class Collector
    {
        // Message consumer, commands from controller
        private ConsumerConfig kcconfig = new()
        {
            GroupId = "Collectors",
            BootstrapServers = "palisaid:9002",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        // Message producer, data to controller
        private ProducerConfig kpconfig = new()
        {
            BootstrapServers = "palisaid:9002"
        };

        public Collector(Guid tenantid)
        {
            TenantId = tenantid;
        }

		// Reporting Header Data
		public string Name { get; set; } = "CollectorBase";
		public string Version { get; set; } = "0.0.1";
		public string Description { get; set; } = "CollectorBase is a collector that collects data from a source.";
		public string Author { get; set; } = "The Palisaid Backend Team";
		public string Url { get; set; } ="localhost";
		public Guid TenantId { get; set; }
		public string? TenantName { get; set; }	= "Palisaid";
		public string? TenantDescription { get; set; } = "Palisaid is a health information exchange.";

        private Scheduler? scheduler;
        private CollectorConfig? config;
        private DataShapeContext? context;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task RegisterCollector(CollectorConfig config)
        {
            this.config = config;
        }

        public async Task Start()
        { }

        public async Task ShutDown()
        { }

        public async Task PanicStop()
        { }

        public async Task Persist()
        { }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}