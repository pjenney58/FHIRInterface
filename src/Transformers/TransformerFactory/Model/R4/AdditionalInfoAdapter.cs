/*
 MIT License - AdditionalInfoAdapter.cs

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

using TransformerFactory.Interface;

namespace TransformerFactory.Model.R4
{
    public class AdditionalInfoAdapter<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;
        private OEntity? payloadOUT;

        public delegate OEntity VoidDelegate();
        public delegate Task<OEntity?> TaskDelegate();

        public Hl7Version version { get; set; }
        public HL7Format format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public AdditionalInfoAdapter(Guid tenant, HL7Format format, Hl7Version version, SourceSystems source)
        {
            this.tenant = tenant;
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private async Task<OEntity?> ConvertFhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Extension;
            if(fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }

            var meta = new DataShapes.Model.AdditionalInfo();
            if(meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            await Task.Run(() =>
            {
            });


            return meta as OEntity;
        }

    
        private async Task<OEntity> ConvertMetaToFhir()
        {
            // var fhir = payloadIN as DataShapes.Model.{Type}; var meta = new Hl7.Fhir.Model.{Type}();
            await Task.Run(() =>
            {
            });

            throw new NotImplementedException();
        }

        public async Task<object?> Transform(object payload)
        {
            payloadIN = payload as IEntity;

            Dictionary<Tuple<string, Hl7Version>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Extension => DataShapes.Model.AdditionalInfo", Hl7Version.R4), ConvertFhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.AdditionalInfo => Hl7.Fhir.Model.Extension", Hl7Version.R4), ConvertMetaToFhir }
            };

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