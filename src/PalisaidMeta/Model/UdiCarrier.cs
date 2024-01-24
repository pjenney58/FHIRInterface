namespace PalisaidMeta.Model
{
    public class UdiCarrier : Entity
	{
		public string? DeviceIdentifier { get; set; }
		public Uri? Issuer { get; set; }
		public Uri? Juristiction { get; set; }
		public string? CarrierAIDC { get; set; }
		public string? CarrierHRF { get; set; }
		public Code? EntryType { get; set; }

		public UdiCarrier() { }

		public UdiCarrier(Guid tenantId, Guid ownerId)
			: base(tenantId, ownerId) { }
	}
}

