using Confluent.Kafka;
using System.Security.Cryptography;
using System.Text;
using Transporters.Interface;

namespace Transporters.Model
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