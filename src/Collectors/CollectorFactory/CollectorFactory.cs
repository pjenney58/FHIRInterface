
using Collectors.Interface;
using Collectors.Model;

namespace Collectors.CollectorFactory
{
    public static class CollectorFactory 
    {
        public static ICollector Create(string collectorType)
        {
            switch (collectorType)
            {
                case "Fhir2":
                    return new Fhir2Collector();

                case "Fhir3":
                    return new Fhir3Collector();

                case "Fhir4":
                    return new Fhir4Collector();

                case "Fhir4b":
                    return new Fhir4bCollector();

                case "Fhir5":
                    return new Fhir5Collector();

                default:
                    return default;
            }
        }
        
    }
}

