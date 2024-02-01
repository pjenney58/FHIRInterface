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

using Hl7.Fhir.Model;
using Hl7.Fhir.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using PalisaidMeta.Model;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

        PalisaidMeta.Model.Code SetValues(PalisaidMeta.Model.Code code, DataType data)
        {
            foreach (KeyValuePair<string, object> set in data)
            {
                switch (set.Key.ToLower())
                {
                    case "value":
                        code.Value = set.Value.ToString();
                        break;

                    case "unit":
                        code.Units = set.Value.ToString();
                        break;

                    case "system":
                        code.System = set.Value.ToString();
                        break;
                }
            }

            return code;
        }
       
        KeyValuePair<string,string>? GetValue(KeyValuePair<string, object> set)
        {
            switch (set.Key.ToLower())
            {
                case "value":
                    return new KeyValuePair<string, string>("value", set.Value.ToString());

                case "unit":
                    return new KeyValuePair<string, string>("unit", set.Value.ToString());

                case "system":
                    return new KeyValuePair<string, string>("system", set.Value.ToString());
            }

            return default;
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
            
            var meta = new PalisaidMeta.Model.Observation()
            {
                TenantId = tenant,
            };

            if (meta == null)
            {
                throw new Exception("Invalid meta type");
            }

            meta.TenantId = tenant;
            
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
                    meta.OriginId = fhir.Id;
                }
            }

            if (fhir.Category != null && fhir.Category.Any())
            {
                foreach (var catagory in fhir.Category)
                {
                    foreach (var coding in catagory.Coding)
                    {
                        var Code = new PalisaidMeta.Model.Code
                        {
                            Name = coding.Code,
                            Description = coding.Display,
                            CodingSystem = GetCodingSystem(coding.System),
                            System = coding.System
                        };
                        
                        meta.Codes.Add(Code);
                    }
                }
            }

            if (fhir.Code != null && fhir.Code.Coding.Any())
            {
                foreach (var code in fhir.Code.Coding)
                {
                    var Code = new PalisaidMeta.Model.Code
                    {
                        Name = code.Code,
                        Description = code.Display,
                        CodingSystem = GetCodingSystem(code.System),
                        System = code.System
                    };
                    
                    meta.Codes.Add(Code);                  
                }
            }
            
            if (fhir.Component != null && fhir.Component.Any())
            {
                foreach (var component in fhir.Component)
                {
                    foreach (var code in component.Code.Coding)
                    {
                        var Code = new PalisaidMeta.Model.Code
                        {
                            Name = code.Code,
                            Description = code.Display,
                            CodingSystem = GetCodingSystem(code.System)
                        };

                        meta.Codes.Add(SetValues(Code, component.Value));
                    }
                }
            }

            if (fhir.Contained != null && fhir.Contained.Any())
            {
                foreach (var contained in fhir.Contained)
                {
                    foreach (var item in contained)
                    {
                        var observationitem = new ObservationItem
                        {
                            TypeName = item.Key,
                            TenantId = tenant
                        };
                        
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
                    if(fhir.Effective is FhirDateTime)
                    {
                        meta.StartDate = DateTimeOffset.Parse(fhir.Effective.ToString());
                    }
                    else if(fhir.Effective is Period)
                    {
                        if(meta.StartDate == DateTimeOffset.MinValue)
                        {
                            meta.StartDate = DateTimeOffset.Parse(fhir.Effective.ToString());
                        }
                        else
                        {
                            meta.StopDate = DateTimeOffset.Parse(fhir.Effective.ToString());
                        }
                    }
                }
            }

            if (fhir.Encounter != null && fhir.Encounter.Any())
            {
                foreach (var item in fhir.Encounter)
                {
                    var observationitem = new ObservationItem
                    {
                        TypeName = item.Key,
                        TenantId = tenant
                    };

                    observationitem.Code.Name = "Encounter";
                    observationitem.Timestamp = DateTime.Now;
                    meta.Items.Add(observationitem);
                }
            }
            
            if (fhir.Subject != null && fhir.Subject.Any())
            {
                foreach (var item in fhir.Subject)
                {
                    
                    if(fhir.Subject is Hl7.Fhir.Model.Patient)
                    {
                        meta.PatientId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Location)
                    {
                        meta.LocationId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Practitioner)
                    {
                        meta.PractitionerId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    /*
                    else if(fhir.Subject is Hl7.Fhir.Model.Group)
                    {
                        meta.GroupId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Device)
                    {
                        meta.DeviceId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Location)
                    {
                        meta.LocationId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Organization)
                    {
                        meta.OrganizationId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Practitioner)
                    {
                        meta.PractitionerId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.RelatedPerson)
                    {
                        meta.RelatedPersonId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Substance)
                    {
                        meta.SubstanceId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Specimen)
                    {
                        meta.SpecimenId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Group)
                    {
                        meta.GroupId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Location)
                    {
                        meta.LocationId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Organization)
                    {
                        meta.OrganizationId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Practitioner)
                    {
                        meta.PractitionerId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.RelatedPerson)
                    {
                        meta.RelatedPersonId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    else if(fhir.Subject is Hl7.Fhir.Model.Substance)
                    {
                        meta.SubstanceId = Guid.Parse(fhir.Subject.ReferenceElement.Value);
                    }
                    */

                    var observationitem = new ObservationItem
                    {
                        TypeName = item.Key,
                        TenantId = tenant
                    };

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
                    var observationitem = new ObservationItem()
                    {
                        TypeName = "Note",
                        TenantId = tenant
                    };
                              
                    observationitem.Code.Name = item.TypeName;
                    observationitem.Timestamp = DateTime.Now;
                    meta.Items.Add(observationitem);
                }
            }
            

            // Id - Item Reference? Subject - Patient? BasedOn

            // Effective.value - Observation DateTime

            // Encounter Note Performer value

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