using Collectors;
using Collector.Interface;
using Collector.Model;

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

                var hl7v2 = CollectorFactory.Create("HL7v2");
                Assert.NotNull(hl7v2);

                var hl7v3 = CollectorFactory.Create("HL7v3");
                Assert.NotNull(hl7v3);

                var hl7CDA = CollectorFactory.Create("HL7CDA");
                Assert.NotNull(hl7CDA);

                var x12 = CollectorFactory.Create("X12");
                Assert.NotNull(x12);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}