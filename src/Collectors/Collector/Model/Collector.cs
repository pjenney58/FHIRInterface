using Collectors.Interface;
using EasyNetQ;
using PalisaidMeta.Model;
using Support.Model;
using System.Diagnostics;
using Transformers.Interface;
using Transformers.Model;
using Transporters.Interface;
using Transporters.Model;
using Task = System.Threading.Tasks.Task;

namespace Collectors.Model
{
    public abstract class Collector<T> : PalisaidMessageQueue, ICollector, IDisposable
    {
        #region strings

        private readonly string Name = "Name";
        private readonly string Version = "Version";
        private readonly string Description = "Description";
        private readonly string Author = "Author";
        private readonly string Url = "Url";
        private readonly string TenantId = Guid.Empty.ToString();
        private readonly string TenantName = "TenantName";
        private readonly string TenantDescription = "TenantDescription";
        private readonly string Transform = "Transform";

        private Dictionary<string, string> _parameters = new()
        {
            { "Name", "value" },
            { "Version", "value" },
            { "Description", "value" },
            { "Author", "value" },
            { "Url", "value" },
            { "TenantId", "value" },
            { "TenantName", "value" },
            { "TenantDescription", "value" },
            { "Transform", "value" }
        };

        #endregion strings

        protected IConfiguration? config;
        protected CollectorConfig? collectorconfig;
        protected List<ITransporter?> transporters = new();
        protected List<ITransformer?> transformers = new();
        protected IScheduler? scheduler;

        private SubscriptionResult? payloadSubscription;
        private SubscriptionResult? commandSubscription;

        private TaskFactory? taskFactory = null;
        private Task? commandTask;
        private Task? transformTask;

        private bool running = false;
        private bool cancelled = false;

        public Collector(Guid tenantid, string name)
                  : base(tenantid, $"command-{name}", $"transform-{name}")
        {
            config = AppConfig.Get("collectorsettings.json");

            // Setup MQ channels
            Trace.WriteLine("Registering Collector Transformer[0]");
            RegisterTransformer(tenantid, $"command-{name}", $"transform-{name}");

            Trace.WriteLine("Registering Collector Transporter[0]");
            RegisterTransporter();

            // Register Command Handler
            Trace.WriteLine("Registering Collector Command Handler");
            RegisterCommmandHandler(ProcessCommand);

            // Register Trasform Handler
            Trace.WriteLine("Registering Transform Handler");
            RegisterTransformHandler(ProcessTransform);

            // Get things rolling
            Trace.WriteLine("Starting Collector");
            ProcessCommand("Start");
        }

        public void RegisterTransporter(CollectorConfig cconfig, Guid tenantid, string commandbus, string payloadbus)
        {
            // Create the transporter
            var transporter = new Transporter(cconfig, tenantid, commandbus, payloadbus) as ITransporter;

            // Add the transporter to the list
            transporters.Add(transporter);
        }

        public void RegisterTransformer(Guid tenantid, string commandbus, string payloadbus)
        {
            // Create the transformer
            var transformer = new Transformer(tenantid, commandbus, payloadbus) as ITransformer;

            // Add the transformer to the list
            transformers.Add(transformer);
        }

        private void ProcessCommand(string command)
        {
            Debug.WriteLine($"Collector Command Request: {command}");

            switch (command.ToUpperInvariant())
            {
                case "SHUTDOWN":
                    Dispose();
                    Environment.Exit(0);
                    break;

                case "PANIC":
                    cancelled = true;
                    Dispose();
                    Environment.FailFast("Panic");
                    break;

                case "STOP":
                    if (running)
                    {
                        Dispose(commandSubscription);
                        Dispose(payloadSubscription);
                        running = false;
                    }
                    break;

                case "START":
                    if (!running)
                    {
                        if (taskFactory == null)
                        {
                            taskFactory = new TaskFactory();
                        }

                        commandTask = taskFactory.StartNew(() => WaitForCommandRequest());
                        transformTask = taskFactory.StartNew(() => WaitForTransformRequest());
                        running = true;
                    }

                    break;

                default:
                    break;
            }
        }

        private void ProcessTransform(TransformerPayload payload)
        {
            Debug.WriteLine($"Collector Transform Request: {payload}");
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public virtual async Task RegisterCollector(CollectorConfig collectorconfig)
        {
            Debug.WriteLine($"Registering Collector");
            this.collectorconfig = collectorconfig;
            //throw new NotImplementedException(nameof(RegisterCollector));
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

        protected void Dispose(SubscriptionResult? subscription)
        {
            subscription?.Dispose();
        }

        public void Dispose()
        {
            Dispose(commandSubscription);
            Dispose(payloadSubscription);

            commandTask?.Dispose();
            transformTask?.Dispose();
        }

        public Task RegisterCollector()
        {
            throw new NotImplementedException();
        }

        public Task RegisterTransporter()
        {
            throw new NotImplementedException();
        }

        public Task RegisterTransformer(DataProtocol dataProtocolIn)
        {
            throw new NotImplementedException();
        }

        public Task RegisterScheduler()
        {
            throw new NotImplementedException();
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}