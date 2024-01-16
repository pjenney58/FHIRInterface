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

using PalisaidMeta.Model;
using Transformers.Interface;

namespace Transformers.Model.R4
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

        CodingSystem GetCodingSystem(string system)
        {
            return system switch
            {
                "http://loinc.org" => CodingSystem.LOINC,
                "http://hl7.org/fhir/sid/icd-10" => CodingSystem.ICD10,
                "http://hl7.org/fhir/sid/icd-9" => CodingSystem.ICD9,
                "http://hl7.org/fhir/sid/icd-11" => CodingSystem.ICD11,
                "http://hl7.org/fhir/sid/atc" => CodingSystem.ATC,
                "http://snomed.info/sct" => CodingSystem.SNOMED,
                "http://hl7.org/fhir/sid/us-ssn" => CodingSystem.USCDI,
                "http://hl7.org/fhir/sid/ichi/icd-10" => CodingSystem.ICHI,
                "http://hl7.org/fhir/sid/icpm" => CodingSystem.ICPM,
                "http://hl7.org/fhir/sid/hcpcs" => CodingSystem.HCPCS,
                "http://hl7.org/fhir/sid/gtin" => CodingSystem.GTIN,
                "http://hl7.org/fhir/sid/ndc" => CodingSystem.NDC,
                "http://hl7.org/fhir/sid/din" => CodingSystem.DIN,
                "http://terminology.hl7.org/CodeSystem/v3-ActCode" => CodingSystem.v3_ActCode,
                _ => CodingSystem.Unknown
            };
        }

        private async Task<OEntity> ConvertMetaToFhir()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertFhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Observation;
            if (fhir == null)
            {
                throw new Exception("Invalid payload type");
            }

            var meta = new PalisaidMeta.Model.Observation();
            if (meta == null)
            {
                throw new Exception("Invalid meta type");
            }

            if(fhir.HasVersionId)
            {
                meta.Version = long.Parse(fhir.VersionId);
            }

            // Fhir doesn't necessarily use a uuid for the key, so we need to be able to set capture it as a long
            if(!string.IsNullOrEmpty(fhir.Id))
            {
                try
                {
                    meta.EntityId = Guid.Parse(fhir.Id);
                }
                catch
                {
                    meta.EntityId = Guid.NewGuid();
                    meta.EntityKey = long.Parse(fhir.Id);
                }
            }

            if (fhir.Category != null && fhir.Category.Any())
            {
                foreach (var catagory in fhir.Category)
                {
                    foreach (var coding in catagory.Coding)
                    {
                        var observationitem = new ObservationItem();
                        observationitem.TypeName = "Category Code";
                        observationitem.Code.Name = coding.Code;
                        observationitem.Code.Description = coding.Display;
                        observationitem.Code.CodingSystem = GetCodingSystem(coding.System);
                        observationitem.Timestamp = DateTime.Now;
                        meta.Items.Add(observationitem);
                    }
                }
            }

            if (fhir.Code != null && fhir.Code.Coding.Any())
            {
                foreach (var code in fhir.Code.Coding)
                {
                    var observationitem = new ObservationItem();
                    observationitem.TypeName = "Code";
                    observationitem.Code.Name = code.Code;
                    observationitem.Code.Description = code.Display;
                    observationitem.Code.CodingSystem = GetCodingSystem(code.System);
                    observationitem.Timestamp = DateTime.Now;
                    meta.Items.Add(observationitem);
                }
            }

            if(fhir.Value != null)
            {
                var observationitem = new ObservationItem();
        
                foreach(KeyValuePair<string,object> code in fhir.Value)
                {
                    observationitem.Code.Name = fhir.Value.TypeName;
                    switch(code.Key.ToLower())
                    {
                        case "value":
                            observationitem.Code.Value = code.Value.ToString();
                            break;
                        
                        case "unit":
                            observationitem.Code.Units = code.Value.ToString();
                            break;

                        case "system":
                            observationitem.Code.CodingSystem = GetCodingSystem(code.Value.ToString());
                            break;
                    }
                }

                meta.Items.Add(observationitem);
            }

            if (fhir.Component != null && fhir.Component.Any())
            {
                foreach (var component in fhir.Component)
                {
                    foreach (var code in component.Code.Coding)
                    {
                        var observationitem = new ObservationItem();
                        observationitem.TypeName = "Component Code";
                        observationitem.Code.Name = code.Code;
                        observationitem.Code.Description = code.Display;
                        observationitem.Code.CodingSystem = GetCodingSystem(code.System);
                        observationitem.Timestamp = DateTime.Now;
                        meta.Items.Add(observationitem);
                    }
                }
            }

            if (fhir.Contained != null && fhir.Contained.Any())
            {
                foreach (var contained in fhir.Contained)
                {
                    foreach (var item in contained)
                    {
                        var observationitem = new ObservationItem();
                        observationitem.TypeName = item.Key;
                        observationitem.Code.Name = item.Value.ToString();
                        observationitem.Timestamp = DateTime.Now;
                        meta.Items.Add(observationitem);
                    }
                }
            }

            if (fhir.Effective != null && fhir.Effective.Any())
            {
                foreach (var item in fhir.Effective)
                {
                    var observationitem = new ObservationItem();
                    observationitem.TypeName = item.Key;
                    observationitem.Code.Name = item.Value.ToString();
                    observationitem.Timestamp = DateTime.Now;
                    meta.Items.Add(observationitem);
                }
            }

            if (fhir.Encounter != null && fhir.Encounter.Any())
            {
                foreach (var item in fhir.Encounter)
                {
                    var observationitem = new ObservationItem();
                    observationitem.TypeName = item.Key;
                    observationitem.Code.Name = item.Value.ToString();
                    observationitem.Timestamp = DateTime.Now;
                    meta.Items.Add(observationitem);
                }
            }
            
            if (fhir.Subject != null && fhir.Subject.Any())
            {
                foreach (var item in fhir.Subject)
                {
                    var observationitem = new ObservationItem();
                    observationitem.TypeName = item.Key;
                    observationitem.Code.Name = item.Value.ToString().Contains("urn")
                        ? item.Value.ToString().Substring(9)
                        : item.Value.ToString();

                    observationitem.Timestamp = DateTime.Now;
                    meta.Items.Add(observationitem);
                }
            }
            
            if (fhir.Performer != null && fhir.Performer.Any())
            {
                foreach (var item in fhir.Performer)
                {
                    var observationitem = new ObservationItem();
                    try
                    {
                        meta.PractitionerId = Guid.Parse(item.ReferenceElement.Value);
                    }
                    catch
                    {
                        meta.PractitionerId = Guid.Empty;
                        meta.AlternateId = long.Parse(item.ReferenceElement.Value);
                    }
                }
            }
           
            if (fhir.Note != null && fhir.Note.Any())
            {
                foreach (var item in fhir.Note)
                {
                    var observationitem = new ObservationItem();               
                    observationitem.Code.Name = item.TypeName;
                    observationitem.Timestamp = DateTime.Now;
                    meta.Items.Add(observationitem);
                }
            }
            

            // Id - Item Reference? Subject - Patient? BasedOn

            // Effective.Value - Observation DateTime

            // Encounter Note Performer Value

            // Status

            return meta as OEntity;
        }



        public ObservationAdapter(InputVersion version)
        {
            this.version = version;
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