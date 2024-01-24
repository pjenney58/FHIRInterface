using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PalisaidMeta.Model;
using System.Threading.Tasks;

namespace IntegrationTests
{
    // Add a reference to the Microsoft.EntityFrameworkCore assembly
    // by installing the corresponding NuGet package.
    public class TDatabase
    {
        PalisaidMetaContext _context;
        Guid _entityId;
        Guid _tenantId;
        Guid _ownerId = Guid.Empty;

        public TDatabase()
        {
            _context = new PalisaidMetaContext();
            _tenantId = Guid.NewGuid();
        }

        [Fact]
        public async Task ProcessPatient()
        {
            await CreatePatient();
            await FindPatient();
            await UpdatePatient();
            await DeletePatient();
        }

        private async Task CreatePatient()
        {
            var patient = new Patient(_ownerId, _tenantId);
            _entityId = patient.EntityId;

            patient.Name.GivenName.Add("John");
            patient.Name.FamilyName = "Doe";

            patient.Addresses.Add(new Address(_entityId, _tenantId)
            {
                City = "New York",
                Country = "USA",
                PostalCode = "10001",
                State = "NY"
            });

            try
            {
                await _context.AddAsync(patient);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private async Task FindPatient()
        {
            try
            {
                var patient = await _context.Patients.FindAsync(_entityId);
                Assert.NotNull(patient);
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }   
        }

        private async Task UpdatePatient()
        {
            try
            {
                var patient = await _context.Patients.FindAsync(_entityId);
                Assert.NotNull(patient);

                patient.Name.MiddleName = "Q"; 
                patient.Name.FamilyName = "Doe";

                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private async Task DeletePatient()
        {
            try
            {
                var patient = await _context.Patients.FindAsync(_entityId);
                Assert.NotNull(patient);

                _context.Remove(patient);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }   
    }
}
