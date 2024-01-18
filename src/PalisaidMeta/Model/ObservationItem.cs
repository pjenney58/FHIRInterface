/*
 MIT License - ObservationItem.cs

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
    public enum ObservationType
    {
        Behavior,
        Reaction,
        Report,
        Visual,
        Audible,
        Olfactory,
        Tactile,
        Taste,
        Note,
        Empty
    }

    public class ObservationItem : Entity
    {
        public Guid ObservationReference { get; set; }

        public ObservationItem() 
        { }
        
        public ObservationItem(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) 
            { }

        public ObservationType? ObservationType { get; set; }
        public Code Code { get; set; } = new();
        public string? TypeName { get; set; }
        public string? Description { get; set; }
        
        public string? Value { get; set; }
        public Dictionary<string,string> Values { get; set; } = new();
        
        public DateTimeOffset Timestamp { get; set; }
        public int Quantity { get; set; }

        public List<string> Interpretation { get; set; } = new();
        public List<string> Notes { get; set; } = new();

        public Code BodySite { get; set; } = new();
        public Uri? BodyStructure { get; set; }

        public Code Method { get; set; } = new();

        public Specimen Specimen { get; set; } = new();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Code.Dispose();
                Method.Dispose();
                BodySite.Dispose();
                
                Value = null;
                Description = null;
                Timestamp = DateTime.MinValue;
            }
        }
    }
}