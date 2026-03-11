namespace PalisaidMeta.Model
{
    public class Collection : Entity
    {
        public Collection()
        { }

        public Collection(Guid tenantId, Guid ownerId)
            : base(tenantId, ownerId) { }

        public Guid CollectorId { get; set; }
        public TimeSpan Duration { get; set; } = TimeSpan.MinValue;
        public decimal Quantity { get; set; }
        public Code Method { get; set; } = new();
        public Code Device { get; set; } = new();
        public Code Procedure { get; set; } = new();
        public Code BodySite { get; set; } = new();
        public TimeSpan FastingDuration { get; set; } = TimeSpan.MinValue;

        public string ProcessingDescription { get; set; } = "Unknown";
        public Code ProcessingMethod { get; set; } = new();
        public List<string> ProcessingAdditive { get; set; } = new();
        public DateTimeOffset ProcessingDate { get; set; } = DateTimeOffset.Now;
        public TimeSpan ProcessingTime { get; set; } = TimeSpan.MinValue;
        public Uri? Container { get; set; }
        public Uri? Location { get; set; }
        public decimal ContainerQuantity { get; set; }
        public Code Condition { get; set; } = new();
        public List<string> Notes { get; set; } = new();

        protected override void Dispose(bool disposing)
        {
            CollectorId = Guid.Empty;
            Method.Dispose();
            Device.Dispose();
            Procedure.Dispose();
            BodySite.Dispose();
            ProcessingDescription = string.Empty;
            ProcessingMethod.Dispose();

            ProcessingAdditive.Clear();
            ProcessingAdditive.TrimExcess();

            Container = null;
            Location = null;
            Condition.Dispose();

            Notes.Clear();
            Notes.TrimExcess();
        }
    }
}