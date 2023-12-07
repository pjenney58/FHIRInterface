using DataShapes.Model;
using Confluent.Kafka;
using Collectors.Interface;

namespace Collectors.Model
{
    public abstract class Collector<T> : ICollector,  IDisposable
    {
        // Message consumer, commands from controller
        private ConsumerConfig kcconfig = new()
        {
            GroupId = "Collectors",
            BootstrapServers = "palisaid:9002",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        private IConsumer<string, string> consumer;

        // Message producer, data to controller
        private ProducerConfig kpconfig = new()
        {
            BootstrapServers = "palisaid:9002"
        };
        private IProducer<string, string> producer;

        public Collector(Guid tenantid)
        {
            TenantId = tenantid.ToString();

            consumer = new ConsumerBuilder<string, string>(kcconfig).Build();
            consumer.Subscribe("CollectorControl");

            producer = new ProducerBuilder<string, string>(kpconfig).Build();
        }

        private string Name = "Name";
        private string Version = "Version";
        private string Description = "Description";
        private string Author = "Author";
        private string Url = "Url";
        private string TenantId = Guid.Empty.ToString();
        private string TenantName = "TenantName";
        private string TenantDescription = "TenantDescription";

        private Dictionary<string, string> _parameters = new()
        {
            { "Name", "Value" },
            { "Version", "Value" },
            { "Description", "Value" },
            { "Author", "Value" },
            { "Url", "Value" },
            { "TenantId", "Value" },
            { "TenantName", "Value" },
            { "TenantDescription", "Value" }
        };

        private CollectorConfig? config;
        private DataShapeContext? context;

        // Configure the system on input fron the controller
        private async Task WaitForCommand()
        {
            bool cancelled = false;
            CancellationToken cancellationToken = new CancellationToken();

            using (var consumer = new ConsumerBuilder<string, string>(kcconfig).Build())
            {
                while (!cancelled)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    switch (consumeResult.Message.Key.ToLower())
                    {
                        case "name":
                            _parameters[Name] = consumeResult.Message.Value;
                            break;

                        case "version":
                            _parameters[Version] = consumeResult.Message.Value;
                            break;

                        case "description":
                            _parameters[Description] = consumeResult.Message.Value;
                            break;

                        case "author":
                            _parameters[Author] = consumeResult.Message.Value;
                            break;

                        case "url":
                            _parameters[Url] = consumeResult.Message.Value;
                            break;

                        case "tenantid":
                            _parameters[TenantId] = consumeResult.Message.Value;
                            break;

                        case "tenantname":
                            _parameters[TenantName] = consumeResult.Message.Value;
                            break;

                        case "tenantdescription":
                            _parameters[TenantDescription] = consumeResult.Message.Value;
                            break;

                        case "shutdown":
                            Environment.Exit(0);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task RegisterCollector(CollectorConfig config)
        {
            this.config = config;
        }

        public async Task Start()
        {
            // Launch Transporter - Create the proper transporter based on the config
            // Launch Scheduler   - Create the proper scheduler based on the config
            // Launch Transformer - Create the proper transformer based on the config
        }

        public async Task ShutDown()
        { }

        public async Task Panic()
        { }

        public async Task Persist()
        { }

        public Task Connect()
        {
            throw new NotImplementedException();
        }

        public Task Deploy()
        {
            throw new NotImplementedException();
        }

        public Task Configure()
        {
            throw new NotImplementedException();
        }

        public Task<string> Retrieve()
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


#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}