using System;
using System.Threading;
using RestSharp;
using RestSharp.Authenticators;

namespace CollectorSupport.Model
{
	public class RestLoginManager
	{
		RestClient client { get; set; }
		CancellationToken cancellationToken { get; set; }

		public string? url { get; set; } = "https://api.twitter.com/1.1";
        public string? username { get; set; }
		public string? password { get; set; }

		public RestLoginManager()
		{
		}

		public async Task Connect(Uri target)
		{
            var options = new RestClientOptions(url)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };

            client = new RestClient(options);           
        }


		public async Task<T> GetData<T>(string api)
		{
            var request = new RestRequest(api);

            // The cancellation token comes from the caller. You can still make a call without it.
            var response = await client.GetAsync<T>(request, cancellationToken);

			return (T) response;
        }
	}
}

