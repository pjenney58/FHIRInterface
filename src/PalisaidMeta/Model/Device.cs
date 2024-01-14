using System;
namespace PalisaidMeta.Model
{
    public class Device : Entity
	{

		public Device() { }
        

        public Device(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) { }

        public List<string> Identifier { get; set; } = new();
		public string? DisplayName { get; set; } = nameof(Device);
		public UdiCarrier? UdiCarrier { get; set; }
		public Code? Status { get; set; }
		public string? AvailabilityStatus { get; set; }
		public string? BiologicalSourceEvent { get; set; }

		public string? Manufacturer { get; set; }
		public DateTimeOffset ManufactureDate { get; set; } = DateTimeOffset.MinValue;
		public DateTimeOffset ExpireyDate { get; set; } = DateTimeOffset.MinValue;
		public string? LotId { get; set; }
		public string? SerialNumber { get; set; }

		public string? DeviceName { get; set; } = nameof(Device);
		public Code? DeviceType { get; set; } 

		public string? ModelNumber { get; set; }
		public string? PartNumber { get; set; }
		public Code? DeviceCategory { get; set; } 

		public List<string> Versions { get; set; } = new();

		public Code? Mode { get; set; }
		public decimal Cycle { get; set; }
		public Duration? Duration { get; set; }

		public string? Organization { get; set; }
		public string? Contact { get; set; }
		public string? Location { get; set; }
		public Code? DeviceCode { get; set; }
		public string? Annotation { get; set; }
		public string? Reference { get; set; }

        protected override void Dispose(bool disposing)
        {
			Identifier.Clear();
			Identifier.TrimExcess();
			UdiCarrier?.Dispose();
			Status?.Dispose();
			AvailabilityStatus = null;
			BiologicalSourceEvent = null;
			Manufacturer = null;
			ManufactureDate = DateTimeOffset.MinValue;
			ExpireyDate = DateTimeOffset.MinValue;
			LotId = null;
			SerialNumber = null;
			DeviceName = null;
			DeviceType?.Dispose();
			ModelNumber = null;
			PartNumber = null;
			DeviceCategory?.Dispose();
			Versions.Clear();
			Versions.TrimExcess();
			Mode?.Dispose();
			Duration?.Dispose();
			Organization = null;
			Contact = null;
			Location = null;
			DeviceCode?.Dispose();
			Annotation = null;
			Reference = null;
        }
    }
}

