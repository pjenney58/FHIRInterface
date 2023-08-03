using System;
namespace DataShapes.Model
{
	public class Duration 
	{
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }

        public void Dispose()
        {
			StartDate = DateTimeOffset.MinValue;
			EndDate = DateTimeOffset.MinValue;
        }
    }
}

