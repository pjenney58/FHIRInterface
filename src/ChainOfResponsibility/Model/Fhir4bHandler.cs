using DataShapes.Model;
using ChainOfResponsibility.Interface;
using Collectors.Interface;

namespace ChainOfResponsibility.Model
{
    public class Fhir4bHandler : AbstractHandler, IChainOfResponsabilityHandler
    {
        public Fhir4bHandler(Hl7Version version, Guid tenantId)
            : base(version, tenantId);
		{}

        public ICollector HandleRequest(Hl7Version version)
        {
            throw new NotImplementedException();
        }

        public IChainOfResponsabilityHandler Next(IChainOfResponsabilityHandler handler)
        {
            throw new NotImplementedException();
        }
    

    }	
}