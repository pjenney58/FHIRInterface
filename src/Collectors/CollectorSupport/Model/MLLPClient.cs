using Hl7Harmonizer.Transport.Model;

namespace CollectorSupport.Model
{
    public class MLLPClient
    {
        private IBaseEventLogger logger = new BaseEventLogger("MLLPClient");

        private TCPClient client;
        private bool secure;
        private string? username;
        private string? password;
        private string? address;
        private string? port;
        public List<string> Messages { get; set; } = new();

        public MLLPClient(string address, string port, bool secure, string? username = null, string? password = null)
        {
            try
            {
                this.address = address;
                this.port = port;
                this.secure = secure;
                this.username = username;
                this.password = password;

                client = new(address, port, secure);
            }
            catch (Exception ex)
            {
                throw new Exception(logger.ReportError(ex.Message, false));
            }
        }

        public List<string> Read()
        {
            Messages.Clear();
            Messages.TrimExcess();

            var data = client.Read();

            var messages = data.Split('\xB', StringSplitOptions.RemoveEmptyEntries);
            foreach (var message in messages)
            {
                Messages.Add(message.Remove(message.IndexOf('\x1C')));
            }

            return Messages;
        }

        public int Write(string message)
        {
            try
            {
                client.Write(message);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int Write(List<string> messages)
        {
            StringBuilder strings = new StringBuilder();
            foreach (var message in messages)
            {
                strings.Append($"\xB{message}\x1C\x0D");
            }

            try
            {
                client.Write(strings.ToString());
                return messages.Count;
            }
            catch
            {
                return 0;
            }
        }
    }
}