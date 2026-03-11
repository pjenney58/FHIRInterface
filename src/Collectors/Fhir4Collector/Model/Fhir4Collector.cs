using Collectors.Interface;
using PalisaidMeta.Model;

namespace Collectors.Model
{
    public class Fhir4Collector : ICollector
    {
        public Fhir4Collector()
        { }

        public Task Configure()
        {
            throw new NotImplementedException();
        }

        public Task Connect()
        {
            throw new NotImplementedException();
        }

        public Task Deploy()
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

        public Task Panic()
        {
            throw new NotImplementedException();
        }

        public Task Persist()
        {
            throw new NotImplementedException();
        }

        public Task RegisterCollector()
        {
            throw new NotImplementedException();
        }

        public Task RegisterScheduler()
        {
            throw new NotImplementedException();
        }

        public Task RegisterTransformer(DataProtocol dataProtocolIn)
        {
            throw new NotImplementedException();
        }

        public Task RegisterTransporter()
        {
            throw new NotImplementedException();
        }

        public Task<string> Retrieve()
        {
            throw new NotImplementedException();
        }
    }
}