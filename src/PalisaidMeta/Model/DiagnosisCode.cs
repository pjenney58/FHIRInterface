/*
 MIT License - DiagnosisCode.cs

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
    public class DiagnosisCode : Entity
    {
        private CodingSystem CodeType;
        private string? CodeId { get; set; }
        private string? ShortDescription { get; set; }
        private string? LongDescription { get; set; }

        public DiagnosisCode() { }

        /// <summary>
        /// Coding system and code
        /// </summary>
        /// <param name="key"> </param>
        public DiagnosisCode(Guid tenantId, Guid ownerId)
            : base(tenantId, ownerId) { }

        // TODO: Finish Set Code in DiagnosisCodes - Needs Lookup
        public void SetCode(CodingSystem codetype, string? code)
        {
            CodeType = codetype;
            switch (codetype)
            {
                case CodingSystem.ICD10:
                    //var val = new ICD10(code);
                    //ShortDescription = val.ShortDescription;
                    //ßLongDescription = val.LongDescription;
                    break;

                case CodingSystem.SNOMED:
                    ShortDescription = "SNOWMED ";
                    LongDescription = "SNOWMED";
                    break;

                default:
                    break;
            }
            
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CodeId = null;
                ShortDescription = null;
                LongDescription = null;
            }
        }
    }
}