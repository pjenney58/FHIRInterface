/*
 MIT License - ObservationAdapter.cs

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
    public class ObservationAdapter<IEntity, OEntity> : ITransformer
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

        public ObservationAdapter(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
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
            var fhir = payloadIN as Hl7.Fhir.Model.Observation;
            var meta = new PalisaidMeta.Model.Observation();

            meta.EntityId = fhir.Id ?? Guid.NewGuid().ToString();

            // Find all the list entries and add their contents as OservationItems
            //Type _observation = typeof(Hl7.Fhir.Model.Observation);
            //PropertyInfo[] _properties = _observation.GetProperties();
            //
            //foreach(var prop in _properties)
            //{
            //    var t = prop.GetType();
            //}

            string Text = "";

            if (fhir.Value != null && fhir.Value.EnumerateElements().Count() > 0)
            {
                var items = fhir.Value.EnumerateElements().ToList();
                foreach (var val in items)
                {
                    if (val.Value != null)
                    {
                        Text += $"{val.Key}:{val.Value.ToString()} ";
                    }
                }
            }

            foreach (var code in fhir.Code.Coding)
            {
                var obi = new ObservationItem()
                {
                    //Code = code.CodeElement.value,
                    Description = code.Display,
                    Value = Text,
                    Timestamp = DateTime.Now
                };

                meta.Items.Add(obi);
            }

            if (fhir.Effective != null)
            {
                var items = fhir.Effective.EnumerateElements().ToList();
                foreach (var e in items)
                {
                    var foo = new KeyValuePair<string, object>(e.Key, e.Value);
                }
            }

            if (fhir.Encounter != null)
            {
                var items = fhir.Encounter.EnumerateElements().ToList();
                // These might map to events
                foreach (var e in items)
                {
                    var bar = new KeyValuePair<string, object>(e.Key, e.Value);
                }
            }

            if (fhir.Subject != null)
            {
                var subject = fhir.Subject.ToString();
                
                // handle prefix: urn:uuid:
                var str = subject.Contains("urn")
                    ? subject.Substring(9)
                    : subject;

                meta.OwnerId = Guid.Parse(str);
            }

            if (fhir.Performer != null && fhir.Performer.Count > 0)
            {
                var performer = fhir.Performer.First().ToString();
                // handle prefix: urn:uuid:
                var str = performer.Contains("urn")
                    ? performer.Substring(9)
                    : performer;

                meta.PractitionerId = Guid.Parse(str);
            }

            if (fhir.Note != null)
            {
                foreach (var n in fhir.Note)
                {
                    foreach (var n2 in n)
                    {
                        var obi = new ObservationItem()
                        {
                            //Code = n2.Key,
                            Value = n2.Value.ToString(),
                            ObservationType = PalisaidMeta.Model.ObservationType.Note
                        };

                        meta.Items.Add(obi);
                    }
                }
            }

            // Id - Item Reference? Subject - Patient? BasedOn

            // Effective.value - Observation DateTime

            // Encounter Note Performer value

            // Status

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
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR5Fhir()
        {
            throw new NotImplementedException();
        }

        public ObservationAdapter(InputVersion version)
        {
            this.version = version;
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
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.Observation => PalisaidMeta.Model.Observation", InputVersion.HL7FhirR4), ConvertFhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model.Observation => Hl7.Fhir.Model.Observation", InputVersion.HL7FhirR4), ConvertMetaToFhir }
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