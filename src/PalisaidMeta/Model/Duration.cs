using System;
namespace PalisaidMeta.Model
{
	public class Duration : Entity
	{
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }
        public TimeSpan Length { get => EndDate - StartDate; }

        public Duration()
        { }

        public Duration(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId)
        { }

        protected override void Dispose(bool disposing)
        {
			StartDate = DateTimeOffset.MinValue;
			EndDate = DateTimeOffset.MinValue;
        }
    }
}

