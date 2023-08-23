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

		public async Task RegisterCollector(CollectorConfig config)
		{ }


		public async Task Start()
		{ }

		public async Task ShutDown()
		{ }

		public async Task PanicStop()
		{ }

	}
}

