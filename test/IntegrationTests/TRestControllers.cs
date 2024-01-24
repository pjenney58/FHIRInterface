
using System.ComponentModel;
using Microsoft.AspNetCore;
using PalisaidMeta.Model;
using System.Text.Json;
using System.Text;
using Support.Model;
using System.Net.Http.Headers;

namespace IntegrationTests
{
    public class TRestControllers
    {
        string token = string.Empty;
        Patient gPatient = null;

        public TRestControllers()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    Task.Run(async() =>
                    {
                        var payload = JsonSerializer.Serialize(new { username = "admin", password = "!Password0" });
                        var response = await client.PostAsync("http://localhost:5040/api/Authenticate/login", new StringContent(payload, Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        var responseString = await response.Content.ReadAsStringAsync();
                        var tokenModel = JsonSerializer.Deserialize<TokenModel>(responseString);
                        token = tokenModel.accessToken;
                    }).Wait();
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Fact]
        public async Task CyclePatient()
        {
            await POST_Patient();
            await GET_Patient();
            await PUT_Patient();
            await DELETE_Patient();
        }

        public async Task POST_Patient()
        {
            var patient = new Patient();
            patient.EntityId = Guid.NewGuid();
            patient.TenantId = Guid.NewGuid();
            patient.OwnerId = patient.TenantId;

            patient.Name.GivenName.Add("Jane");
            patient.Name.FamilyName = "Doe";
            patient.Room = "222";
            patient.Addresses.Add(new Address(patient.TenantId, patient.EntityId)
            {
                City = "New York",
                Country = "USA",
                PostalCode = "10001",
                State = "NY",
                Address1 = "123 Main St"
            });

            patient.Gender = Gender.Female;

            gPatient = patient;

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var payload = JsonSerializer.Serialize(patient);
                    var response = await client.PostAsync("http://localhost:5040/api/Patients/AddNewPatient", new StringContent(payload, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    var responseString = await response.Content.ReadAsStringAsync();
                    Assert.NotNull(responseString);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }
    
        public async Task GET_Patient()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync($"http://localhost:5040/api/Patients/GetById/{gPatient.EntityId}");
                    response.EnsureSuccessStatusCode();
                    var responseString = await response.Content.ReadAsStringAsync();
                    var patient = JsonSerializer.Deserialize<Patient>(responseString);
                    Assert.NotNull(patient);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }


        public async Task PUT_Patient()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    gPatient.Name.FamilyName = "Smith";
                    var payload = JsonSerializer.Serialize(gPatient);
                    var response = await client.PutAsync("http://localhost:5040/api/Patients/UpdatePatient", new StringContent(payload, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    var responseString = await response.Content.ReadAsStringAsync();
                    Assert.NotNull(responseString);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        public async Task DELETE_Patient()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.DeleteAsync($"http://localhost:5040/api/Patients/DeletePatient/{gPatient.EntityId}");
                    response.EnsureSuccessStatusCode();
                    var responseString = await response.Content.ReadAsStringAsync();
                    Assert.NotNull(responseString);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }
    }
}
