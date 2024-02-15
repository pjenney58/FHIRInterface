/*
 MIT License - ObservationItemAdapter.cs

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

using Task = System.Threading.Tasks.Task;
using Transformers.Interface;

namespace Transformers.Model.Stu3
{
    public class ObservationItemAdapter<IEntity, OEntity> : ITransformer
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

        public ObservationItemAdapter(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
        {
            this.tenant = tenant;
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private async Task<OEntity?> ConvertFhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Observation;
            if(fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }

            var meta = new PalisaidMeta.Model.ObservationItem();
            if(meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            meta.TenantId = this.tenant;
            meta.EntityId = fhir.Id ?? Guid.NewGuid().ToString();

            await Task.Run(() =>
            {
                meta.ObservationType = PalisaidMeta.Model.ObservationType.Visual;

                if (fhir.Value != null)
                {
                    foreach (var val in fhir.Value)
                    {
                        if (val.Value != null)
                        {
                            meta.Value += $"{val.Key}:{val.Value.ToString()} ";
                        }
                    }
                }
            });

            return meta as OEntity;
        }

        private async Task<OEntity?> ConvertMetaToFhir()
        {
            var meta = payloadIN as PalisaidMeta.Model.ObservationItem; 
            if(meta == null || meta.Value == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            var fhir = new Hl7.Fhir.Model.Observation();
            if(fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }

            fhir.Id = meta.EntityId.ToString();

            await Task.Run(() =>
            {
                // Split into a collection of Key/value pairs
                var items = meta.Value.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in items)
                {
                    // Split encodeed key/value pair
                    var i = item.Split(':');
                    fhir.Value.AddAnnotation(new KeyValuePair<object, object>(i[0], i[1]));
                }
            });

            return fhir as OEntity;
        }

        public async Task<object?> Transform(object payload)
        {
            // Override this with the appropriate key conditions - replace MSG as desired. There may
            // be several similar messages required, e.g. SIU & SRM
            payloadIN = payload as IEntity;
           
            Dictionary<Tuple<string, InputVersion>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.ObservationItem => PalisaidMeta.Model.ObservationItem", InputVersion.HL7FhirR4), ConvertFhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model.ObservationItem => Hl7.Fhir.Model.ObservationItem", InputVersion.HL7FhirR4), ConvertMetaToFhir }
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