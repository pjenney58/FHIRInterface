/*
 MIT License - PatientAdapter.cs

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

/*
extern alias r5;
extern alias r4b;
extern alias r4;
extern alias stu3;

using Hl7.Fhir.Model;
using System;
using R5 = r5::Hl7.Fhir.Model;
using R4B = r4b::Hl7.Fhir.Model;
using R4 = r4::Hl7.Fhir.Model;
using Stu3 = stu3::Hl7.Fhir.Model;
*/

using DataShapes.Model;
using Hl7.Fhir.Model;
using Task = System.Threading.Tasks.Task;

namespace TransformerFactory.Model.R4
{
    public class PatientAdapter<IEntity, OEntity> : ITransformer<IEntity, OEntity>
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;

        public delegate OEntity VoidDelegate();
        public delegate Task<OEntity> TaskDelegate();

        public Hl7Version version { get; set; }
        public HL7Format format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public PatientAdapter(Guid tenant, HL7Format format, Hl7Version version, SourceSystems source)
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

        private async Task<OEntity> ConvertR4FhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Patient ?? throw new ArgumentNullException("fhir");
            var meta = new DataShapes.Model.Patient()
            {
                TenantId = tenant == Guid.Empty ? Constants.Transform : tenant,
                EntityId = Guid.Parse(fhir.Id),
                CreateDate = DateTimeOffset.Now,
                LastUpdate = DateTimeOffset.Now,
                PrimaryPatientIdString = fhir.Id,
                Version = 1
            };

            var nameconverter = TransformerFactory<Hl7.Fhir.Model.HumanName, DataShapes.Model.PersonName>.GetTransformer(tenant, version);
            foreach (var name in fhir.Name)
            {
                meta.Name = await nameconverter.Convert(name);
            }

            // Known addresses
            var addressAdapter = TransformerFactory<Hl7.Fhir.Model.Address, DataShapes.Model.Address>.GetTransformer(tenant, version);
            foreach (var address in fhir.Address)
            {
                meta.Addresses.Add(await addressAdapter.Convert(address));
            }

            // Practitioners ...
            var pr = TransformerFactory<Hl7.Fhir.Model.Practitioner, DataShapes.Model.Practitioner>.GetTransformer(tenant, version);
            foreach (var practioner in fhir.GeneralPractitioner)
            {
                PatientPractitioner pp = new();
                pp.Practitioner = new DataShapes.Model.Practitioner();
                pp.Relationship = PractitionerRelationship.Primary;
                meta.Practitioners.Add(pp);
            }

            // TODO: Possible Patient Locations - This may be managed in addresses var l =
            // AdapterFactory<Hl7.Fhir.Model.Location,DataShapes.Model.Location>.GetAdapterr(version);
            // foreach (var location in p.) { o.Practitioners.Add(l.Convert(location)); }

            // Identifiers
            if (fhir.Identifier != null && fhir.Identifier.Count > 0)
            {
                foreach (var id in fhir.Identifier)
                {
                    var idset = new DataShapes.Model.Identifier()
                    {
                        EntityId = Guid.NewGuid(),
                        Version = 1,
                        CreateDate = DateTimeOffset.Now,
                        LastUpdate = DateTimeOffset.Now,
                        IdType = id.Type?.Text,
                        IdValue = id.Value,
                        IdUse = id.Use?.ToString(),
                        IdSource = id.System
                    };

                    if (id.Period != null)
                    {
                        idset.StartDate = DateTime.Parse(id.Period.Start);
                        idset.StopDate = DateTime.Parse(id.Period.End);
                    }

                    meta.HL7Identifiers.Add(idset);
                }
            }

            meta.BirthDate = DateTime.Parse(fhir.BirthDate);

            if (fhir.Deceased is FhirDateTime fdt)
            {
                // TODO:  There seems too have been a breaking change in R4  thast forces a timezone param
                meta.DeceasedDate = fdt.ToDateTimeOffset(DateTimeOffset.Now.Offset);
                meta.IsDeceased = true;
            }

            return meta as OEntity;
        }

        private async Task<OEntity> ConvertR5FhirToMeta()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR5Fhir()
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

        private async Task<OEntity> ConvertMetaToR4Fhir()
        {
            // var p = payloadIN as DataShapes.Model.{Type}; var o = new Hl7.Fhir.Model.{Type}();
            throw new NotImplementedException();
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

        public async Task<OEntity> Convert(IEntity payload)
        {
            // Override this with the appropriate key conditions - replace MSG as desired. There may
            // be several similar messages required, e.g. SIU & SRM
            Dictionary<Tuple<string, Hl7Version>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Patient => DataShapes.Model.Patient", Hl7Version.Dstu2), ConvertR5FhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.Patient => Hl7.Fhir.Model.Patient", Hl7Version.Dstu2), ConvertMetaToR5Fhir },
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Patient => DataShapes.Model.Patient", Hl7Version.Stu3), ConvertR3FhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.Patient => Hl7.Fhir.Model.Patient", Hl7Version.Stu3), ConvertMetaToR3Fhir },
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Patient => DataShapes.Model.Patient", Hl7Version.R4), ConvertR4FhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.Patient => Hl7.Fhir.Model.Patient", Hl7Version.R4), ConvertMetaToR4Fhir },
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Patient => DataShapes.Model.Patient", Hl7Version.R5), ConvertR5FhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.Patient => Hl7.Fhir.Model.Patient", Hl7Version.R5), ConvertMetaToR5Fhir }
            };

            payloadIN = payload;

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