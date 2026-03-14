using PalisaidMeta.Model;
using ChainOfResponsibility.Interface;
using Collector.Interface;

namespace ChainOfResponsibility.Model
{
    public class Fhir4bHandler : AbstractHandler, IChainOfResponsabilityHandler
    {
        public Fhir4bHandler(InputVersion version, Guid tenantId)
            : base(version, tenantId)
		{}

        public ICollector HandleRequest(InputVersion version)
        {
            throw new NotImplementedException();
        }

        public IChainOfResponsabilityHandler Next(IChainOfResponsabilityHandler handler)
        {
            throw new NotImplementedException();
        }
    

    }	
}