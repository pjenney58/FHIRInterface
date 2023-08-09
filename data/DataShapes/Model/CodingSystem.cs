/*
 MIT License - CodingSystem.cs

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
    public enum CodingSystem
    {
        ICD,
        ICD9,
        ICD10,
        ICD11,
        ATC,
        SNOMED,
        USCDI,
        ICHI,
        ICPM,
        HCPCS,
        GTIN,
        NDC,
        DIN,
        Unknown,
        v3_ActCode
    }

    public class Code : Entity
    {
        public CodingSystem CodingSystem { get; set; }
        public string? CodingSystemName { get => CodingSystem.ToString(); }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }

        public Code() {}

        public Code(Guid ownerId, Guid tenantId)
            : base(ownerId,tenantId) { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CodingSystem = CodingSystem.Unknown;
                Name = null;
                Description = null;
                Link = null;
            }
        }    
    }
}