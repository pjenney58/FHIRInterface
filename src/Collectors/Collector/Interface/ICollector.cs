using System;
namespace Collector.Interface
{
	public interface ICollector : IDisposable
	{
		Task Connect();
		Task Deploy();
		Task Configure();
		Task<string> Retrieve();
		Task Destroy();
	}
}

