/*
 MIT License - Allergy.cs

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
    public class Allergy : Entity
    {
        public Allergy()
        { }

        public Allergy(Guid tenantId, Guid ownerId)
            : base(tenantId, ownerId) { }

        public string? Icd9Code { get; set; }
        public string? Icd9ShortDescription { get; set; }
        public string? Icd9LongDescription { get; set; }

        public string? Icd10Code { get; set; }
        public string? Icd10ShortDescription { get; set; }
        public string? Icd10LongDescription { get; set; }

        public string? Icd11Code { get; set; }
        public string? Icd11ShortDescription { get; set; }
        public string? Icd11LongDescription { get; set; }

        public string? SNOWMEDCode { get; set; }
        public string? SNOWMEDDescription { get; set; }
        public string? SNOMEDUri { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Icd9Code = null;
                Icd9ShortDescription = null;
                Icd9LongDescription = null;

                Icd10Code = null;
                Icd10ShortDescription = null;
                Icd10LongDescription = null;

                Icd11Code = null;
                Icd11ShortDescription = null;
                Icd11LongDescription = null;

                SNOWMEDCode = null;
                SNOWMEDDescription = null;
                SNOMEDUri = null;
            }
        }
    }
}