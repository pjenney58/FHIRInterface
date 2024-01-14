/*
 MIT License - Treatment.cs

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

namespace PalisaidMeta.Model
{
    public class Treatment : Entity
    {
        public Treatment() { }
       
        public Treatment(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) { }

        public string? TreatmentId { get; set; }
        public string? TreatmentName { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset StopDate { get; set; }
        public DateTimeOffset DcDate { get; set; }
        public DateTimeOffset DispenseDate { get; set; }

        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? Sig { get; set; }

        public Patient? Patient { get; set; } = new();
        public Practitioner? Practitioner { get; set; } = new();
        public Diagnosis? Diagnosis { get; set; } = new();
        public Location? Location { get; set; } = new();

        public decimal WrittenQuantity { get; set; }
        public DateTimeOffset WrittenDate { get; set; }

        public DoseSchedule? DoseSchedule { get; set; } = new();
        public DisposableList<DoseDay> DoseDays { get; set; } = new();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TreatmentId = null;
                Patient?.Dispose();
                Practitioner?.Dispose();
                Location?.Dispose();
                TreatmentName = null;
                ShortDescription = null;
                LongDescription = null;
                DoseDays?.Dispose();
                StartDate = DateTimeOffset.MinValue;
                StopDate = DateTimeOffset.MinValue;
            }
        }       
    }
}