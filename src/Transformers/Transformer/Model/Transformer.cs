// Transformer role
//  1. Monitors MQ for messages and processes them
//  2. Gets messages from MQ creates the proper transformation using the TransformerFactory
//  3. Returns converted data to the MQ

using System.Diagnostics;
using EasyNetQ;
using Microsoft.Extensions.Configuration;


namespace Transformers.Model
{
    public class Transformer : PalisaidMessageQueue, IDisposable
    {
        IConfiguration? config;      
        ITransformer? transformer;
     
        public Transformer(Guid tenantid, string commandbus, string payloadbus)
                    : base(tenantid, commandbus, payloadbus)
        {            
            // Get the configuration
            config = AppConfig.Get("transformersettings.json");

            // Register Command Handler
            Trace.WriteLine("Registering Command Handler");
            RegisterCommmandHandler(ProcessCommand);

            // Register Trasform Handler
            Trace.WriteLine("Registering Transform Handler");
            RegisterTransformHandler(ProcessTransform);

            // Get things rolling
            Trace.WriteLine("Starting Transformer");
            ProcessCommand("Start");
        }

        /// <summary>
        /// <c>Transform</c> is the main entry point for the Transformer.  It takes a <c>TransformerPayload</c> and returns the transformed data.
        //  The <c>TransformerPayload</c> contains the following:
        //  <list type="bullet">Type1 - The source data type</list>
        //  <list type="bullet">Type2 - The target data type</list>
        //  <list type="bullet">Format - The source data format</list>
        //  <list type="bullet">Version - The source data version</list>
        //  <list type="bullet">SourceHost - The source data host</list>
        //  <list type="bullet">data - The source data</list>
        /// </summary>
        /// <param name="payload">TransformerPayload class</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<object?> Transform(TransformerPayload payload)
        {
            try
            {
                // Get the detailed Create to match the payload parameters
                var transformer = typeof(TransformerFactory)
                                     .GetMethods().First(w => w.Name == "Create" && w.GetParameters().Count() > 2)
                                     .MakeGenericMethod(payload.Type1, payload.Type2)
                                     .Invoke(this, new object[] { tenantid, payload.Format, payload.Version, payload.SourceHost }) as ITransformer;

                if (transformer == null)
                {
                    throw new ArgumentNullException("transformer");
                }

                return await transformer.Transform(payload.data);
            }
            finally
            { }
        }

        // Transform Handler
        internal void ProcessTransform(TransformerPayload payload)
        {
            try
            {
                var result = Transform(payload);
                using (var bus = RabbitHutch.CreateBus(AppRunningIn.Docker ? "host=rabbitmq" : "host=localhost"))
                {
                    bus.PubSub.Publish(result, payloadbus);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }
        
        // Command Handler
        internal void ProcessCommand(string payload)
        {
            Debug.WriteLine($"Transformer Command Request: {payload}");

            switch (payload.ToUpperInvariant())
            {
                case "SHUTDOWN":
                    Dispose();
                    Environment.Exit(0);
                    break;

                case "PANIC":
                    Panic();
                    break;

                case "STOP":
                    Stop();
                    break;

                case "START":
                    Start();
                    break;

                default:
                    break;
            }
        }
    }
}