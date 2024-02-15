/*
 MIT License - PrescriptionAdapter.cs

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
using PalisaidMeta.Model;
using Transformers.Interface;

namespace Transformers.Model.Stu3
{
    public class PrescriptionAdapter<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;

        public delegate OEntity VoidDelegate();

        public delegate Task<OEntity> TaskDelegate();

        public InputVersion version { get; set; }
        public InputFormat format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public PrescriptionAdapter(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
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
            var fhir = payloadIN as Hl7.Fhir.Model.MedicationRequest;
            if (fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }

            var meta = new PalisaidMeta.Model.Prescription()
            {
                TenantId = this.tenant,
                EntityId = fhir.Id ?? Guid.NewGuid().ToString(),
                OwnerId = Guid.Parse(fhir.Subject.Reference.Substring("urn:uuid:".Length)),
                CreateDate = DateTimeOffset.UtcNow,
                LastUpdate = DateTimeOffset.UtcNow,
                WrittenDate = DateTimeOffset.Parse(fhir.AuthoredOn),
                DoNotPerform = fhir.DoNotPerform != null ? (bool)fhir.DoNotPerform.Value : false,
                IsActive = true
            };

            if (meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            if (fhir.Status != null && Enum.TryParse<PrescriptionStatus>(fhir.Status.Value.ToString(), out PrescriptionStatus _status))
            {
                meta.Status = _status;
            }

            if (fhir.Intent != null && Enum.TryParse<PrescriptionIntent>(fhir.Intent.Value.ToString(), out PrescriptionIntent _intent))
            {
                meta.Intent = _intent;
            }

            if (fhir.Priority != null && Enum.TryParse<PrescriptionPriority>(fhir.Priority.Value.ToString(), out PrescriptionPriority _priority))
            {
                meta.Priority = _priority;
            }

            if (fhir.StatusReason != null && fhir.StatusReason.Count() > 0)
            {
                foreach (var reason in fhir.StatusReason)
                {
                    meta.StatusReasons.Add(reason.Value.ToString());
                }
            }

            if (fhir.Category != null)
            {
                foreach (var catagory in fhir.Category)
                {
                    if (Enum.TryParse<PrescriptionCatagory>(catagory.Text, out PrescriptionCatagory _catagory))
                        meta.Catagories.Add(_catagory);
                }
            }

            if (fhir.DispenseRequest != null)
            {
                meta.WrittenQuantity = (decimal)fhir.DispenseRequest.InitialFill.Quantity.Value;
                meta.StartDate = DateTimeOffset.Parse(fhir.DispenseRequest.ValidityPeriod.Start);
                meta.StopDate = DateTimeOffset.Parse(fhir.DispenseRequest.ValidityPeriod.End);
                meta.MaximumRefills = fhir.DispenseRequest.NumberOfRepeatsAllowed.Value + 1;
            }

            meta.PriorPrescriptionName = fhir.PriorPrescription?.Display;

            if (fhir.DosageInstruction != null)
            {
                foreach (var instruction in fhir.DosageInstruction)
                {
                    meta.Sig += $"{instruction?.PatientInstruction}\r";

                    foreach (var time in instruction?.Timing.Repeat.TimeOfDay)
                    {
                        if (instruction.DoseAndRate != null)
                        {
                            var day = new DoseDay()
                            {
                                TenantId = tenant,
                                OwnerId = Guid.Parse(meta.EntityId),
                                EntityId = Guid.NewGuid().ToString(),
                                CreateDate = DateTimeOffset.Now,
                                IsActive = true
                            };

                            foreach (var dose in instruction.DoseAndRate)
                            {
                                day.DoseEvents.Add(new DoseEvent()
                                {
                                    MaxmumCount = (decimal)dose.Dose.ElementAt(0).Value,
                                    Time = DateTime.Parse(time).TimeOfDay
                                });
                            }

                            meta.DoseSchedule?.DoseDays.Add(day);
                        }
                    }
                }

                if (fhir.Medication != null)
                {
                    if (fhir?.Medication?.TypeName == "CodeableConcept")
                    {
                        var med = fhir.Medication as CodeableConcept;
                        meta.Code = new()
                        {
                            Name = med?.Coding?.FirstOrDefault()?.Code,
                            Description = med?.Coding?.FirstOrDefault()?.Display,
                            System = med?.Coding?.FirstOrDefault()?.System
                        };

                        // Medication lookup
                        // https://rxnav.nlm.nih.gov/REST/rxcui?idtype=SNOMEDCT&id=261000 Returns
                        // NDC list by labeler Call for individual NDC details
                        // TODO: Hook up medication Retriever

                        /*
                        meta.Medication = await ndc.GetByRxcui(med.Coding.FirstOrDefault().Code);

                        if (meta.Medication != null)
                        {
                            meta.Medication.TenantId = this.tenant;
                            meta.Medication.EntityId = Guid.Parse(fhir.Id);
                            meta.Medication.OwnerId = Guid.Parse(fhir.Subject.Reference.Substring("urn:uuid:".Length));
                            meta.Medication.RxCuiCode = med.Coding.FirstOrDefault().Code;
                            meta.Medication.Description = med.Text;
                        }
                        */

                        // CourseOfTherapyType DetectedIssues DispenseRequest DoNotPerform
                        // DosageInstruction.DoseAndRate.Dose DoseInstruction.AdditionalInstruction
                        // DoseInstruction.AsNeeded (PRN) DosageInstruction.Type
                        // DosageInstruction.MaxDosePerAdministration
                        // DosageInstruction.MaxDosePerLifetime DosageInstruction.MaxDosePerPeriod
                        // DosageInstruction.Method DosageInstruction.PatientInstruction
                        // DosageInstruction.Route DosageInstruction.Sequence DosageInstruction.Site
                        // DosageInstruction.Text DosageInstruction.Timing
                        // DosageInstruction.Timing.Repeat.DayOfWeek
                        // DosageInstruction.Timing.Repeat.Duration
                        // DosageInstruction.Timing.Repeat.DurationMax
                        // DosageInstruction.Timing.Repeat.DurationUnit
                        // DosageInstruction.Timing.Repeat.Frequency
                        // DosageInstruction.Timing.Repeat.FrequencyMax
                        // DosageInstruction.Timing.Repeat.Offset
                        // DosageInstruction.Timing.Repeat.Period
                        // DosageInstruction.Timing.Repeat.PeriodMax
                        // DosageInstruction.Timing.Repeat.PeriodUnit
                        // DosageInstruction.Timing.Repeat.TimeOfDay
                        // DosageInstruction.Timing.Repeat.When Encounter EventHistory ImplicitRules
                        // Insurance ScripIntent Language Medication Medication.Coding
                        // Medication.Coding.Code Medication.Coding.Display Medication.Coding.System
                        // Medication.Text Note Performer PerformerType PriorPrescription Priority
                        // ReasonCode ReasonReference Recorder Reported Requester Status
                        // StatusReason Subject Substitution SupportingInforrmation
                    }
                }
            }

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
            var meta = payloadIN as PalisaidMeta.Model.Prescription;
            var fhir = new Hl7.Fhir.Model.MedicationRequest();
            throw new NotImplementedException();
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
            Dictionary<Tuple<string, InputVersion>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.MedicationRequest => PalisaidMeta.Model.Prescription", InputVersion.HL7FhirR4), ConvertFhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model.Prescription => Hl7.Fhir.Model.MedicationRequest", InputVersion.HL7FhirR4), ConvertMetaToFhir }
            };

            payloadIN = payload as IEntity;

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