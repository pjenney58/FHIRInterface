using PalisaidMeta.Model;

namespace ChainOfResponsibility.Model
{
	public class Fhir5Handler : Handler
	{
		public Fhir5Handler(InputVersion version, Guid tenantId)
            : base(version, tenantId)
		{
            
		}
	}
}