/*
 MIT License - DoseScheduleAdapter.cs

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

namespace Transformers.Model.R4
{
    public class DoseScheduleTransformer<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;

        public delegate OEntity VoidDelegate();

        public delegate Task<OEntity?> TaskDelegate();

        public InputVersion version { get; set; }
        public InputFormat format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public DoseScheduleTransformer(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
        {
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private async Task<OEntity?> ConvertFhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Dosage;
            if (fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }

            var meta = new PalisaidMeta.Model.DoseSchedule();
            if (meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            meta.TenantId = tenant;
            meta.EntityId = fhir.ElementId ?? Guid.NewGuid().ToString();
            meta.DoseScheduleName = fhir.Text;

            await Task.Run(() =>
            {
                ;
            });

            return meta as OEntity;
        }

        private async Task<OEntity?> ConvertMetaToFhir()
        {
            var meta = payloadIN as PalisaidMeta.Model.DoseSchedule;
            if (meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            var fhir = new Hl7.Fhir.Model.Dosage();
            if (fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }

            fhir.ElementId = meta.EntityId;
            fhir.Text = meta.DoseScheduleName;

            // TODO: Add more fields

            await Task.Run(() =>
            {
            });

            return fhir as OEntity;
        }

        public async Task<object?> Transform(object payload)
        {
            payloadIN = payload as IEntity;

            // Use full names to differenciate
            Dictionary<Tuple<string, InputVersion>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.DoseSchedule => PalisaidMeta.Model.DoseSchedule", InputVersion.HL7FhirR4), ConvertFhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model.DoseSchedule => Hl7.Fhir.Model.DoseSchedule", InputVersion.HL7FhirR4), ConvertMetaToFhir }
            };

            var jumpkey = new Tuple<string, InputVersion>($"{typeof(IEntity).FullName} => {typeof(OEntity).FullName}", version);
            if (jumpTable.TryGetValue(jumpkey, out TaskDelegate? funcC))
            {
                return await funcC();
            }

            return default;
        }
    }
}