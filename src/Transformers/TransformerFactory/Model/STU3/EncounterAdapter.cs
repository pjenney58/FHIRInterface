/*
 MIT License - EncounterAdapter.cs

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
using Hl7.Fhir.Model;
using Task = System.Threading.Tasks.Task;
using Transformers.Interface;

namespace Transformers.Model.Stu3
{
    public class EncounterAdapter<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;
        private OEntity? payloadOUT;

        public delegate OEntity VoidDelegate();

        public delegate Task<OEntity?> TaskDelegate();

        public InputVersion version { get; set; }
        public InputFormat format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        private readonly IBaseEventLogger eventLogger = new BaseEventLogger("EncounterAdapter");

        public EncounterAdapter(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
        {
            this.tenant = tenant;
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private CodingSystem GetCodingSystem(string link)
        {
            var text = link.ToLower();

            if (text.Contains("snomed"))
                return CodingSystem.SNOMED;

            if (text.Contains("icd9") || text.Contains("icd-9"))
                return CodingSystem.ICD9;

            if (text.Contains("icd10") || text.Contains("icd-10"))
                return CodingSystem.ICD10;

            if (text.Contains("cdi") || text.Contains("uscdi"))
                return CodingSystem.USCDI;

            if (text.Contains("hcpcs"))
                return CodingSystem.HCPCS;

            if (text.Contains("v3-actcode"))
                return CodingSystem.v3_ActCode;

            return CodingSystem.Unknown;
        }

        private async Task<DataShapes.Model.Encounter> processFhirAppointment(ResourceReference appointment)
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Encounter;
            var meta = new DataShapes.Model.Encounter();

            if (fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }

            if (meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            meta.TenantId = this.tenant;
            meta.EntityId = Guid.Parse(fhir.Id);

            var codes = new List<DataShapes.Model.Code>();

            await System.Threading.Tasks.Task.Run(() =>
            {
                meta.EncounterType = EncounterType.Appointment;

                meta.TenantId = Constants.Transform;
                meta.EntityId = Guid.Parse(fhir.Id);

                if (fhir.Participant != null)
                {
                    meta.StartDate = DateTimeOffset.Parse(fhir.Period.Start);
                    meta.StopDate = DateTimeOffset.Parse(fhir.Period.End);
                }

                foreach (var ftype in fhir.Type)
                {
                    foreach (var code in ftype.Coding)
                    {
                        var cd = new DataShapes.Model.Code()
                        {
                            CodingSystem = GetCodingSystem(code.System),
                            Name = code.Code,
                            Description = code.Display,
                            Link = code.System
                        };

                        // Add the code to the list, they will be persisted at the end
                        meta.Codes.Add(cd);
                    }
                }

                foreach (var rc in fhir.ReasonCode)
                {
                    foreach (var code in rc.Coding)
                    {
                        var cd = new DataShapes.Model.Code()
                        {
                            CodingSystem = GetCodingSystem(code.System),
                            Name = code.Code,
                            Description = code.Display,
                            Link = code.System
                        };

                        meta.Codes.Add(cd);
                    }
                }

                /*
                if (codes.Count > 0)
                {
                    var repository = RepositoryFactory<DataShapes.Model.Code>.GetRepository(Constants.IgnorePartition, RepositoryIntent.DataStorage);
                    if (repository == null)
                    {
                        throw new NullReferenceException(nameof(repository));
                    }

                    foreach (var code in codes)
                    {
                        repository.CreateRecord(code);
                    }
                }
                */
            });

            return meta;
        }

        private const string US_NPI = "us-npi|";
        private const string URN_UUID = "urn:uuid:";

        private string getIndividualId(Uri uri)
        {
            if (uri != null)
            {
                int start = 0;

                if (uri.OriginalString.Contains(US_NPI))
                {
                    start = uri.OriginalString.IndexOf(US_NPI) + US_NPI.Length;
                }

                if (uri.OriginalString.Contains(URN_UUID))
                {
                    start = uri.OriginalString.IndexOf(URN_UUID) + URN_UUID.Length;
                }

                return uri.OriginalString.Substring(start);
            }

            return string.Empty;
        }

        private async Task<OEntity?> ConvertFhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Encounter;
            var meta = new DataShapes.Model.Encounter()
            {
                TenantId = this.tenant,
                EntityId = Guid.Parse(fhir.Id),
                OwnerId = Guid.Parse(fhir.Subject.Reference.Substring("urn:uuid:".Length)),
                CreateDate = DateTimeOffset.UtcNow,
                LastUpdate = DateTimeOffset.UtcNow
            };

            meta.EncounterStatus = (EncounterStatus)fhir.Status;

            if (fhir.Appointment.Count > 0)
            {
                ;
            }

            var metaList = new List<DataShapes.Model.Encounter>();
            var diagnoses = new List<Diagnosis>();

            // There are more than one appointment in a single encounter for some reason, so we'll
            // build a list and in order to keep the single OEntity return, we'll yield return the list
            await System.Threading.Tasks.Task.Run(async () =>
            {
                if (fhir.Period != null)
                {
                    meta.StartDate = DateTimeOffset.Parse(fhir.Period.Start);
                    meta.StopDate = DateTimeOffset.Parse(fhir.Period.End);
                }

                foreach (var ftype in fhir.Type)
                {
                    foreach (var code in ftype.Coding)
                    {
                        var cd = new DataShapes.Model.Code()
                        {
                            CodingSystem = GetCodingSystem(code.System),
                            Name = code.Code,
                            Description = code.Display,
                            Link = code.System
                        };

                        // Add the code to the list, they will be persisted at the end
                        meta.Codes.Add(cd);
                    }
                }

                var cla = new DataShapes.Model.Code()
                {
                    TenantId = tenant,
                    CodingSystem = GetCodingSystem(fhir.Class.System),
                    Name = fhir.Class.Code,
                    Description = "Class",
                    Link = fhir.Class.System
                };

                meta.Codes.Add(cla);

                foreach (var rc in fhir.ReasonCode)
                {
                    foreach (var code in rc.Coding)
                    {
                        var cd = new DataShapes.Model.Code()
                        {
                            TenantId = tenant,
                            CodingSystem = GetCodingSystem(code.System),
                            Name = code.Code,
                            Description = code.Display,
                            Link = code.System
                        };

                        meta.Codes.Add(cd);
                    }
                }

                if (fhir.Diagnosis.Count > 0)
                {
                    foreach (var diagnosis in fhir.Diagnosis)
                    {
                        var diag = new DataShapes.Model.Diagnosis();
                        diag.EntityId = Guid.Parse(diagnosis.ElementId.Substring("urn:uuid:".Length));

                        // meta.Diagnoses.Add(Guid.Parse(diagnosis.ElementId));
                    }
                }

                // Participants are practitoners
                foreach (var participant in fhir.Participant)
                {
                    meta.Participants.Add(new Participant()
                    {
                        type = typeof(DataShapes.Model.Practitioner),
                        Name = participant.Individual.Display,
                        IdString = participant.Individual.Reference.Substring(participant.Individual.Reference.IndexOf('|') + 1),
                        RoleType = participant.Type[0].Text,
                        StartDate = DateTimeOffset.Parse(participant.Period.Start),
                        StopDate = DateTimeOffset.Parse(participant.Period.End),
                    });
                }

                // Subjects are patients
                if (fhir.Subject.Count() > 0)
                {
                    foreach (var pair in fhir.Subject)
                    {
                        var p = new Participant();

                        if (pair.Key.ToLower() == "reference")
                        {
                            p.Name = fhir.Subject.Display;
                            p.type = typeof(DataShapes.Model.Patient);
                            p.Id = Guid.Parse(pair.Value.ToString().Substring("urn:uuid:".Length));

                            meta.Participants.Add(p);

                            // meta.Patients.Add(Guid.Parse(pair.Value.ToString().Substring("urn:uuid:".Length)));
                        }
                    }
                }

                foreach (var location in fhir.Location)
                {
                    meta.Participants.Add(new Participant()
                    {
                        type = typeof(DataShapes.Model.Location),
                        Name = location.Location.Display,
                        Id = Guid.Parse(location.Location.Reference.Substring(location.Location.Reference.IndexOf('|') + 1)),
                    });
                }

                var sp = new Participant();
                sp.type = typeof(DataShapes.Model.ServiceProvider);

                foreach (var pair in fhir.ServiceProvider)
                {
                    if (pair.Key.ToLower() == "reference")
                    {
                        sp.Id = Guid.Parse(pair.Value.ToString().Substring(pair.Value.ToString().IndexOf('|') + 1));
                    }
                    else if (pair.Key.ToLower() == "display")
                    {
                        sp.Name = pair.Value.ToString();
                    }
                }

                meta.Participants.Add(sp);

                // Process the Appointments, there may be several and will result in the same care
                // event just with a different appointment status
                if (fhir.Appointment.Count > 0)
                {
                    // An appointment is a Encounter
                    foreach (var record in fhir.Appointment)
                    {
                        //meta.EncounterStatus = record.
                        //metaList.Add(await processFhirAppointment(record));
                    }
                }
            });

            return meta as OEntity;
        }

        private async Task<OEntity?> ConvertMetaToFhir()
        {
            await Task.Run(() =>
            {
            });

            throw new NotImplementedException();
        }

        public async Task<object?> Transform(object payload)
        {
            // Override this with the appropriate key conditions - replace MSG as desired. There may
            // be several similar messages required, e.g. SIU & SRM
            Dictionary<Tuple<string, InputVersion>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.Encounter => DataShapes.Model..Encounter", InputVersion.HL7HhirStu3), ConvertFhirToMeta },
                { new Tuple<string, InputVersion>(@"DataShapes.Model..Encounter => Hl7.Fhir.Model.Encounter", InputVersion.HL7HhirStu3), ConvertMetaToFhir },
              };

            payloadIN = payload as IEntity;

            var jumpkey = new Tuple<string, InputVersion>($"{typeof(IEntity).FullName} => {typeof(OEntity).FullName}", version);

            if (jumpTable.TryGetValue(jumpkey, out TaskDelegate? funcC))
            {
                return await funcC();
            }

            return default;
        }
    }
}