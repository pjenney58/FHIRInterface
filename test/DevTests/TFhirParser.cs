using System.Diagnostics;
using DataShapes.Model;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Npgsql;
using RandomDataGenerator;
using Hl7Harmonizer.Adapters.Model;

/*
extern alias r5;
extern alias r4b;
extern alias r4;
extern alias stu3;

using R5 = r5::Hl7.Fhir.Model;
using R4B = r4b::Hl7.Fhir.Model;
using R4 = r4::Hl7.Fhir.Model;
using Stu3 = stu3::Hl7.Fhir.Model;
*/

namespace DevTests
{
    public class TFhirParser
    {
        private readonly string sourcedir = Environment.OSVersion.Platform == PlatformID.Win32NT
            ? "C:\\SandDriftSoftware\\data\\SyntheaData"
            : "/Users/petejenney/Projects/SyntheaData";

        private DataShapeContext _context;
        private Guid _tenantId = Guid.NewGuid();

        public TFhirParser()
        {
            //GetRandomMetaPatient();
            //GetRandomMetaPractitioner();

            _context = new DataShapeContext();

            lastDate = DateTime.Now.AddDays(-31);
        }

        [Fact]
        public void TestDisposableList()
        {
            try
            {
                DisposableList<PersonName> list = new();

                for (var i = 0; i < 10; i++)
                {
                    var patient = GetRandomMetaPatient();
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public void BuildRTypes()
        {
            //var R2_Address = new Hl7.Fhir.ElementModel. Address();
            //var R3_Patient = new HL7.Fhir.STU3.Patient();
            //var R4_Practitioner = new HL7.Fhir.R4.Practitioner();
            //var R5_Location = new HL7.Fhir.R5.Location();

            var address = new Hl7.Fhir.Model.Address();
        }

        /*
        [Theory]
        [InlineData("Address", Hl7Version.R4)]
        [InlineData("Appointment", Hl7Version.R4)]
        [InlineData("Observation", Hl7Version.R4)]
        public void TestClassFactory(string name, Hl7Version version)
        {
            try
            {
                var c = FhirClassFactory.GetClass(name, version);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        */

        #region SupportFunctions

        private string[]? files;
        private DataShapes.Model.Patient lastPatient;
        private DataShapes.Model.Practitioner lastPractitioner;

        private DateTime lastDate;

        private void buildFileList()
        {
            try
            {
                files = Directory.GetFiles(sourcedir);
            }
            catch
            {
                throw;
            }
        }

        private string? getFhirData()
        {
            try
            {
                if (files == null || files.Count() == 0)
                {
                    buildFileList();
                }

                var file = files?.ElementAt(RandomData.Integer(0, files.Length - 1));
                return File.ReadAllText(file);
            }
            catch
            {
                throw;
            }
        }

        private async Task<DataShapes.Model.Patient?> GetRandomMetaPatient()
        {
            var data = getFhirData();

            var parser = new FhirJsonParser();
            var parsedBundle = parser.Parse<Bundle>(data);

            if (null != parsedBundle)
            {
                foreach (var e in parsedBundle.Entry)
                {
                    if (e.Resource.TypeName == "Patient")
                    {
                        var patient = e.Resource as Hl7.Fhir.Model.Patient;

                        if (patient != null)
                        {
                            var fhirConverter = AdapterFactory<Hl7.Fhir.Model.Patient, DataShapes.Model.Patient>.GetAdapter(Guid.NewGuid(), Hl7Version.R4);
                            var metaPatient = await fhirConverter.Convert(patient);
                            lastPatient = metaPatient;
                            return lastPatient;
                        }
                    }
                }
            }

            return default;
        }

        private async Task<DataShapes.Model.Practitioner?> GetRandomMetaPractitioner()
        {
            var data = getFhirData();

            var parser = new FhirJsonParser();
            var parsedBundle = parser.Parse<Bundle>(data);

            if (null != parsedBundle)
            {
                foreach (var e in parsedBundle.Entry)
                {
                    if (e.Resource.TypeName == "Practitioner")
                    {
                        var practitioner = e.Resource as Hl7.Fhir.Model.Practitioner;

                        if (practitioner != null)
                        {
                            var fhirConverter = AdapterFactory<Hl7.Fhir.Model.Practitioner, DataShapes.Model.Practitioner>.GetAdapter(Guid.NewGuid(), Hl7Version.R4);
                            var metaPractitioner = await fhirConverter.Convert(practitioner);
                            lastPractitioner = metaPractitioner;
                            return lastPractitioner;
                        }
                    }
                }
            }

            return default;
        }

        #endregion SupportFunctions

        [Theory]
        //[InlineData(Hl7Version.Stu3)]
        //[InlineData(Hl7Version.R4b)]
        //[InlineData(Hl7Version.R5)]
        [InlineData(Hl7Version.R4)]
        public async System.Threading.Tasks.Task ProcessPrescriptions(Hl7Version version)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    // Arrange
                    for (var i = 0; i < 100; i++)
                    {
                        var data = getFhirData();
                        Assert.NotNull(data);

                        var parser = new FhirJsonParser();
                        Assert.NotNull(parser);

                        var parsedBundle = parser.Parse<Bundle>(data);
                        Assert.NotNull(parsedBundle);

                        // Act
                        if (parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.MedicationRequest>().Any())
                        {
                            var prescriptions = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.MedicationRequest>();

                            // Assert
                            Assert.NotNull(prescriptions);
                            Assert.True(prescriptions.Any());

                            foreach (var prescription in prescriptions)
                            {
                                var fhirConverter = AdapterFactory<Hl7.Fhir.Model.MedicationRequest, DataShapes.Model.Prescription>.GetAdapter(_tenantId, version);
                                Assert.NotNull(fhirConverter);

                                var metaPrescription = await fhirConverter.Convert(prescription);
                                Assert.NotNull(metaPrescription);
                            }
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Assert.Fail($"{e.Message}");
            }
        }

        [Theory]
        //[InlineData(Hl7Version.Stu3)]
        //[InlineData(Hl7Version.R4b)]
        //[InlineData(Hl7Version.R5)]
        [InlineData(Hl7Version.R4)]
        public async System.Threading.Tasks.Task ProcessTreatments(Hl7Version version)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    // Arrange
                    for (var i = 0; i < 100; i++)
                    {
                        var data = getFhirData();
                        Assert.NotNull(data);

                        var parser = new FhirJsonParser();
                        Assert.NotNull(parser);

                        var parsedBundle = parser.Parse<Bundle>(data);
                        Assert.NotNull(parsedBundle);

                        // Act
                        if (parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.MedicationRequest>().Any())
                        {
                            var prescriptions = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.MedicationRequest>();

                            // Assert
                            Assert.NotNull(prescriptions);
                            Assert.True(prescriptions.Any());

                            foreach (var prescription in prescriptions)
                            {
                                var fhirConverter = AdapterFactory<Hl7.Fhir.Model.MedicationRequest, DataShapes.Model.Prescription>.GetAdapter(_tenantId, version);
                                Assert.NotNull(fhirConverter);

                                var metaPrescription = await fhirConverter.Convert(prescription);
                                Assert.NotNull(metaPrescription);
                            }
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Assert.Fail($"{e.Message}");
            }
        }

        [Theory]
        //[InlineData(Hl7Version.Stu3)]
        //[InlineData(Hl7Version.R4b)]
        //[InlineData(Hl7Version.R5)]
        [InlineData(Hl7Version.R4)]
        public async System.Threading.Tasks.Task ProcessLocations(Hl7Version version)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    // Arrange
                    for (var i = 0; i < 100; i++)
                    {
                        var data = getFhirData();
                        Assert.NotNull(data);

                        var parser = new FhirJsonParser();
                        Assert.NotNull(parser);

                        var parsedBundle = parser.Parse<Bundle>(data);
                        Assert.NotNull(parsedBundle);

                        // Act
                        if (parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.Location>().Any())
                        {
                            var locations = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.Location>();

                            // Assert
                            Assert.NotNull(locations);
                            Assert.True(locations.Any());

                            foreach (var location in locations)
                            {
                                var fhirConverter = AdapterFactory<Hl7.Fhir.Model.Location, DataShapes.Model.Location>.GetAdapter(_tenantId, version);
                                Assert.NotNull(fhirConverter);

                                var metaLocation = await fhirConverter.Convert(location);
                                Assert.True(metaLocation != null);
                            }
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Assert.Fail($"{e.Message}");
            }
        }

        /*
        [Theory]
        [InlineData("617320")]
        [InlineData("131725")]
        [InlineData("665078")]
        public async System.Threading.Tasks.Task CheckCodes(string code)
        {
            var ndc = new NDC();
            var med = await ndc.GetByRxcui(code);
        }
        */

        [Theory]
        //[InlineData(Hl7Version.Stu3)]
        //[InlineData(Hl7Version.R4b)]
        //[InlineData(Hl7Version.R5)]
        [InlineData(Hl7Version.R4)]
        public async System.Threading.Tasks.Task ProcessPatients(Hl7Version version)
        {
            try
            {
                var encounterList = new List<DataShapes.Model.Encounter>();
                var patientList = new List<DataShapes.Model.Patient>();
                var locationList = new List<DataShapes.Model.Location>();
                var practionerList = new List<DataShapes.Model.Practitioner>();

                //IRepository<DataShapes.Model.Patient>? patientRepo = RepositoryFactory<DataShapes.Model.Patient>.GetRepository(Constants.IgnorePartition, RepositoryIntent.Testing);
                //IRepository<DataShapes.Model.Encounter>? encounterRepo = RepositoryFactory<DataShapes.Model.Encounter>.GetRepository(Constants.IgnorePartition, RepositoryIntent.Testing);
                //IRepository<DataShapes.Model.Prescription?> scripRepo = RepositoryFactory<DataShapes.Model.Prescription>.GetRepository(Constants.IgnorePartition, RepositoryIntent.Testing);

                DataShapes.Model.Observation observations = new();

                for (var i = 0; i < 100; i++)
                {
                    var data = getFhirData();

                    var parser = new FhirJsonParser();
                    var parsedBundle = parser.Parse<Bundle>(data);

                    if (null != parsedBundle)
                    {
                        var patients = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.Patient>();

                        if (patients != null && patients.Any())
                        {
                            foreach (var patient in patients)
                            {
                                if (patient != null)
                                {
                                    var fhirConverter = AdapterFactory<Hl7.Fhir.Model.Patient, DataShapes.Model.Patient>.GetAdapter(_tenantId, version);
                                    Assert.NotNull(fhirConverter);

                                    var metaPatient = await fhirConverter.Convert(patient);
                                    Assert.NotNull(metaPatient);

                                    try
                                    {
                                        metaPatient.TenantId = _tenantId;
                                        metaPatient.OwnerId = _tenantId;
                                        await _context.AddAsync(metaPatient);
                                        await _context.SaveChangesAsync();
                                    }
                                    catch (NpgsqlException nx)
                                    {
                                        var t = nx.GetType();
                                        continue;
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex);
                                    }

                                    Debug.WriteLine($"processing patient: {metaPatient.Name.FirstName} {metaPatient.Name.FamilyName}");

                                    patientList.Add(metaPatient);

                                    var encounters = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.Encounter>();
                                    Assert.NotNull(encounters);

                                    if (encounters.Any())
                                    {
                                        foreach (var encounter in encounters)
                                        {
                                            Assert.NotNull(encounter);

                                            var id = encounter.Subject.Reference.Substring("urn:uuid:".Length);
                                            if (id != null && id == patient.Id)
                                            {
                                                var fhirEncounterConverter = AdapterFactory<Hl7.Fhir.Model.Encounter, DataShapes.Model.Encounter>.GetAdapter(_tenantId, version);
                                                Assert.NotNull(fhirEncounterConverter);

                                                var metaEncounter = await fhirEncounterConverter.Convert(encounter);
                                                Assert.NotNull(metaEncounter);

                                                try
                                                {
                                                    metaEncounter.TenantId = _tenantId;
                                                    metaEncounter.OwnerId = _tenantId;
                                                    if (!_context.Encounters.Contains(metaEncounter))
                                                    {
                                                        await _context.AddAsync(metaEncounter);
                                                        await _context.SaveChangesAsync();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Debug.WriteLine(ex);
                                                }

                                                encounterList.Add(metaEncounter);

                                                //encounterRepo.CreateRecord(metaEncounter);
                                            }
                                        }
                                    }

                                    /*
                                    var allergies = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.AllergyIntolerance>();
                                    Assert.NotNull(allergies);

                                    var diagnoses = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.DiagnosticReport>();
                                    Assert.NotNull(diagnoses);

                                    var devices = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.Device>();
                                    Assert.NotNull(devices);

                                    foreach (var device in devices)
                                    {
                                        var fhirEncounterConverter = AdapterFactory<Hl7.Fhir.Model.Device, DataShapes.Model.Device>.GetAdapter(_tenantId, version);
                                        Assert.NotNull(fhirEncounterConverter);

                                        var metaEncounter = await fhirEncounterConverter.Convert(device);
                                        Assert.NotNull(device);

                                        metaPatient.Devices.Add(metaEncounter);
                                    }

                                    var supplies = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.SupplyDelivery>();
                                    Assert.NotNull(supplies);

                                    var tests = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.TestReport>();
                                    Assert.NotNull(tests);
                                    */
                                    var scrips = parsedBundle.Entry.ByResourceType<MedicationRequest>();
                                    Assert.NotNull(scrips);

                                    foreach (var scrip in scrips)
                                    {
                                        var fhirScripConverter = AdapterFactory<Hl7.Fhir.Model.MedicationRequest, DataShapes.Model.Prescription>.GetAdapter(_tenantId, version);
                                        Assert.NotNull(fhirScripConverter);

                                        var metaScrip = await fhirScripConverter.Convert(scrip);
                                        Assert.NotNull(scrip);

                                        metaPatient?.Prescriptions?.Add(metaScrip);

                                        if (metaScrip.Medication != null)
                                        {
                                            Debug.WriteLine($"\tProcessing scrip {metaScrip?.Code?.Name} for {metaScrip?.Medication.GenericName}, written on {metaScrip?.WrittenDate}");
                                        }
                                        else
                                        {
                                            Debug.WriteLine($"Missing med for scrip {metaScrip.Code}");
                                        }

                                        try
                                        {
                                            metaScrip.TenantId = _tenantId;
                                            metaScrip.OwnerId = _tenantId;
                                            if (!_context.Prescriptions.Contains(metaScrip))
                                            {
                                                await _context.AddAsync(metaScrip);
                                                await _context.SaveChangesAsync();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Debug.WriteLine(ex);
                                        }

                                        //scripRepo.CreateRecord(metaScrip);
                                    }

                                    //patientRepo.CreateRecord(metaPatient);
                                }
                            }
                        }
                    }
                }

                bool done = true;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public void ProcessDiagnoses()
        {
            try
            { }
            catch
            { }
        }

        [Theory]
        [InlineData(EncounterType.OfficeVisit)]
        [InlineData(EncounterType.BloodWork)]
        [InlineData(EncounterType.A1CChange)]
        [InlineData(EncounterType.Treatment)]
        public void ProcessEvents(EncounterType et)
        {
            try
            {
                var data = getFhirData();

                var e = new DataShapes.Model.Encounter()
                {
                    EncounterType = et,
                    EncounterStatus = EncounterStatus.InProgress,
                    //PatientId = lastPatient?.MyId,
                    CreateDate = lastDate
                };

                lastDate = lastDate.AddDays(7);
            }
            catch
            {
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task ProcessObservations()
        {
            try
            {
                DataShapes.Model.Observation observations = new();

                for (var i = 0; i < 100; i++)
                {
                    var patientId = Guid.Empty;

                    var data = getFhirData();

                    var parser = new FhirJsonParser();
                    var parsedBundle = parser.Parse<Bundle>(data);

                    if (null != parsedBundle)
                    {
                        foreach (var e in parsedBundle.Entry)
                        {
                            if (e.Resource.TypeName == "Observation")
                            {
                                var observation = e.Resource as Hl7.Fhir.Model.Observation;

                                if (observation != null)
                                {
                                    var fhirConverter = AdapterFactory<Hl7.Fhir.Model.Observation, DataShapes.Model.Observation>.GetAdapter(_tenantId, Hl7Version.R4);
                                    var metaObservation = await fhirConverter.Convert(observation);

                                    if (observations.PatientId == Guid.Empty)
                                    {
                                        observations.PatientId = Guid.NewGuid();
                                        observations.PractitionerId = Guid.NewGuid();
                                        //observations.CreateDate = metaObservation.CreateDate;
                                    }

                                    var item = AdapterFactory<Hl7.Fhir.Model.Observation, DataShapes.Model.ObservationItem>.GetAdapter(_tenantId, Hl7Version.R4);
                                    observations.Items.Add(await item.Convert(observation));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public void ReadAndParseFiles()
        {
            try
            {
                Console.OpenStandardOutput();

                for (var i = 0; i < 100; i++)
                {
                    var data = getFhirData();

                    var settings = new ParserSettings()
                    {
                        AcceptUnknownMembers = true
                    };

                    var parser = new FhirJsonParser(settings);
                    var parsedBundle = parser.Parse<Bundle>(data);

                    if (null != parsedBundle)
                    {
                        foreach (var e in parsedBundle.Entry)
                        {
                            if (e.Resource.TypeName == "Patient")
                            {
                                Debug.WriteLine("\n*****\n");
                                var p = e.Resource as Hl7.Fhir.Model.Patient;

                                foreach (var name in p.Name)
                                {
                                    Debug.WriteLine(name);
                                }

                                var aa = AdapterFactory<Hl7.Fhir.Model.Address, DataShapes.Model.Address>.GetAdapter(_tenantId, Hl7Version.R4);

                                if (p.Address != null && p.Address.FirstOrDefault() != null)
                                {
                                    var meta = aa.Convert(p.Address.FirstOrDefault());
                                }
                            }

                            Debug.WriteLine(e.Resource.TypeName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}