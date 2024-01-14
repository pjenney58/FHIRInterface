using System.Net;
using Transformers.Model;
using Transformers.Interface;
using Transporters.Model;
using DataShapes.Model;
using Support.Model;

namespace DevTests.Transporter
{
    public class TTransformer
    {
        public TTransformer()
        {
        }

        [Fact]
        public async Task CheckTransforms()
        {
            var tenantid = Guid.NewGuid();

            var fhiraddress = new Hl7.Fhir.Model.Address()
            {
                ElementId = Guid.NewGuid().ToString(),
                Line = new List<string>() { "123 Main Street" },
                City = "Boston",
                State = "MA",
            };          

            var transformer = TransformerFactory.Create<Hl7.Fhir.Model.Address, DataShapes.Model.Address>(tenantid, InputFormat.HL7Fhir, InputVersion.HL7FhirR4, SourceSystems.Epic);
            var meta = await transformer.Transform(fhiraddress);
            Assert.NotNull(meta);

            TransformerPayload payload = new TransformerPayload()
            {
                Type1 = typeof(Hl7.Fhir.Model.Address),
                Type2 = typeof(DataShapes.Model.Address),
                Format = InputFormat.HL7Fhir,
                Version = InputVersion.HL7FhirR4,
                SourceHost = SourceSystems.Epic,
                data = fhiraddress,
            };

            var transform = new Transformer(tenantid, "commandqueue", "payloadqueue");
            var result = await transform.Transform(payload) as Address;
            Assert.NotNull(result);
        }
    }
}