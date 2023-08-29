using System;
using System.Diagnostics;
using CollectorSupport.Model;

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

		[Fact]
		public  async Task Query()
		{
            /*
			 *  var q = new SearchParams()
			 *	.Where("name:exact=ewout")
             *  .OrderBy("birthdate", SortOrder.Descending)
             *  .SummaryOnly().Include("Patient:organization")
             *  .LimitTo(20);

			Bundle result = client.Search<Patient>(q);

			/api/FHIR/R4/List?code={code}&identifier={identifier}
			hostname/instance/api/FHIR/R4/List?identifier=urn:oid:1.2.840.114350.1.13.5325.1.7.2.698283|9192&code=patients
			*/
        }
    }
}

