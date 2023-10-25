using ChainOfResponsibility.Interface;
using Collector.Interface;
using Collector.CollectorFactory;
using DataShapes.Model;

namespace ChainOfResponsibility.Model
{
    public class Fhir4Handler : IChainOfResponsabilityHandler
    {
        internal readonly Guid _tenantid;
        internal readonly Hl7Version _version;
        public IChainOfResponsabilityHandler _next { get; }

        public Fhir4Handler(IChainOfResponsabilityHandler next, Guid tenantId)
        {
            _next = next;
            _tenantid = tenantId;
            _version = Hl7Version.R4;
        }

        public void HandleRequest(Hl7Version version)
        {
            
        }

        public IChainOfResponsabilityHandler Next(IChainOfResponsabilityHandler handler)
        {
            _next.HandleRequest();
        }

        ICollector IChainOfResponsabilityHandler.HandleRequest(Hl7Version version)
        {
            if (version == _version)
            {
                return CollectorFactory.Get(typeof(Fhir4Collector), _tenantid);
            }
            else
            {
                Next(_next);
            }
        }
    }
}