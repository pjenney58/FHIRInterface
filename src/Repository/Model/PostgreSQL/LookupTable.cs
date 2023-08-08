using System;
namespace Hl7Harmonizer.Repository.Model.PostgreSQL
{
	public class LookupTable
	{
		public Guid OwnerId { get; set; }
		public LookupResolver OwnerType { get; set; }
		public LookupResolver ItemType { get; set; }

		public LookupTable()
		{
		}
	}
}

