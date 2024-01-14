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
using Transformers.Interface;

namespace Transformers.Model.Stu3
{
    public class PatientAdapter<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;

        public delegate OEntity VoidDelegate();
        public delegate Task<OEntity> TaskDelegate();

        public InputVersion version { get; set; }
        public InputFormat format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public PatientAdapter(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
        {
            this.tenant = tenant;
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private async Task<OEntity?> ConvertFhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Patient;
            var meta = new DataShapes.Model.Patient()
            {
                TenantId = tenant == Guid.Empty ? Constants.Transform : tenant,
                EntityId = Guid.Parse(fhir.Id),
                CreateDate = DateTimeOffset.Now,
                LastUpdate = DateTimeOffset.Now,
                PrimaryPatientIdString = fhir.Id
            };

            var nameconverter = TransformerFactory.Create<Hl7.Fhir.Model.HumanName, DataShapes.Model.PersonName>(tenant, format, version, source);
            foreach (var name in fhir.Name)
            {
                meta.Name = await nameconverter.Transform(name) as DataShapes.Model.PersonName;
            }

            // Known addresses
            var addressAdapter = TransformerFactory.Create<Hl7.Fhir.Model.Address, DataShapes.Model.Address>(tenant, format, version, source);
            foreach (var address in fhir.Address)
            {
                meta.Addresses.Add(await addressAdapter.Transform(address) as DataShapes.Model.Address);
            }

            // Practitioners ...
            var pr = TransformerFactory.Create<Hl7.Fhir.Model.Practitioner, DataShapes.Model.Practitioner>(tenant, format, version, source);
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

        private async Task<OEntity?> ConvertMetaToFhir()
        {
            // var p = payloadIN as DataShapes.Model.{Type}; var o = new Hl7.Fhir.Model.{Type}();
            throw new NotImplementedException();
        }

        public async Task<object?> Transform(object payload)
        {
            // Override this with the appropriate key conditions - replace MSG as desired. There may
            // be several similar messages required, e.g. SIU & SRM
            Dictionary<Tuple<string, InputVersion>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.Patient => DataShapes.Model.Patient", InputVersion.HL7FhirR4), ConvertFhirToMeta },
                { new Tuple<string, InputVersion>(@"DataShapes.Model.Patient => Hl7.Fhir.Model.Patient", InputVersion.HL7FhirR4), ConvertMetaToFhir }
            };

            payloadIN = payload as IEntity;

            var jumpkey = new Tuple<string, InputVersion>($"{typeof(IEntity).FullName} => {typeof(OEntity).FullName}", version);
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