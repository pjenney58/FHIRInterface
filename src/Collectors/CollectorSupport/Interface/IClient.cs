namespace Collector.Interface
{
    public interface IClient
    {
        List<string> Messages { get; set; }

        List<T> GetData<T>();

        int PutData<T>(string message);

        int PutData<T>(List<string> messages);

        Task Connect(Uri target);
    }
}