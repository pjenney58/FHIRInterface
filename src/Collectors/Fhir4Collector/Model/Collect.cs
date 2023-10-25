using Collector.Interface;

namespace Collectors.Fhir4Collecter
{

    public class Fhir4Collector : ICollector
    {
        Guid tenantid;

        public Fhir4Collector(Guid tenantid)   
        {
            this.tenantid = tenantid;
        }

        public System.Threading.Tasks.Task Configure()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task Connect()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task Deploy()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task Destroy()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string> Retrieve()
        {
            throw new NotImplementedException();
        }
    }
}