using DataShapes.Model;

namespace Support.Model
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
