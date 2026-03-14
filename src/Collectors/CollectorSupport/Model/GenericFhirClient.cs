using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

using Task = System.Threading.Tasks.Task;

namespace Collector.Model
{
    public class GenericFhirClient //: IClient
    {
        private BaseFhirClient client;
        private Uri target;

        public GenericFhirClient()
        {
        }

        public List<string> Messages
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public async Task Connect(Uri target)
        {
            this.target = target;

            var settings = new FhirClientSettings
            {
                Timeout = 0,
                PreferredFormat = ResourceFormat.Json,
                VerifyFhirVersion = true,
                //ReturnPreference = ReturnPreference.Minimal
            };

            try
            {
                //client = new FhirClient(target.OriginalString, settings).WithStrictSerializer();
                client = new FhirClient(target.OriginalString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public Bundle? GetData<T>(string res)
        {
            var data = client.ReadAsync<Bundle>(res).Result;
            return data;
        }

        public async Task<Bundle> GetDataAsync(string res)
        {
            var data = await client.GetAsync(res);

            return (Bundle)data;
        }

        public async Task<int> PutData<T>(string message)
        {
            throw new NotImplementedException();
        }

        public async Task<int> PutData<T>(List<string> messages)
        {
            throw new NotImplementedException();
        }
    }
}