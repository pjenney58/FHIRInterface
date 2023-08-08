/*
 MIT License - Prescription.cs

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

namespace DataShapes.Model
{
    public enum RxType
    {
        Daily,
        DayOfMonth,
        Alternating,
        DayOfWeek,
        Prn,
        Sequential,
        MonthlyTitrating,
        WeeklyTitrating
    };

    public enum PrescriptionStatus
    {
        Active,
        OnHold,
        Pending,
        Cancelled,
        Completed,
        EnteredInError,
        Stopped,
        Draft,
        DC,
        Unknown
    };

    public enum PrescriptionIntent
    {
        Proposal,
        Plan,
        Order,
        OriginalOrder,
        ReflexOrder,
        FillerOrder,
        InstanceOrder,
        Option
    };

    public enum PrescriptionCatagory
    {
        Inpatient,
        Outpatient,
        Community,
        Discharge
    };

    public enum PrescriptionPriority
    {
        Routine,
        Urgent,
        Asap,
        Stat
    };

    public class Prescription : Entity
    {
        public Prescription() { }

        private void Init(Guid ownerId, Guid tenantId)
        {
            DoseSchedule = new(ownerId, tenantId);
        }

        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        public Prescription(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) { }

        /// <summary>
        /// Generally referred to as a Prescription Number, but many use alphas, e.g.987652C where C
        /// indicates Controlled
        /// </summary>
        public string? PrescriptionName { get; set; }

        public RxType RxType { get; set; }
        public string? PriorPrescriptionName { get; set; }

        public List<string>? StatusReasons { get; set; } = new();
        public List<PrescriptionCatagory> Catagories {get;set;} = new();

        public PrescriptionIntent Intent { get; set; }
        public PrescriptionCatagory Catagory { get; set; }
        public PrescriptionStatus Status { get; set; }
        public PrescriptionPriority Priority { get; set; }

        public bool DoNotPerform { get; set; }

        public Code? Code { get; set; }
        public Location? FillingPharmacy { get; set; }
        public Patient? Patient { get; set; }
        public Practitioner? Practitioner { get; set; }
        public Diagnosis? Diagnosis { get; set; }
        public Medication? Medication { get; set; }

        public string? Sig { get; set; }
        public decimal WrittenQuantity { get; set; }

        public DateTimeOffset WrittenDate { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset StopDate { get; set; }
        public DateTimeOffset DispenseDate { get; set; }
        public DateTimeOffset DcDate { get; set; }

        public DateTimeOffset RefillRequestDate { get; set; }
        public int RefillRequestType { get; set; }

        public bool IsIsolated { get; set; }

        public DoseSchedule? DoseSchedule { get; set; }

        public List<string>? SpecialInstructions { get; set; } = new();

        public int MaximumRefills { get; set; }
        public int RemainingRefills { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PrescriptionName = null;
                PriorPrescriptionName = null;

                StatusReasons?.Clear();
                StatusReasons?.TrimExcess();
                StatusReasons = null;

                Patient?.Dispose();
                Practitioner?.Dispose();
                FillingPharmacy?.Dispose();
                Medication?.Dispose();
                DoseSchedule?.Dispose();
                Code?.Dispose();
            }
        }
    }
}