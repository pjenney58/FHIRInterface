/*
 MIT License - Diagnosis.cs

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
    /// <summary>
    /// A mapping of alll the resources that led to and describe a diagnosis. All treatments,
    /// prescriptions, referrals, etc are based in a diagnosis
    /// </summary>
    public class Diagnosis : Entity
    {
        public Diagnosis()
        { }

        public Diagnosis(Guid tenantId, Guid ownerId)
            : base(tenantId, ownerId) { }

        /// <summary>
        /// Unique name for the diagnosis
        /// </summary>
        public string? DiagnosisName { get; set; }

        public Patient Patient { get; set; } = new();

        public DisposableList<Code>? Codes { get; set; } = new();
        public DisposableList<Practitioner>? Practitioners { get; set; }

        // This could be an aggregate of all practitioner and patient locations
        public DisposableList<Location>? Locations { get; set; }

        public Guid CurrentLocation { get; set; }

        // Add Spatial Data

        /// <summary>
        /// Condition begin and end, if ended
        /// </summary>
        public Duration Duration { get; set; } = new();

        // Calculate this from the practioner list ...
        /// <summary>
        /// Tests, consults, visits, ...
        /// </summary>
        public DisposableList<Encounter>? Encounters { get; set; } = new();

        /// <summary>
        /// Observations germane to the diagnosis other than events
        /// </summary>
        public DisposableList<Observation>? Observations { get; set; }

        /// <summary>
        /// Notes af any type
        /// </summary>
        public DisposableList<Note>? Notes { get; set; }

        /// <summary>
        /// Prescrptions/medicatiosn associated with the diagnosis
        /// </summary>
        public DisposableList<Prescription>? Prescriptions { get; set; }

        /// <summary>
        /// Treatments associated with the diagnosis
        /// </summary>
        public DisposableList<Treatment>? Treatments { get; set; }

        protected override void Dispose(bool disposing)
        {
            DiagnosisName = null;

            Patient?.Dispose();
            Practitioners?.Dispose();
            Locations?.Dispose();
            Encounters?.Dispose();
            Observations?.Dispose();
            Notes?.Dispose();
            Prescriptions?.Dispose();
            Treatments?.Dispose();

            Duration.StartDate = DateTimeOffset.MinValue;
            Duration.EndDate = DateTimeOffset.MinValue;
        }
    }
}