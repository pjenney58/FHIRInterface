using System;
using System.Threading.Tasks;

namespace Collectors.Interface
{
	public interface ICollector
	{
		Task RegisterCollector();
		Task RegisterTransporter();
		Task RegisterTransformer(DataShapes.Model.DataProtocol dataProtocolIn);
		Task RegisterScheduler();

		Task Connect();
		Task Deploy();
		Task Configure();
		Task Panic();
		Task<string> Retrieve();
		Task Persist();
		Task Destroy();
	}
}

