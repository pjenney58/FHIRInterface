using DataShapes.Model;
using Confluent.Kafka;
using Collectors.Interface;
using Transporters.Interface;
using TransformerFactory.Model;
using Transformers.Model;

using Task = System.Threading.Tasks.Task;

namespace Collectors.Model
{
    public abstract class Collector<T> : ICollector, IDisposable
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
        private string Transform = "Transform";

        private Dictionary<string, string> _parameters = new()
        {
            { "Name", "Value" },
            { "Version", "Value" },
            { "Description", "Value" },
            { "Author", "Value" },
            { "Url", "Value" },
            { "TenantId", "Value" },
            { "TenantName", "Value" },
            { "TenantDescription", "Value" },
            { "Transform", "Value" }
        };
#endregion strings

        protected IConfiguration? config;
        protected CollectorConfig? collectorconfig;

        protected ITransporter transporter;
        

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

        protected struct TransformerPayload
        {
            public Type Type1 { get; set; }
            public Type Type2 { get; set; }
            public HL7Format Format { get; set; }
            public Hl7Version Version { get; set; }
            public SourceSystems Src { get; set; }
            public string data { get; set; }
        }

        protected IConsumer<string, string> consumer;
        protected IConsumer<string, TransformerPayload> transformerconsumer;

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

            // Setup command topic
            consumer = new ConsumerBuilder<string, string>(kcconfig).Build();
            consumer.Subscribe("CollectorControl");

            // Setup transform topic
            transformerconsumer = new ConsumerBuilder<string, TransformerPayload>(kcconfig).Build();
            transformerconsumer.Subscribe("TransformerControl");

            // Setup data out topic
            producer = new ProducerBuilder<string, string>(kpconfig).Build();

            TaskFactory taskFactory = new TaskFactory();
            taskFactory.StartNew(() => WaitForCommand());
            taskFactory.StartNew(() => WaitForTransformRequest());
        }

        public T ConvertObject<T>(object input) {
            return (T) Convert.ChangeType(input, typeof(T));
        }
        
        protected virtual async Task WaitForTransformRequest()
        {
            bool cancelled = false;
            CancellationToken cancellationToken = new CancellationToken();
            
            // Listening for configuration commands
            using (transformerconsumer)
            {
                while (!cancelled)
                {
                    var payload = transformerconsumer.Consume(cancellationToken);
                    
                    var transform = new Transformer(TenantId) ;
                    var result = await transform.Transform(payload.data);

                    /*
                    var transformer = TransformerFactory.GetTransformer<t1,t2>(Guid.Parse(TenantId), 
                                                                               payload.Message.Value.Format, 
                                                                               payload.Message.Value.Version, 
                                                                               payload.Message.Value.Src);
                    
                    var result = await transformer.Transform(payload.Message.Value.data);
                    */
                }
            }
        
            return;
        }

        // Configure the system on input fron the controller
        protected virtual async Task WaitForCommand()
        {
            bool cancelled = false;
            CancellationToken cancellationToken = new CancellationToken();
            
            // Listening for configuration commands
            using (consumer)
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
        
            return;
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