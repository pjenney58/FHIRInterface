using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Task = System.Threading.Tasks.Task;
using Collectors.Interface;
using Collectors.Model;

// Public Test Servers:
//		https://wiki.hl7.org/index.php?title=Publicly_Available_FHIR_Servers_for_testing


namespace DevTests
{
	public class TFhirClient
	{
		const string hapiserver = "http://hapi.fhir.org/baseR4";
		const string hapiserverpatients = "http://hapi.fhir.org/baseR4/Patient";

        public TFhirClient()
		{
		}


		[Fact]
		public async Task Connect()
		{
			try
			{
				var client = new GenericFhirClient();
				await client.Connect(new Uri(hapiserver));
				var data = await client.GetDataAsync(hapiserverpatients);
				Assert.NotNull(data);
			}
			catch(Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

/*
		[Fact]
		public void LoadCollector()
		{
			try
			{
				var c = CollectorFactory.Create("Fhir4");
				Assert.NotNull(c);
			}
			catch(Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
*/
		[Fact]
		public  async Task QueryForBob()
		{
			/*
			try
			{
				var client = new GenericFhirClient();
				await client.Connect(new Uri(hapiserver));
				

				var q = new SearchParams()
						.Where("family:exact=Alexander")
						.SummaryOnly().Include("Patient:organization")
						.LimitTo(1);

				Bundle? bundle = await client.SearchAsync<Patient>(q);
                Assert.NotNull(bundle);

                var patient = await client.ReadAsync<Patient>(bundle.Entry[0].FullUrl);
                Assert.NotNull(patient);
            }
			catch(Exception ex)
			{
				Console.WriteLine(ex);
			}
          
            //var stuff = 
            //var name = parsedBundle.Entry.ByResourceType<HumanName>();

            // /api/FHIR/R4/List?code={code}&identifier={identifier}
            // hostname/instance/api/FHIR/R4/List?identifier=urn:oid:1.2.840.114350.1.13.5325.1.7.2.698283|9192&code=patients
			*/
        }
    }
}

