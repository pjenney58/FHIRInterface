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

using DataShapes.Model;
using Hl7.Fhir.Validation;

namespace TransformerFactory.Model.Dstu2
{
    public class DoseScheduleAdapter<IEntity, OEntity> : ITransformer<IEntity, OEntity>
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity payloadIN;

        public delegate OEntity VoidDelegate();

        public delegate Task<OEntity> TaskDelegate();

        public Hl7Version version { get; set; }
        public HL7Format format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public DoseScheduleAdapter(Guid tenant, HL7Format format, Hl7Version version, SourceSystems source)
        {
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
            var fhir = payloadIN as Hl7.Fhir.Model.Dosage;
            var meta = new DataShapes.Model.DoseSchedule();

            meta.TenantId = tenant;
            meta.EntityId = Guid.Parse(fhir.ElementId);
            meta.DoseScheduleName = fhir.Text;

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

        private async Task<OEntity> ConvertMetaToR4Fhir()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR5Fhir()
        {
            throw new NotImplementedException();
        }

        public DoseScheduleAdapter(Hl7Version version)
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

        public async Task<OEntity> Convert(IEntity payload)
        {
            // Override this with the appropriate key conditions - replace MSG as desired. There may
            // be several similar messages required, e.g. SIU & SRM
            Dictionary<string, TaskDelegate> jumpTable = new Dictionary<string, TaskDelegate>()
            {
                { @"Hl7.Fhir.Model.Dosage/Hl7Harmonizer.Metatypes.Model.DoseSchdule", ConvertR4FhirToMeta },
                { @"Hl7Harmonizer.Metatypes.Model.DoseSchdule/Hl7.Fhir.Model.Dosage", ConvertMetaToR4Fhir}
            };

            payloadIN = payload;

            var jumpkey = $"{typeof(IEntity).FullName}/{typeof(OEntity).FullName}";
            if (jumpTable.TryGetValue(jumpkey, out TaskDelegate funcC))
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