using PalisaidMeta.Model;

namespace Support.Model
{
    public struct TransformerPayload
    {
        public Type Type1 { get; set; }
        public Type Type2 { get; set; }
        public InputFormat Format { get; set; }
        public InputVersion Version { get; set; }
        public SourceSystems SourceHost { get; set; }
        public object data { get; set; }
    }
}