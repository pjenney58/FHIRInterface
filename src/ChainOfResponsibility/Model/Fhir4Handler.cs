using ChainOfResponsibility.Interface;
using Collector.Interface;
using Collector.CollectorFactory;
using DataShapes.Model;

namespace ChainOfResponsibility.Model
{
    public class Fhir4Handler : IChainOfResponsabilityHandler
    {
        internal readonly Guid _tenantid;
        internal readonly InputVersion _version;
        public IChainOfResponsabilityHandler _next { get; }

        public Fhir4Handler(IChainOfResponsabilityHandler next, Guid tenantId)
        {
            _next = next;
            _tenantid = tenantId;
            _version = InputVersion.HL7FhirR4;
        }

        public void HandleRequest(InputVersion version)
        {
            
        }

        public IChainOfResponsabilityHandler Next(IChainOfResponsabilityHandler handler)
        {
            _next.HandleRequest();
        }

        ICollector IChainOfResponsabilityHandler.HandleRequest(InputVersion version)
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