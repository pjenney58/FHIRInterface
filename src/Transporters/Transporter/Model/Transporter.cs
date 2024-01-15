
using Confluent.Kafka;
using PalisaidMeta.Model;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using NLog.Common;
using Support.Model;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Transporters.Interface;

namespace Transporters.Model
{
    
    delegate void TransporterConnectionHandler();

    public class Transporter : PalisaidMessageQueue, IDisposable
    {     
        protected readonly IConfiguration? config;
        protected readonly CollectorConfig? cconfig;
        TransporterConnectionHandler? client;

        public Transporter(CollectorConfig cconfig, Guid tenantid, string commandbus, string payloadbus) 
            : base(tenantid, commandbus, payloadbus) 
        {
            config = AppConfig.Get("transportersettings.json");
            
            this.cconfig = cconfig;

            // Register ConnectionHandler
            Trace.WriteLine("Registering Transporter Connection Handler");

            // Register Command Handler
            Trace.WriteLine("Registering Transporter Command Handler");
            RegisterCommmandHandler(ProcessCommand);

            // Register Trasform Handler
            Trace.WriteLine("Registering Transporter Payload Handler");
            RegisterTransformHandler(ProcessReadData);

            // Get things rolling
            Trace.WriteLine($"Starting Transporter {cconfig.NetworkProtocolIn}");
            ProcessCommand("Start");
        }

        internal void ProcessConnectionHandler()
        {
            try
            {
                switch(cconfig.NetworkProtocolIn)
                {
                    case NetworkProtocol.TCP:                  
                        Connect();
                        break;

                    case NetworkProtocol.HTTPS:
                        Connect();
                        break;

                    case NetworkProtocol.HTTP:
                        Connect();
                        break;

                    case NetworkProtocol.REST:
                        Connect();
                        break;

                    case NetworkProtocol.WebSocket:
                        Connect();
                        break;

                    case NetworkProtocol.SignalR:
                        Connect();
                        break;

                    case NetworkProtocol.GRPC:
                        Connect();
                        break;
                       
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        internal void ProcessReadData(TransformerPayload payload)
        {
            try
            {
                var result = Read();

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

        protected string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = SHA256.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public virtual Task Authenticate()
        {
            throw new NotImplementedException();
        }

        public virtual Task Cancel(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task Connect()
        {
            throw new NotImplementedException();
        }

        public virtual Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public virtual Task Write(string message)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<string> Read()
        {
            throw new NotImplementedException();
        }
    }
}