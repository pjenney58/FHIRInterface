using DataShapes.Model;
using ChainOfResponsibility.Interface;

namespace ChainOfResponsibility.Model
{
	public class Fhir4bHandler : IHandler
	{
		public Fhir4bHandler(Hl7Version version, Guid tenantId)
            : base(version, tenantId);
		{
            
		}
	}
}