
using Collectors.Interface;
using Collectors.Model;

namespace Collectors.CollectorFactory
{
    public static class CollectorFactory 
    {
        public static ICollector Create(string collectorType)
        {
            switch (collectorType.ToLower())
            {
                case "fhir2":
                    return new Fhir2Collector();

                case "fhir3":
                    return new Fhir3Collector();

                case "fhir4":
                    return new Fhir4Collector();

                case "fhir4b":
                    return new Fhir4bCollector();

                case "fhir5":
                    return new Fhir5Collector();

                case "hl7v2":
                    return new HL7v2Collector();

                case "hl7v3":
                    return new HL7v3Collector();

                case "hl7cda":
                    return new HL7CDACollector();

                case "x12":
                    return new X12Collector();

                default:
                    return default;
            }
        }
        
    }
}

