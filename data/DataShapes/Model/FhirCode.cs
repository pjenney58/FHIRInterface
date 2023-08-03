

using System;

namespace DataShapes.Model
{
    public class MetaCode : IDisposable
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }

        public MetaCode() { }

        public void Dispose()
        {
            Code = null;
            Description = null;
            Value = null;

            GC.SuppressFinalize(this);
        }
    }
}