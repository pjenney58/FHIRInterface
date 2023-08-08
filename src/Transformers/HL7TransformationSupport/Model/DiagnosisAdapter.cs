/*
 MIT License - DiagnosisAdapter.cs

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

using System;
using Hl7.Fhir.Model;
using DataShapes.Model;

namespace Hl7Harmonizer.Adapters.Model
{
    /// <summary>
    /// Allows a basic Fluent statement or query object to be passed to a QRD
    /// </summary>
    /// <typeparam name="IEntity"> </typeparam>
    /// <typeparam name="OEntity"> </typeparam>
    public class DiagnosisAdapter<IEntity, OEntity> : IAdapter<IEntity, OEntity>
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

        public DiagnosisAdapter(Guid tenant, HL7Format format, Hl7Version version, SourceSystems source)
        {
            this.tenant = tenant;
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private async Task<OEntity> ConvertR2FhirToMeta()
        {
            // var fhir = payloadIN as Hl7.Fhir.Model.{Type}; var meta = new DataShapes.Model.{Type}();
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertR3FhirToMeta()
        {
            // var fhir = payloadIN as Hl7.Fhir.Model.{Type}; var meta = new DataShapes.Model.{Type}();
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertR4FhirToMeta()
        {
            // var fhir = payloadIN as Hl7.Fhir.Model.{Type}; var meta = new DataShapes.Model.{Type}();
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertR5FhirToMeta()
        {
            // var fhir = payloadIN as Hl7.Fhir.Model.{Type}; var meta = new DataShapes.Model.{Type}();
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR2Fhir()
        {
            // var fhir = payloadIN as Hl7.Fhir.Model.{Type}; var meta = new DataShapes.Model.{Type}();
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR3Fhir()
        {
            // var fhir = payloadIN as Hl7.Fhir.Model.{Type}; var meta = new DataShapes.Model.{Type}();
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR4Fhir()
        {
            // var fhir = payloadIN as Hl7.Fhir.Model.{Type}; var meta = new DataShapes.Model.{Type}();
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR5Fhir()
        {
            // var fhir = payloadIN as Hl7.Fhir.Model.{Type}; var meta = new DataShapes.Model.{Type}();
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
            payloadIN = payload;

            Dictionary<Tuple<string, Hl7Version>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Diagnosis => DataShapes.Model.Diagnosis", Hl7Version.R4), ConvertR4FhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.Diagnosis => Hl7.Fhir.Model.Diagnosis", Hl7Version.R4), ConvertMetaToR4Fhir }
            };

            var jumpkey = new Tuple<string, Hl7Version>($"{typeof(IEntity).FullName} => {typeof(OEntity).FullName}", version);
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