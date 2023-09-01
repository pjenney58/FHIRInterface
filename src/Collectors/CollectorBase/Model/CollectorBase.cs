using System;
using DataShapes.Model;

namespace CollectorBase.Model
{
	public abstract class CollectorBase
	{
		private Scheduler scheduler;
		private CollectorConfig config;
		private DataShapeContext context;

		public CollectorBase()
        {
			
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task RegisterCollector(CollectorConfig config)
        {
			this.config = config;
		}

		public async Task Start()
		{ }

		public async Task ShutDown()
		{ }

		public async Task PanicStop()
		{ }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

	}
}

