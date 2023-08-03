/*
 MIT License - Encounter.cs

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

using System.ComponentModel;

using System.Runtime.CompilerServices;

namespace DataShapes.Model
{
    public enum EncounterLocation
    {
        Home,
        Office,
        Clinic,
        UrgentCare,
        Hospital,
        School,
        Other
    }

    public enum EncounterType
    {
        // For filtering
        Undefined = -1,
        Ignored,

        // Actual event types
        Scheduled,
        Appointment,
        Office,
        OfficeVisit,
        UrgentCareVisit,
        EmergencyRoomVisit,
        TeleMedicineVisit,
        BloodWork,
        LabTest,
        VisonTest,
        StressTest,
        SampleTaken,
        Innoculation,
        MedicationDose,
        Treatment,
        Surgery,
        OutpatientSurgery,
        InpatientSurgery,
        Consult,
        Proceedure,
        Class,
        A1CChange
    }

    public enum EncounterStatus
    {
        New,
        Planned,
        Rescheduled,
        Arrived,
        Triaged,
        InProgress,
        Onleave,
        Finished,
        Changed,
        Cancelled,
        Deleted,
        EnteredInError,
        Noshow,
        Requested,
        Complete,
        ReadyForReview,
        Unknown
    }

    /// <summary>
    /// Based on HL7 Encounter
    /// </summary>
    public class Encounter : Entity
    {
        public Encounter() {}

        public Encounter(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) {}

        public List<string> EncounterIdString { get; set; } = new();
        public List<string> EncounterReasonString { get; set; } = new();

        private EncounterStatus _encounterStatus;

        [NotMapped]
        public EncounterStatus EncounterStatus
        {
            get => _encounterStatus;
            set { _encounterStatus = value; }
        }

        [NotMapped]
        public string EncounterStatusString
        {
            get => _encounterStatus.ToString();
            set => Enum.TryParse<EncounterStatus>(value, out _encounterStatus);
        }

        private EncounterType _encounterType;

        [NotMapped]
        public EncounterType EncounterType
        {
            get => _encounterType;
            set => _encounterType = value;  // TODO: OnPropertyChanged("EventStatus");
        }

        [NotMapped]
        public string EncounterTypeString
        {
            get => _encounterType.ToString();
            set => Enum.TryParse<EncounterType>(value, out _encounterType);
        }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset StopDate { get; set; }

        [NotMapped]
        public TimeSpan Duration { get => StopDate - StartDate; set { } }

        public DisposableList<Code> Codes { get; set; } = new();
        public DisposableList<Patient> Patients { get; set; } = new();
        public DisposableList<Practitioner> Practitioners { get; set; } = new();
        public DisposableList<Practitioner> ReferringPractitioners { get; set; } = new();
        public DisposableList<Location> Locations { get; set; } = new();
        public DisposableList<Diagnosis> Diagnoses { get; set; } = new();
        public DisposableList<Observation> Observations { get; set; } = new();

        public Prescription? Prescription { get; set; }
        public Treatment? Treatment { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: Dispose Encounter
                Codes.Dispose();
                Patients.Dispose();
                Practitioners.Dispose();
                ReferringPractitioners.Dispose();
                Locations.Dispose();
                Diagnoses.Dispose();
                Observations.Dispose();

                StartDate = DateTimeOffset.MinValue;
                StopDate = DateTimeOffset.MinValue;
            }
        }
    }
}