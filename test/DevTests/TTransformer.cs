using System.Net;
using Transformers.Model;
using Transformers.Interface;
using Transporters.Model;
using DataShapes.Model;

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

            var transformer = TransformerFactory.Create<Hl7.Fhir.Model.Address, DataShapes.Model.Address>(tenantid, HL7Format.Fhir, Hl7Version.R4, SourceSystems.Epic);
            var meta = await transformer.Transform(fhiraddress);
            Assert.NotNull(meta);

            TransformerPayload payload = new TransformerPayload()
            {
                Type1 = typeof(Hl7.Fhir.Model.Address),
                Type2 = typeof(DataShapes.Model.Address),
                Format = HL7Format.Fhir,
                Version = Hl7Version.R4,
                SourceHost = SourceSystems.Epic,
                data = fhiraddress,
            };

            var transform = new Transformer(tenantid);
            var result = await transform.Transform(payload) as Address;
            Assert.NotNull(result);
        }
    }
}