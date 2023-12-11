using DataShapes.Model;
using Confluent.Kafka;
using Collectors.Interface;
using Transporters.Interface;
using TransformerFactory.Interface;


namespace Collectors.Model
{
    public abstract class Collector<T> : ICollector,  IDisposable
    {
#region strings
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
#endregion strings

        protected IConfiguration? config;
        protected CollectorConfig? collectorconfig;

        protected ITransporter transporter;
        protected ITransformer<OEntity,IEntity> transformer;

        protected void GetApplicationConfig(string configname)
        {
            config = new ConfigurationBuilder()
              .AddJsonFile(configname)
              .Build();
        }

#region MQSetup
        // Message consumer, commands from controller
        protected ConsumerConfig kcconfig = new()
        {
            GroupId = "Collectors",
            BootstrapServers = "palisaid:9002",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        protected IConsumer<string, string> consumer;

        // Message producer, data to controller
        protected ProducerConfig kpconfig = new()
        {
            BootstrapServers = "palisaid:9002"
        };
        protected IProducer<string, string> producer;
#endregion MQSetup

        public Collector(Guid tenantid)
        {
            GetApplicationConfig("collectorsettings.json");

            TenantId = tenantid.ToString();

            consumer = new ConsumerBuilder<string, string>(kcconfig).Build();
            consumer.Subscribe("CollectorControl");

            producer = new ProducerBuilder<string, string>(kpconfig).Build();
        }

        
        // Configure the system on input fron the controller
        protected virtual async Task WaitForCommand()
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
                            await Task.Run(() => Environment.Exit(0));
                            break;

                        default:
                            break;
                    }
                }
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public virtual async Task RegisterCollector(CollectorConfig collectorconfig)
        {
            this.collectorconfig = collectorconfig;
            throw new NotImplementedException(nameof(RegisterCollector));
        }

        public virtual async Task Start()
        {
            // Launch Transporter - Create the proper transporter based on the config
            // Launch Scheduler   - Create the proper scheduler based on the config
            // Launch Transformer - Create the proper transformer based on the config
            throw new NotImplementedException(nameof(Start));
        }

        public virtual async Task ShutDown()
        { 
            throw new NotImplementedException(nameof(ShutDown));
        }

        public virtual async Task Panic()
        { 
            throw new NotImplementedException(nameof(Panic));
        }

        public virtual async Task Persist()
        { 
            throw new NotImplementedException(nameof(Persist));
        }

        public virtual async Task Connect()
        {
            throw new NotImplementedException(nameof(Connect));
        }

        public virtual async Task Deploy()
        {
            throw new NotImplementedException(nameof(Deploy));
        }

        public virtual async Task Configure()
        {
            throw new NotImplementedException(nameof(Configure));
        }

        public virtual async Task<string> Retrieve()
        {
            throw new NotImplementedException(nameof(Retrieve));
        }

        public virtual async Task Destroy()
        {
            throw new NotImplementedException(nameof(Destroy));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}