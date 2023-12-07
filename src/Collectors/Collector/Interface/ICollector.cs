using System;
using System.Threading.Tasks;

namespace Collectors.Interface
{
	public interface ICollector
	{
		Task Connect();
		Task Deploy();
		Task Configure();
		Task Panic();
		Task<string> Retrieve();
		Task Persist();
		Task Destroy();
	}
}

