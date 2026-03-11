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

namespace Transformers.Model.Stu3
{
    public class LocationAdapter<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;
        private OEntity? payloadOUT;

        public delegate OEntity VoidDelegate();

        public delegate Task<OEntity> TaskDelegate();

        public InputVersion version { get; set; }
        public InputFormat format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public LocationAdapter(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
        {
            this.tenant = tenant;
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private async Task<OEntity?> ConvertFhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Location;
            var meta = new PalisaidMeta.Model.Location()
            {
                TenantId = tenant == Guid.Empty ? Constants.Transform : tenant,
                EntityId = fhir.Id ?? Guid.NewGuid().ToString(),
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
            var addressAdapter = TransformerFactory.Create<Hl7.Fhir.Model.Address, PalisaidMeta.Model.Address>(tenant, format, version, source);

            var address = await addressAdapter.Transform(fhir.Address);
            if (address != null)
            {
                meta.Addresses.Add(await addressAdapter.Transform(fhir.Address) as Address);
            }

            return meta as OEntity;
        }

        private async Task<OEntity> ConvertMetaToFhir()
        {
            await Task.Run(() =>
            {
            });

            // var p = payloadIN as PalisaidMeta.Model.{Type}; var o = new Hl7.Fhir.Model.{Type}();
            throw new NotImplementedException();
        }

        public async Task<object?> Transform(object payload)
        {
            // Override this with the appropriate key conditions - replace MSG as desired. There may
            // be several similar messages required, e.g. SIU & SRM Override this with the
            // appropriate key conditions - replace MSG as desired. There may be several similar
            // messages required, e.g. SIU & SRM
            Dictionary<Tuple<string, InputVersion>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.Location => PalisaidMeta.Model.Location", InputVersion.HL7HhirStu3), ConvertFhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model.Location => Hl7.Fhir.Model.Location", InputVersion.HL7HhirStu3), ConvertMetaToFhir }
            };

            payloadIN = payload as IEntity;

            var jumpkey = new Tuple<string, InputVersion>($"{typeof(IEntity).FullName} => {typeof(OEntity).FullName}", version);
            if (jumpTable.TryGetValue(jumpkey, out TaskDelegate? funcC))
            {
                return await funcC();
            }

            return default;
        }
    }
}