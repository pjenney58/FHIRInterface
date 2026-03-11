namespace PalisaidMeta.Model
{
    public class Specimen : Entity
    {
        public Specimen()
        { }

        public Specimen(Guid tenantId, Guid ownerId)
            : base(tenantId, ownerId) { }

        public Code? SpecimenType { get; set; } = new();
        public DateTimeOffset DateCollected { get; set; } = DateTimeOffset.Now;
        public List<string> Request { get; set; } = new();
        public Code? Role { get; set; } = new();
        public Code? Feature { get; set; }
        public string? FeatureDescription { get; set; }

        protected override void Dispose(bool disposing)
        {
            SpecimenType?.Dispose();
            Role?.Dispose();
            Feature?.Dispose();
            FeatureDescription = null;
            DateCollected = DateTimeOffset.MinValue;
            Request.Clear();
            Request.TrimExcess();
            Feature = null;
        }
    }
}