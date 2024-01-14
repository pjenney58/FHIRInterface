/*
 MIT License - Medication.cs

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
    public class Medication : Entity
    {
        public Medication() { }

        public Medication(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) { }

       
        public Guid MedicationId { get => EntityId; set => EntityId = value; }

        public string? MedicationCode { get; set; }
        public bool IsGeneric { get; set; }
        public bool IsOtc { get; set; }
        public string? BrandName { get; set; }
        public string? GenericName { get; set; }
        public string? Manufacturer { get; set; }
        public string? LotId { get; set; }
        public DateTimeOffset? ExpireDate { get; set; }
        public string? Strength { get; set; }
        public string? StrengthUnits { get; set; }
        public string? Route { get; set; }
        public string? Form { get; set; }
        public string? ShortVisualDescription { get; set; }
        public string? LongVisualDescription { get; set; }
        public string? SpecialInstructions { get; set; }
        public string? Schedule { get; set; }       
        public string? RxCuiCode { get; set; }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                MedicationCode = null;
                BrandName = null;
                GenericName = null;
                Manufacturer = null;
                LotId = null;
                Strength = null;
                StrengthUnits = null;
                Route = null;
                Form = null;
                ShortVisualDescription = null;
                LongVisualDescription = null;
                Schedule = null;
            }
        }       
    }
}