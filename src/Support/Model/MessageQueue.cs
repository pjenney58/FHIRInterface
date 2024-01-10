using System.Diagnostics;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Support.Model
{
    public delegate void MQCommandHandler(string command);
    public delegate void MQTransformHandler(TransformerPayload payload);


    public abstract class PalisaidMessageQueue : IDisposable
	{
        IConfiguration? config;      

        protected void GetApplicationConfig(string configname)
        {
            config = new ConfigurationBuilder()
               .AddJsonFile(configname)
               .Build();
        }

        private readonly string username = string.Empty;
        private readonly string password = string.Empty;

        protected readonly string commandbus = string.Empty;
        protected readonly string payloadbus = string.Empty;
        protected Guid tenantid = Guid.Empty;

        protected SubscriptionResult? payloadSubscription;
        protected SubscriptionResult? commandSubscription;

        protected TaskFactory? taskFactory = null;
        protected Task? commandTask;
        protected Task? transformTask;

        protected bool running = false;
        protected bool cancelled = false;

        MQCommandHandler? mqcommandhandler;
        MQTransformHandler? mqtransformhandler;

        public PalisaidMessageQueue(Guid tenantid, string commandbus, string payloadbus, string? username = null, string? password = null)
		{   
            GetApplicationConfig("messagequeuesettings.json");

            this.tenantid = tenantid;
            this.commandbus = commandbus;
            this.payloadbus = payloadbus;

            this.username = string.IsNullOrEmpty(username) ? config["Username"] : username;
            this.password = string.IsNullOrEmpty(password) ? config["Password"] : password;

            mqcommandhandler = DefaultCommandHandler;
            mqtransformhandler = DefaultTransformHandler;

            taskFactory = new TaskFactory();
            
            Trace.WriteLine("Starting Command Handler Task");
            taskFactory.StartNew(() => WaitForCommandRequest());

            Trace.WriteLine("Starting Transform Request Handler");
            taskFactory.StartNew(() => WaitForTransformRequest());

        }

        private void DefaultTransformHandler(TransformerPayload payload)
        {
            throw new NotImplementedException();
        }

        private void DefaultCommandHandler(string command)
        {
            throw new NotImplementedException();
        }

        protected virtual void RegisterCommmandHandler(MQCommandHandler mqcommandhandler)
        {
            this.mqcommandhandler = mqcommandhandler ?? throw new ArgumentNullException(nameof(MQCommandHandler));
        }

        protected virtual void RegisterTransformHandler(MQTransformHandler mqtransformhandler)
        {
            this.mqtransformhandler = mqtransformhandler ?? throw new ArgumentNullException(nameof(MQTransformHandler));
        }        

        protected virtual void WaitForTransformRequest()
        {
            // Listening for configuration commands
            using (var bus = RabbitHutch.CreateBus(AppRunningIn.Docker ? "host=rabbitmq" : "host=localhost" ))
            {
                payloadSubscription = bus.PubSub.Subscribe<TransformerPayload>(payloadbus, new Action<TransformerPayload>(mqtransformhandler ?? throw new ArgumentNullException(nameof(mqcommandhandler))));
                Debug.WriteLine("Listening for transform messages");
            }

            return;
        }

        protected virtual void WaitForCommandRequest()
        {      
            using (var bus = RabbitHutch.CreateBus(AppRunningIn.Docker ? "host=rabbitmq" : "host=localhost"))
            {
                commandSubscription = bus.PubSub.Subscribe<string>(commandbus, new Action<string>(mqcommandhandler ?? throw new ArgumentNullException(nameof(mqcommandhandler))));
                Debug.WriteLine("Listening for command messages");
            }

            return;
        }

        protected virtual void Stop()
        {
            if (running)
            {
                Dispose(commandSubscription);
                Dispose(payloadSubscription);
                running = false;
            }
        }

        protected virtual void Start()
        {
            if (!running)
            {
                if (taskFactory == null)
                {
                    taskFactory = new TaskFactory();
                }

                if (taskFactory != null)
                {
                    Trace.WriteLine("Starting Command Handler Task");
                    taskFactory.StartNew(() => WaitForCommandRequest());

                    Trace.WriteLine("Starting Transform Request Handler");
                    taskFactory.StartNew(() => WaitForTransformRequest());

                    running = true;
                    cancelled = false;
                }
                else
                {
                    throw new ArgumentNullException("taskFactory null");
                }
            }
        }

        protected virtual void Panic()
        {
            cancelled = true;
            Dispose();
            Environment.FailFast("Panic");
        }

        protected virtual void ShutDown()
        {
            Dispose();
            Environment.Exit(0);
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
    }
}

