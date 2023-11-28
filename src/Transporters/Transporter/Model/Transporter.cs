using Confluent.Kafka;
using Transporter.Interface;

namespace Transporter.Model
{
    public abstract class Transporter : IDisposable
    {
        protected ConsumerConfig kcconfig = new()
        {
            GroupId = "Transporters",
            BootstrapServers = "palisaid:9002",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        // Message producer, data to controller
        protected ProducerConfig kpconfig = new()
        {
            BootstrapServers = "palisaid:9002"
        };

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