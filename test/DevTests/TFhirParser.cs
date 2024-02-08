using System.Diagnostics;
using PalisaidMeta.Model;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Npgsql;
using RandomDataGenerator;
using Microsoft.EntityFrameworkCore;


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
        private const int MAX_VALS = 50;

        private readonly string sourcedir = "../../../../../data/test/Patients_fhir_0fded401-29da-4937-887f-24b9a446194d";

        private PalisaidMetaContext _context;
        private Guid _tenantId = Guid.NewGuid();

        public TFhirParser()
        {
            _context = new PalisaidMetaContext();
            if (_context == null)
            {
                throw new Exception("Context is null");
            }

            _context.Database.Migrate();
            _context.Database.EnsureCreated();

            
            if (Directory.Exists(sourcedir) == false)
            {
                throw new Exception("Directory not found");
            }

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
        private PalisaidMeta.Model.Patient lastPatient;
        private PalisaidMeta.Model.Practitioner lastPractitioner;

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

        private async Task<PalisaidMeta.Model.Patient?> GetRandomMetaPatient()
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
                            var fhirConverter = TransformerFactory.Create<Hl7.Fhir.Model.Patient, PalisaidMeta.Model.Patient>(Guid.NewGuid(), InputVersion.HL7FhirR4);
                            var metaPatient = await fhirConverter.Transform(patient) as PalisaidMeta.Model.Patient;
                            lastPatient = metaPatient;
                            return lastPatient;
                        }
                    }
                }
            }

            return default;
        }

        private async Task<PalisaidMeta.Model.Practitioner?> GetRandomMetaPractitioner()
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
                            var fhirConverter = TransformerFactory.Create<Hl7.Fhir.Model.Practitioner, PalisaidMeta.Model.Practitioner>(Guid.NewGuid(), InputVersion.HL7FhirR4);
                            var metaPractitioner = await fhirConverter.Transform(practitioner) as PalisaidMeta.Model.Practitioner;
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
        [InlineData(InputVersion.HL7FhirR4)]
        public async System.Threading.Tasks.Task ProcessPrescriptions(InputVersion version)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    // Arrange
                    var data = getFhirData();
                    Assert.NotNull(data);

                    foreach (var file in files)
                    {
                        //var data = getFhirData();
                        //Assert.NotNull(data);

                        var parser = new FhirJsonParser();
                        Assert.NotNull(parser);

                        var parsedBundle = parser.Parse<Bundle>(File.ReadAllText(file));
                        Assert.NotNull(parsedBundle);

                        // Act
                        if (parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.MedicationRequest>().Any())
                        {
                            var prescriptions = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.MedicationRequest>();

                            // Assert
                            Assert.NotNull(prescriptions);

                            if (!prescriptions.Any())
                            {
                                continue;
                            }

                            foreach (var prescription in prescriptions)
                            {
                                var fhirConverter = TransformerFactory.Create<Hl7.Fhir.Model.MedicationRequest, PalisaidMeta.Model.Prescription>(_tenantId, version);
                                Assert.NotNull(fhirConverter);

                                var metaPrescription = await fhirConverter.Transform(prescription) as PalisaidMeta.Model.Prescription;
                                Assert.NotNull(metaPrescription);

                                // Persist
                                try
                                {
                                    await _context.AddAsync(metaPrescription);
                                    await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException cx)
                                {
                                    var t = cx.GetType();
                                    Debug.WriteLine(cx);
                                    continue;
                                }
                                catch (NpgsqlException nx)
                                {
                                    var t = nx.GetType();
                                    Debug.WriteLine(nx);
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    if (ex.InnerException.Message.Contains("duplicate key value violates unique constraint"))
                                    {
                                        continue;
                                    }

                                    Debug.WriteLine(ex);
                                    continue;
                                }
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
        [InlineData(InputVersion.HL7FhirR4)]
        public async System.Threading.Tasks.Task ProcessTreatments(InputVersion version)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    // Arrange
                    for (var i = 0; i < MAX_VALS; i++)
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
                                var fhirConverter = TransformerFactory.Create<Hl7.Fhir.Model.MedicationRequest, PalisaidMeta.Model.Prescription>(_tenantId, version);
                                Assert.NotNull(fhirConverter);

                                var metaPrescription = await fhirConverter.Transform(prescription) as PalisaidMeta.Model.Prescription;
                                Assert.NotNull(metaPrescription);

                                // Persist
                                try
                                {
                                    await _context.AddAsync(metaPrescription);
                                    await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException cx)
                                {
                                    var t = cx.GetType();
                                    Debug.WriteLine(cx.Message);
                                    continue;
                                }
                                catch (NpgsqlException nx)
                                {
                                    var t = nx.GetType();
                                    Debug.WriteLine(nx.Message);
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    if (ex.InnerException.Message.Contains("duplicate key value violates unique constraint"))
                                    {
                                        continue;
                                    }

                                    Debug.WriteLine(ex.Message);
                                    continue;
                                }
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
        [InlineData(InputVersion.HL7FhirR4)]
        public async System.Threading.Tasks.Task ProcessLocations(InputVersion version)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    // Arrange

                    var data = getFhirData();
                    Assert.NotNull(data);

                    foreach (var file in files)
                    {
                        var parser = new FhirJsonParser();
                        Assert.NotNull(parser);

                        var parsedBundle = parser.Parse<Bundle>(File.ReadAllText(file));
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
                                var fhirConverter = TransformerFactory.Create<Hl7.Fhir.Model.Location, PalisaidMeta.Model.Location>(_tenantId, version);
                                Assert.NotNull(fhirConverter);

                                var metaLocation = await fhirConverter.Transform(location);
                                Assert.True(metaLocation != null);

                                // Persist
                                try
                                {
                                    await _context.AddAsync(metaLocation);
                                    await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException cx)
                                {
                                    var t = cx.GetType();
                                    Debug.WriteLine(cx.Message);
                                    continue;
                                }
                                catch (NpgsqlException nx)
                                {
                                    var t = nx.GetType();
                                    Debug.WriteLine(nx.Message);
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    if (ex.InnerException.Message.Contains("duplicate key value violates unique constraint"))
                                    {
                                        continue;
                                    }

                                    Debug.WriteLine(ex.Message);
                                    continue;
                                }
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
        [InlineData(InputVersion.HL7FhirR4)]
        public async System.Threading.Tasks.Task ProcessEncounters(InputVersion version)
        {
            var data = getFhirData();

            foreach (var file in files)
            {
                var parser = new FhirJsonParser();
                Assert.NotNull(parser);

                var parsedBundle = parser.Parse<Bundle>(File.ReadAllText(file));
                var encounters = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.Encounter>();
                Assert.NotNull(encounters);

                if (encounters.Any())
                {
                    foreach (var encounter in encounters)
                    {
                        Assert.NotNull(encounter);

                        var id = encounter.Subject.Reference.Substring("urn:uuid:".Length);

                        var fhirEncounterConverter = TransformerFactory.Create<Hl7.Fhir.Model.Encounter, PalisaidMeta.Model.Encounter>(_tenantId, version);
                        Assert.NotNull(fhirEncounterConverter);

                        var metaEncounter = await fhirEncounterConverter.Transform(encounter) as PalisaidMeta.Model.Encounter;
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
                        catch (DbUpdateConcurrencyException cx)
                        {
                            var t = cx.GetType();
                            Debug.WriteLine(cx.Message);
                            continue;
                        }
                        catch (NpgsqlException nx)
                        {
                            var t = nx.GetType();
                            Debug.WriteLine(nx.Message);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException.Message.Contains("duplicate key value violates unique constraint"))
                            {
                                Debug.WriteLine($"Dup: {metaEncounter.EncounterIdString}");
                                continue;
                            }
                            Debug.WriteLine(ex.Message);
                        }

                    }
                }

            }
        }

        [Theory]
        //[InlineData(Hl7Version.Stu3)]
        //[InlineData(Hl7Version.R4b)]
        //[InlineData(Hl7Version.R5)]
        [InlineData(InputVersion.HL7FhirR4)]
        public async System.Threading.Tasks.Task ProcessPatients(InputVersion version)
        {
            try
            {
                var encounterList = new List<PalisaidMeta.Model.Encounter>();
                var patientList = new List<PalisaidMeta.Model.Patient>();
                var locationList = new List<PalisaidMeta.Model.Location>();
                var practionerList = new List<PalisaidMeta.Model.Practitioner>();
                var observationList = new List<PalisaidMeta.Model.Observation>();


                var data = getFhirData();

                foreach (var file in files)
                {
                    var parser = new FhirJsonParser();
                    Assert.NotNull(parser);

                    var parsedBundle = parser.Parse<Bundle>(File.ReadAllText(file));

                    if (null != parsedBundle)
                    {
                        var patients = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.Patient>();

                        if (patients != null && patients.Any())
                        {
                            foreach (var patient in patients)
                            {
                                if (patient != null)
                                {
                                    var fhirConverter = TransformerFactory.Create<Hl7.Fhir.Model.Patient, PalisaidMeta.Model.Patient>(_tenantId, version);
                                    Assert.NotNull(fhirConverter);

                                    var metaPatient = await fhirConverter.Transform(patient) as PalisaidMeta.Model.Patient;
                                    Assert.NotNull(metaPatient);

                                    if (_context.Patients.Contains(metaPatient))
                                    {
                                        continue;
                                    }

                                    Debug.WriteLine($"processing patient: {metaPatient.Name.FirstName} {metaPatient.Name.FamilyName}");

                                    patientList.Add(metaPatient);

                                    var observations = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.Observation>();
                                    Assert.NotNull(observations);

                                    // TODO: Pete => NEED TO FILTER BY PATIENT ID
                                    if (observations.Any())
                                    {
                                        foreach (var observation in observations)
                                        {
                                            var fhirObservationConverter = TransformerFactory.Create<Hl7.Fhir.Model.Observation, PalisaidMeta.Model.Observation>(_tenantId, version);
                                            Assert.NotNull(fhirObservationConverter);

                                            var metaObservation = await fhirObservationConverter.Transform(observation) as PalisaidMeta.Model.Observation;
                                            Assert.NotNull(metaObservation);

                                            try
                                            {
                                                metaObservation.TenantId = _tenantId;
                                                metaObservation.OwnerId = metaPatient.EntityId;
                                                metaObservation.PatientId = metaPatient.EntityId;
                                                metaPatient.Observations.Add(metaObservation);
                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.WriteLine(ex);
                                            }

                                            observationList.Add(metaObservation);
                                        }
                                    }



                                    /*
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
                                                var fhirEncounterConverter = TransformerFactory.Create<Hl7.Fhir.Model.Encounter, PalisaidMeta.Model.Encounter>(_tenantId, version);
                                                Assert.NotNull(fhirEncounterConverter);

                                                var metaEncounter = await fhirEncounterConverter.Transform(encounter) as PalisaidMeta.Model.Encounter;
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
                                    */
                                    /*
                                    var allergies = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.AllergyIntolerance>();
                                    Assert.NotNull(allergies);

                                    var diagnoses = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.DiagnosticReport>();
                                    Assert.NotNull(diagnoses);

                                    var devices = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.Device>();
                                    Assert.NotNull(devices);

                                    foreach (var device in devices)
                                    {
                                        var fhirEncounterConverter = TransformerFactory<Hl7.Fhir.Model.Device, PalisaidMeta.Model.Device>.GetTransformer(_tenantId, version);
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
                                    /*var scrips = parsedBundle.Entry.ByResourceType<MedicationRequest>();
                                    Assert.NotNull(scrips);

                                    foreach (var scrip in scrips)
                                    {
                                        var fhirScripConverter = TransformerFactory.Create<Hl7.Fhir.Model.MedicationRequest, PalisaidMeta.Model.Prescription>(_tenantId, version);
                                        Assert.NotNull(fhirScripConverter);

                                        var metaScrip = await fhirScripConverter.Transform(scrip) as PalisaidMeta.Model.Prescription;
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
                                    }
                                */
                                    // Persist
                                    try
                                    {
                                        await _context.AddAsync(metaPatient);
                                        await _context.SaveChangesAsync();
                                    }
                                    catch (DbUpdateConcurrencyException cx)
                                    {
                                        var t = cx.GetType();
                                        Debug.WriteLine(cx.Message);
                                        continue;
                                    }
                                    catch (NpgsqlException nx)
                                    {
                                        var t = nx.GetType();
                                        Debug.WriteLine(nx.Message);
                                        continue;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.InnerException.Message.Contains("duplicate key value violates unique constraint"))
                                        {
                                            Debug.WriteLine($"Dup: {metaPatient.Name.FirstName} {metaPatient.Name.FamilyName}");
                                            continue;
                                        }
                                        Debug.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
                // }

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

                var e = new PalisaidMeta.Model.Encounter()
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
                var data = getFhirData();

                foreach (var file in files)
                {
                    var parser = new FhirJsonParser();
                    var parsedBundle = parser.Parse<Bundle>(File.ReadAllText(file));

                    Assert.NotNull(parsedBundle);

                    var observations = parsedBundle.Entry.ByResourceType<Hl7.Fhir.Model.Observation>();
                    Assert.NotNull(observations);


                    foreach (var observation in observations)
                    {
                        Assert.NotNull(observation);

                        var fhirConverter = TransformerFactory.Create<Hl7.Fhir.Model.Observation, PalisaidMeta.Model.Observation>(_tenantId, InputVersion.HL7FhirR4);
                        var metaObservation = await fhirConverter.Transform(observation);

                        // Persist
                        try
                        {
                            await _context.AddAsync(metaObservation);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
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

                for (var i = 0; i < MAX_VALS; i++)
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

                                var aa = TransformerFactory.Create<Hl7.Fhir.Model.Address, PalisaidMeta.Model.Address>(_tenantId, InputVersion.HL7FhirR4);

                                if (p.Address != null && p.Address.FirstOrDefault() != null)
                                {
                                    var meta = aa.Transform(p.Address.FirstOrDefault());
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