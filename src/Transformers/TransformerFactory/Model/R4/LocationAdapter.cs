/*
 MIT License - LocationAdapter.cs

Copyright (c) 2021 - Present by Sand Drift Software, LLC
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using Transformers.Interface;

namespace Transformers.Model.R4
{
    public class LocationAdapter<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;
        private OEntity? payloadOUT;

        public delegate OEntity VoidDelegate();
        public delegate Task<OEntity> TaskDelegate();

        public Hl7Version version { get; set; }
        public HL7Format format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public LocationAdapter(Guid tenant, HL7Format format, Hl7Version version, SourceSystems source)
        {
            this.tenant = tenant;
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private async Task<OEntity> ConvertR2FhirToMeta()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertR3FhirToMeta()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertFhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Location;
            var meta = new DataShapes.Model.Location()
            {
                TenantId = tenant == Guid.Empty ? Constants.Transform : tenant,
                EntityId = Guid.Parse(fhir.Id),
                CreateDate = DateTimeOffset.Now,
                LastUpdate = DateTimeOffset.Now,
            };

            meta.Name = fhir.Name;
            meta.Description = fhir.Description;

            if (fhir.ManagingOrganization != null)
            {
                meta.OwnerId = Guid.Parse(fhir.ManagingOrganization.Identifier.Value);
            }

            // Known addresses
            var addressAdapter = TransformerFactory.Create<Hl7.Fhir.Model.Address, DataShapes.Model.Address>(tenant, format, version, source);

            var address = await addressAdapter.Transform(fhir.Address);
            if (address != null)
            {
                meta.Addresses.Add(await addressAdapter.Transform(fhir.Address) as Address);
            }

            return meta as OEntity;
        }

        private async Task<OEntity> ConvertR5FhirToMeta()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR2Fhir()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR3Fhir()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToFhir()
        {
            // var p = payloadIN as DataShapes.Model.{Type}; var o = new Hl7.Fhir.Model.{Type}();
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR5Fhir()
        {
            throw new NotImplementedException();
        }

        public LocationAdapter(Hl7Version version)
        {
            this.version = version;
        }

        private async Task<OEntity> ConvertV2_MSG_ToMeta()
        {
            // var meta = new DataShapes.Model.{Type}(); var message = payloadIN as NHapi.Model.{Version}.Message.{MSG};

            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToV2_MSG()
        {
            // var meta = new DataShapes.Model.{Type}(); var message = payloadIN as NHapi.Model.{Version}.Message.{MSG};
            throw new NotImplementedException();
        }

        public async Task<object?> Transform(object payload)
        {
            // Override this with the appropriate key conditions - replace MSG as desired. There may
            // be several similar messages required, e.g. SIU & SRM Override this with the
            // appropriate key conditions - replace MSG as desired. There may be several similar
            // messages required, e.g. SIU & SRM
            Dictionary<Tuple<string, Hl7Version>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Location => DataShapes.Model.Location", Hl7Version.Dstu2), ConvertR5FhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.Location => Hl7.Fhir.Model.Location", Hl7Version.Dstu2), ConvertMetaToR5Fhir },
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Location => DataShapes.Model.Location", Hl7Version.Stu3), ConvertR3FhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.Location => Hl7.Fhir.Model.Location", Hl7Version.Stu3), ConvertMetaToR3Fhir },
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Location => DataShapes.Model.Location", Hl7Version.R4), ConvertFhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.Location => Hl7.Fhir.Model.Location", Hl7Version.R4), ConvertMetaToFhir },
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Location => DataShapes.Model.Location", Hl7Version.R5), ConvertR5FhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.Location => Hl7.Fhir.Model.Location", Hl7Version.R5), ConvertMetaToR5Fhir }
            };

            payloadIN = payload as IEntity;

            var jumpkey = new Tuple<string, Hl7Version>($"{typeof(IEntity).FullName} => {typeof(OEntity).FullName}", version);
            if (jumpTable.TryGetValue(jumpkey, out TaskDelegate? funcC))
            {
                return await funcC();
            }

            return default;
        }

        public IEnumerable<OEntity> CollectOEntityItemListItem()
        {
            throw new NotImplementedException();
        }
    }
}