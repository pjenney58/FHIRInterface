using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transformers.Model;

namespace Transformers.Interface
{
    public struct TransformerPayload
    {
        public Type Type1 { get; set; }
        public Type Type2 { get; set; }
        public HL7Format Format { get; set; }
        public Hl7Version Version { get; set; }
        public SourceSystems SourceHost { get; set; }
        public object data { get; set; }
    }
}
