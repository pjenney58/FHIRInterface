using Collectors.Interface;
using Collectors.Model;
using Collectors.CollectorFactory;

namespace CollectorTests
{
    public class FactoryTests
    {
       [Fact]
    		public void LoadCollector()
    		{
    			try
    			{
                    var fhir2 = CollectorFactory.Create("Fhir2");
    				Assert.NotNull(fhir2);

                    var fhir3 = CollectorFactory.Create("Fhir3");
    				Assert.NotNull(fhir3);

    				var fhir4 = CollectorFactory.Create("Fhir4");
    				Assert.NotNull(fhir4);

                    var fhir4b = CollectorFactory.Create("Fhir4b");
    				Assert.NotNull(fhir4b);

                    var fhir5 = CollectorFactory.Create("Fhir5");
    				Assert.NotNull(fhir5);
    			}
    			catch(Exception ex)
    			{
    				Assert.Fail(ex.Message);
    			}
    		}
    }
}
