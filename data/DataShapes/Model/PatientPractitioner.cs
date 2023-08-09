/*
 MIT License - PatientPractitoner.cs

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

namespace DataShapes.Model
{
    public enum PractitionerRelationship
    {
        Primary,
        Secondary,
        Pharmacy,
        Other
    }

    public class PatientPractitioner : Entity
    {
        public PatientPractitioner() { }
     
        public PatientPractitioner(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) { }

        public PractitionerRelationship Relationship { get; set; }

        // Whole practitioner may be coelesed
        public Practitioner? Practitioner { get; set; }

        public Guid PractitionerId { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset StopDate { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Relationship = PractitionerRelationship.Other;
                Practitioner?.Dispose();
            }
        }
    }
}