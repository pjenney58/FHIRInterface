using Collectors.Messaging;

namespace Collectors.Model
{
    public class RESTClient //: IClient
    {
        private MessageService _local;
        public List<string> Messages { get; set; } = new();

        private IBaseEventLogger eventLogger = new BaseEventLogger("RESTClient");
        private RestClient? client { get; set; }
        private CancellationToken cancellationToken { get; set; }

        public string url { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;

        public RESTClient(string url)
        {
            this.url = url;
        }

        public RESTClient(string url, string username, string password)
        {
            this.url = url;
            this.username = username;
            this.password = password;
        }

        public async Task Connect(Uri target)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(_local["BadCredentials"]);
            }

            var options = new RestClientOptions(url)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };

            client = new RestClient(options);
        }

        public async Task<T> GetData<T>(string api)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var request = new RestRequest(api);

            // The cancellation token comes from the caller. You can still make a call without it.
            var response = await client.GetAsync<T>(request, cancellationToken);

            return response;
        }

        public async Task PutData<T>(string api, T data)
        {
            if (client == null)
            {
                throw new ArgumentNullException(eventLogger.ReportError($"Null Client:{nameof(client)}"));
            }

            try
            {
                var request = new RestRequest(JsonSerializer.Serialize<T>(data), Method.Post);
                await client.PutAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(eventLogger.ReportError(ex.Message));
            }
        }
    }
}