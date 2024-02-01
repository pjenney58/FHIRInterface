
using System.ComponentModel;
using Microsoft.AspNetCore;
using PalisaidMeta.Model;
using System.Text.Json;
using System.Text;
using Support.Model;
using System.Net.Http.Headers;
//using EasyNetQ;
// FK_ContactMethod_Tenants_TenantId
// FK_Address_Tenants_TenantId

namespace IntegrationTests
{
    public class TRestControllers
    {
        string token = string.Empty;
        Patient gPatient = null;
        Guid tenantid = Guid.NewGuid();

        string url = "http://localhost:5080/api/";
        public TRestControllers()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    Task.Run(async () =>
                    {
                        var payload = JsonSerializer.Serialize(new { username = "admin", password = "!Password0" });
                        var response = await client.PostAsync($"{url}Authenticate/login", new StringContent(payload, Encoding.UTF8, "application/json"));
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

        [Fact]
        public async Task POST_Patient()
        {
            var practitioner = new Practitioner(tenantid, tenantid);

            if (practitioner != null)
            {
                practitioner.Name.TenantId = tenantid;
                practitioner.Name.OwnerId = practitioner.EntityId;
                practitioner.Name.Prefix.Add("Dr");
                practitioner.Name.GivenName.Add("Jing Wu");
                practitioner.Name.MiddleName = "Blink";
                practitioner.Name.FamilyName = "Ma";
                practitioner.Name.Suffix.Add("MD");

                practitioner.ContactMethods.Add(new ContactMethod(tenantid, practitioner.EntityId)
                {
                    Phone = new Phone(tenantid, practitioner.EntityId)
                    {
                        Number = "603-555-1212",
                        Priority = PhonePriority.Primary
                    }
                });

                practitioner.ContactMethods.Add(new ContactMethod(tenantid, practitioner.EntityId)
                {
                    Email = new Email(tenantid, practitioner.EntityId)
                    {
                        Address = "ma@hospital.com",
                    }
                });
            }
            else
            {
                Assert.Fail("Practitioner is null");
            }

            var patient = new Patient(tenantid, tenantid);
            if (patient != null)
            {
                // TODO: there's got to be a better waay to do this
                patient.Name.TenantId = tenantid;
                patient.Name.OwnerId = patient.EntityId;

                patient.Name.Prefix.Add("Ms");
                patient.Name.Prefix.Add("Dr");
                patient.Name.GivenName.Add("Jane");
                patient.Name.GivenName.Add("Lucy");
                patient.Name.MiddleName = "Blink";
                patient.Name.FamilyName = "Doe";
                patient.Name.Suffix.Add("MD");
                patient.Name.Suffix.Add("PhD");

                patient.Addresses.Add(new Address(patient.TenantId, patient.EntityId)
                {
                    City = "New York",
                    Country = "USA",
                    PostalCode = "10001",
                    State = "NY",
                    Address1 = "123 Main St",
                    AddressType = AddressType.Primary
                });

                patient.Addresses.Add(new Address(patient.TenantId, patient.EntityId)
                {
                    City = "Plymouth",
                    Country = "USA",
                    PostalCode = "03264",
                    State = "NH",
                    Address1 = "10 Dodge Rd",
                    AddressType = AddressType.Holiday
                });

                patient.BirthDate = new DateTimeOffset(1980, 12, 21, 18, 26, 0, TimeSpan.Zero);
                patient.Gender = Gender.Female;

                patient.Practitioners.Add(new PatientPractitioner(tenantid, patient.EntityId)
                {
                    Practitioner = practitioner,
                    Relationship = PractitionerRelationship.Primary,
                    StartDate = new DateTimeOffset(1990, 11, 21, 0, 0, 0, TimeSpan.Zero),
                    StopDate = DateTimeOffset.MinValue
                });


               
            }
            else
            {
                Assert.Fail("Patient is null");
            }

            var payload = JsonSerializer.Serialize(patient);
            patient.OriginHash = patient.GenerateOriginHash(payload);

            gPatient = patient;

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Generate the payload again to include the origin hash
                    payload = JsonSerializer.Serialize(patient);
                    var response = await client.PostAsync($"{url}Patients/AddNewPatient", new StringContent(payload, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();

                    var responseString = await response.Content.ReadAsStringAsync();
                    Assert.NotNull(responseString);
                    //Assert.Equal(payload, responseString);

                    var returned_patient = JsonSerializer.Deserialize<Patient>(responseString);
                    Assert.NotNull(returned_patient);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Fact]
        public async Task GET_Patient()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var target = $"{url}Patients/GetById?patientid={gPatient.EntityId}";
                    var response = await client.GetAsync(target);
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

        [Fact]
        public async Task PUT_Patient()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    gPatient.Name.FamilyName = "Smith";
                    var payload = JsonSerializer.Serialize(gPatient);
                    var response = await client.PutAsync($"{url}Patients/UpdatePatient", new StringContent(payload, Encoding.UTF8, "application/json"));
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

        [Fact]
        public async Task DELETE_Patient()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.DeleteAsync($"{url}Patients/DeletePatient?patientid={gPatient.EntityId}");
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
