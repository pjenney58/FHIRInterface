/*
 MIT License - NameConverter.cs

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
    public class NameAdapter<IEntity, OEntity> : ITransformer
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

        public NameAdapter(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
        {
            this.tenant = tenant;
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private async Task<OEntity?> ConvertFhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.HumanName;
            var meta = new PalisaidMeta.Model.PersonName();

            meta.EntityId = fhir.ElementId == null
                ? Guid.NewGuid()
                : Guid.Parse(fhir.ElementId);

            meta.TenantId = tenant;

            foreach (var given in fhir.Given)
            {
                meta.GivenName.Add(given);
            }

            meta.FamilyName = fhir.Family;

            foreach (var prefix in fhir.Prefix)
            {
                meta.Prefix.Add(prefix);
            }

            foreach (var suffix in fhir.Suffix)
            {
                meta.Suffix.Add(suffix);
            }

            if (fhir.Period != null)
            {
                if (!string.IsNullOrEmpty(fhir.Period.Start))
                {
                    meta.StartDate = DateTime.Parse(fhir.Period.Start);
                }

                if (!string.IsNullOrEmpty(fhir.Period.End))
                {
                    meta.StopDate = DateTime.Parse(fhir.Period.End);
                }
            }

            return meta as OEntity;
        }

        private async Task<OEntity?> ConvertMetaToFhir()
        {
            var meta = payloadIN as PalisaidMeta.Model.PersonName;
            if (meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            var fhir = new Hl7.Fhir.Model.HumanName();
            if(fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }

            fhir.ElementId = meta.EntityId.ToString();

            foreach (var given in meta.GivenName)
            {
                fhir.Given.Append(given);
            }

            fhir.Family = meta.FamilyName;

            foreach (var prefix in meta.Prefix)
            {
                fhir.Prefix.ToList().Add(prefix);
            }

            foreach (var suffix in meta.Suffix)
            {
                fhir.Suffix.Append(suffix);
            }

            if (meta != null)
            {
                if (fhir.Period == null)
                {
                  //  fhir.Period = new Duration(); 
                }

                fhir.Period.Start = meta.StartDate.ToString("yyyyMMdd");
                fhir.Period.End = meta.StopDate.ToString("yyyyMMdd");
            }

            return fhir as OEntity;
        }

        private async Task<OEntity> ConvertMetaToR5Fhir()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertV2_MSG_ToMeta()
        {
            // var meta = new PalisaidMeta.Model.{Type}(); var message = payloadIN as NHapi.Model.{Version}.Message.{MSG};

            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToV2_MSG()
        {
            // var meta = new PalisaidMeta.Model.{Type}(); var message = payloadIN as NHapi.Model.{Version}.Message.{MSG};
            throw new NotImplementedException();
        }

        public async Task<object?> Transform(object payload)
        {
            // Override this with the appropriate key conditions - replace MSG as desired. There may
            // be several similar messages required, e.g. SIU & SRM

            payloadIN = payload as IEntity;

            Dictionary<Tuple<string, InputVersion>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.HumanName => PalisaidMeta.Model.PersonName", InputVersion.HL7FhirR4), ConvertFhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model.PersonName => Hl7.Fhir.Model.HumanName", InputVersion.HL7FhirR4), ConvertMetaToFhir }
            };

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