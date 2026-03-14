namespace Collector.Interface
{
    public interface ICollector
    {
        Task RegisterCollector();

        Task RegisterTransporter();

        Task RegisterTransformer(PalisaidMeta.Model.DataProtocol dataProtocolIn);

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