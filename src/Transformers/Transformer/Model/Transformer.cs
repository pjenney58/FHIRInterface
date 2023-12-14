// Transformer role
//  1. Monitors MQ for messages and processes them
//  2. Gets messages from MQ creates the proper transformation using the TransformerFactory
//  3. Returns converted data to the MQ

using Confluent.Kafka;
using DataShapes.Model;
using Microsoft.Extensions.Configuration;


namespace Transformers.Model
{
    public class Transformer : IDisposable
    {
        IConfiguration? config;
        Guid TenantId = Guid.Empty;
        ITransformer? transformer;

        protected void GetApplicationConfig(string configname)
        {
           config = new ConfigurationBuilder()
              .AddJsonFile(configname)
              .Build();
        }

        // Setup for MQ
 #region MQSetup
        // Message consumer, commands from controller        
        protected ConsumerConfig kcconfig = new()
        {
            GroupId = "Transformers",
            BootstrapServers = "palisaid:9002",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        protected IConsumer<string, string> consumer;
        protected IConsumer<string, TransformerPayload> transformerconsumer;

        // Message producer, data to controller
        protected ProducerConfig kpconfig = new()
        {
            BootstrapServers = "palisaid:9002"
        };
        protected IProducer<string, string> producer;
#endregion MQSetup

        public Transformer(Guid tenantid)
        {
           // GetApplicationConfig("transformersettings.json");

            TenantId = tenantid;
#if !DEBUG
            // Setup command topic
            consumer = new ConsumerBuilder<string, string>(kcconfig).Build();
            consumer.Subscribe("CommandControl");

            // Setup transform topic
            transformerconsumer = new ConsumerBuilder<string, TransformerPayload>(kcconfig).Build();
            transformerconsumer.Subscribe("TransformerControl");

            // Setup data out topic
            producer = new ProducerBuilder<string, string>(kpconfig).Build();


            TaskFactory taskFactory = new TaskFactory();
            var commandtask = taskFactory.StartNew(() => WaitForCommandRequest());
            var transfortmtask = taskFactory.StartNew(() => WaitForTransformRequest());
#endif
        }

        public async Task<object?> Transform(TransformerPayload payload)
        {
            try
            {
                // Get the detailed Create to match the payload parameters
                var transformer = typeof(TransformerFactory)
                                     .GetMethods().First(w => w.Name == "Create" && w.GetParameters().Count() > 2)
                                     .MakeGenericMethod(payload.Type1, payload.Type2)
                                     .Invoke(this, new object[] { TenantId, payload.Format, payload.Version, payload.SourceHost }) as ITransformer;

                if (transformer == null)
                {
                    throw new ArgumentNullException("transformer");
                }

                return await transformer.Transform(payload.data);
            }
            finally
            { }
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
                    var result = await Transform(payload.Message.Value);
                    var message = new Message<string, string> { Key = payload.Message.Key, Value = result.ToString() };
                    producer.Produce("TransformedData", message);
                }
            }
        
            return;
        }

        // Configure the system on input fron the controller
        protected virtual async Task WaitForCommandRequest()
        {
            bool cancelled = false;
            CancellationToken cancellationToken = new CancellationToken();
            
            // Listening for configuration commands
            using (consumer)
            {
                while (!cancelled)
                {
                    var payload = consumer.Consume(cancellationToken);
                    
                    // Process the command
                    switch (payload.Message.Value)
                    {
                        case "Read":                 
                            cancelled = true;
                            break;

                        case "Stop":
                            cancelled = true;
                            break;
                        case "Start":
                            cancelled = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        
            return;
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        // Setup for Transformation Requests
        // Setup for Transformation Responses
    }
}