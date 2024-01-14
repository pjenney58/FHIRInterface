

// https://qnetconfluence.cms.gov/display/ELDF/Documentation+Repository

using PalisaidMeta.Model;
using Location = PalisaidMeta.Model.Location;
using Observation = PalisaidMeta.Model.Observation;
using Patient = PalisaidMeta.Model.Patient;
using Practitioner = PalisaidMeta.Model.Practitioner;
using Task = System.Threading.Tasks.Task;

namespace Transformers.Model
{
    public class ScheduleAdapter<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;

        public delegate OEntity VoidDelegate();
        public delegate Task<OEntity> TaskDelegate();

        public InputVersion version { get; set; }
        public InputFormat format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public ScheduleAdapter(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
        {
            this.tenant = tenant;
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private async Task<OEntity> ConvertR2FhirToMeta()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertR3FhirToMeta()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertFhirToMeta()
        {
            // var p = payloadIN as Hl7.Fhir.Model.{Type}; var o = new PalisaidMeta.Model.{Type}();
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertR5FhirToMeta()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR2Fhir()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR3Fhir()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToFhir()
        {
            // var p = payloadIN as PalisaidMeta.Model.{Type}; var o = new Hl7.Fhir.Model.{Type}();
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToR5Fhir()
        {
            throw new NotImplementedException();
        }

        /***************************
        // EPIC Spec - https://open.epic.com/Scheduling/HL7v2

        *** INBOUND (TO EPIC) ***
         MSH|^~\&|SENDING APP|SENDING FAC|EPIC|EPIC|2005080710350000||SRM^S01|1919|P|2.3||

        The interface receives scheduling information from external systems unsolicited informational messages in either an SIU or SRM format.

        SIU^S12 - Notification of New Appointment Booking and SRM^S01 - Request New Appointment Booking
        Epic receives a message when a new appointment is booked in an external system. In addition to standard visit data,
        a visit number, which was assigned by the external system, is sent to allow the Epic interface to uniquely identify
        the appointment. At this point a new contact is created for that patient. A valid visit type is required for new appointments.

        SIU^S13 - Notification of Appointment Rescheduling and SRM^S02 - Request Appointment Rescheduling
        Epic receives a message when an appointment is rescheduled in an external system. The new start date and time,
        the appointment duration, and other information are provided. Rescheduling an appointment involves changing the
        status of the old appointment to Canceled and creating a new appointment.

        SIU^S14 - Notification of Appointment Modification and SRM^S03 - Request Appointment Modification
        Epic receives a message when modifications are made to appointment status or appointment notes in an external system.
        The change is made to the original appointment contact. If the appointment to update is not found, the interface can
        be set up to book a new appointment based on the information in the message. The PID, PD1, PV1, and PV2 information
        can be updated based on the customer's Epic configuration settings.

        SIU^S15 - Notification of Appointment Cancellation and SRM^S04 - Request Appointment Cancellation
        Epic receives a message when an appointment is canceled in an external system. The status of the appointment changes to Canceled.

        SIU^S17 - Notification of Appointment Deletion and SRM^S06 - Request Appointment Deletion
        Epic receives a message when an appointment is deleted in an external system. The status of the appointment changes to Canceled.

        SIU^S26 Notification that Patient Did Not Show Up for Scheduled Appointment
        Epic receives a message when a scheduled patient does not check in or leaves without being seen for an appointment. The appointment
        contact is given a status of No Show.

        *** QUERY ***
        SQM^S25 Schedule Query Message
        This message is sent to Epic requesting that a schedule query response be sent.

        SQR^S25 Schedule Query Response
        This message is sent in response to an SQM^S25 message. It contains information about scheduled appointments.

        *** OUTBOUND (FROM EPIC)
        SIU^S12 or SRM^S01 New Appointment Booking
        This message is generated by Cadence to inform an external system that a new appointment has been booked. Scheduling a
        new appointment triggers this message.

        SIU^S13 or SRM^S02 Reschedule Appointment
        This event occurs when a Cadence user selects to reschedule an appointment that has just been canceled.

        SIU^S14 or SRM^S03 Appointment Modification
        This message is generated by modifications to appointment status and appointment notes.

        SIU^S15 or SRM^04 Appointment Cancellation
        This message is generated when an appointment is canceled.

        SIU^S26 Notification That Patient Did Not Show for Scheduled Appointment
        This message is generated when a patient does not check in (typically detected by End of Day processing in Cadence)
        or leaves without being seen.

        *****************************/

        /*
        private Tuple<string, string> getMessageType(MSH msh)
        {
            var msgType = msh.MessageType;
            return new Tuple<string, string>(msgType.MessageType.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture),
                                             msgType.TriggerEvent.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture));
        }

        private string messageType(MSH msh)
        {
            var msgType = msh.MessageType;
            return $"{msgType.MessageType.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture)}^{msgType.TriggerEvent.Value.ToUpper(System.Globalization.CultureInfo.InvariantCulture)}";
        }
        
        private async Task<OEntity> ConvertV2_SRM_ToMeta()
        {
            // SRM^S01-S04,S06 - nHAPI only supports SRM^S01, presumably that's enough
            var meta = new PalisaidMeta.Model.Encounter();
            var message = payloadIN as SRM_S01;

            var msh = (MSH)message.GetStructure("MSH");
            var messagekeys = getMessageType(msh);

            switch (messagekeys.Item2)
            {
                case "S01":
                    meta.EncounterStatus = EncounterStatus.New;
                    break;

                case "S02":
                    meta.EncounterStatus = EncounterStatus.Rescheduled;
                    break;

                case "S03":
                    meta.EncounterStatus = EncounterStatus.Changed;
                    break;

                case "S04":
                    meta.EncounterStatus = EncounterStatus.Cancelled;
                    break;

                case "S06":
                    meta.EncounterStatus = EncounterStatus.Deleted;
                    break;

                default:
                    break;
            }

            return default;
        }

        
        // Create a skeleton practitioner
        private Practitioner AddPractitioner(NHapi.Model.V231.Datatype.XCN doc)
        {
            var newdoc = new Practitioner()
            {
                EntityID = Guid.NewGuid(),
                CreateDate = DateTimeOffset.Now,
                LastUpdate = DateTimeOffset.Now,
                PractitionerIdentifier = doc.IDNumber.Value,
            };

            newdoc.Name.Add(new PersonName(new IdentityKey { TenantId = Guid.Empty, OwnerId = newdoc.EntityID })
            {
                FamilyName = doc.FamilyLastName.FamilyName.Value,
                MiddleName = doc.MiddleInitialOrName.Value,
                FirstName = doc.GivenName.Value,
                Title = doc.PrefixEgDR.Value,
                Degree = doc.DegreeEgMD.Value
            });

            return newdoc;
        }

        private Patient AddPatient(NHapi.Model.V231.Datatype.XPN patient, string id)
        {
            var newpat = new Patient()
            {
                EntityID = Guid.NewGuid(),
                CreateDate = DateTimeOffset.Now,
                LastUpdate = DateTimeOffset.Now,
                PrimaryPatientIdString = id,
            };

            newpat.Name = new PersonName(new IdentityKey { TenantId = Guid.Empty, OwnerId = newpat.EntityID })
            {
                FamilyName = patient.FamilyLastName.FamilyName.Value,
                MiddleName = patient.MiddleInitialOrName.Value,
                FirstName = patient.GivenName.Value,
                Title = patient.PrefixEgDR.Value,
                Degree = patient.DegreeEgMD.Value
            };

            return newpat;
        }

        private Location AddLocation(NHapi.Model.V231.Group.SIU_S12_LOCATION_RESOURCE location)
        {
            var loc = new Location()
            {
                EntityID = Guid.NewGuid(),
                CreateDate = DateTimeOffset.Now,
                LastUpdate = DateTimeOffset.Now,
            };

            return loc;
        }

        private async Task<OEntity> ConvertV2_SIU_ToMeta()
        {
            // We could be getting SIU^S12-S15,S17,S26 nHAPI only supports SIU^S12 presumably that's enough

            var meta = new PalisaidMeta.Model.CareEvent();
            var message = payloadIN as SIU_S12;

            var msh = (MSH)message.GetStructure("MSH");
            var messagekeys = getMessageType(msh);

            // Setup to resolve references
            var docrepo = RepositoryFactory<Practitioner>.GetRepository(Constants.IgnorePartition, RepositoryIntent.NoSqlTesting, RepositoryLocus.Container);
            if (docrepo == null)
            {
                throw new NullReferenceException(nameof(docrepo));
            }

            var patrepo = RepositoryFactory<Patient>.GetRepository(Constants.IgnorePartition, RepositoryIntent.NoSqlTesting, RepositoryLocus.Container);
            if (patrepo == null)
            {
                throw new NullReferenceException(nameof(docrepo));
            }

            var obxrepo = RepositoryFactory<Observation>.GetRepository(Constants.IgnorePartition, RepositoryIntent.NoSqlTesting, RepositoryLocus.Container);
            if (obxrepo == null)
            {
                throw new NullReferenceException(nameof(obxrepo));
            }

            var locrepo = RepositoryFactory<Location>.GetRepository(Constants.IgnorePartition, RepositoryIntent.NoSqlTesting, RepositoryLocus.Container);
            if (locrepo == null)
            {
                throw new NullReferenceException(nameof(locrepo));
            }

            if (source == SourceSystems.Epic)
            {
                if (messagekeys.Item1 == "SIU")
                {
                    // Process Schedule Event
                    var sch = (SCH)message.GetStructure("SCH");
                    meta.CareEventTypeString = sch.AppointmentType.Text.Value;
                    meta.CareEventReasonString.Add($"Event:{sch.EventReason.Text.Value}");
                    meta.CareEventReasonString.Add($"Appointment:{sch.AppointmentReason.Text.Value}");
                    meta.CareEventIdString.Add($"Actual:{sch.ScheduleID.Identifier.Value}");
                    meta.CareEventIdString.Add($"Placer:{sch.PlacerAppointmentID.EntityIdentifier.Value}");
                    meta.CareEventIdString.Add($"Filler:{sch.FillerAppointmentID.EntityIdentifier.Value}");

                    // Process practitioners providing service
                    var fillers = sch.GetFillerContactPerson();
                    foreach (var filler in fillers)
                    {
                        var doc = System.Threading.Tasks.Task.Run(() => docrepo.QueryFluent(d => d.PractitionerIdentifier == filler.IDNumber.Value)).Result.FirstOrDefault();
                        if (doc == null)
                        {
                            var newdoc = AddPractitioner(filler);
                            docrepo.CreateRecord(newdoc);
                            meta.Practitioners.Add((Guid)newdoc.EntityID);
                            meta.PractitionerIdString.Add(newdoc.PractitionerIdentifier);
                            newdoc.Dispose();
                        }
                        else
                        {
                            meta.Practitioners.Add((Guid)doc.EntityID);
                            meta.PractitionerIdString.Add(doc.PractitionerIdentifier);
                        }
                    }

                    // Process practitioner that entered the order
                    var enteredby = sch.GetEnteredByPerson();
                    foreach (var enter in enteredby)
                    {
                        var doc = Task.Run(() => docrepo.QueryFluent(d => d.PractitionerIdentifier == enter.IDNumber.Value)).Result.FirstOrDefault();
                        if (doc == null)
                        {
                            var newdoc = AddPractitioner(enter);
                            docrepo.CreateRecord(newdoc);
                            meta.Practitioners.Add((Guid)newdoc.EntityID);
                            meta.PractitionerIdString.Add(newdoc.PractitionerIdentifier);
                            newdoc.Dispose();
                        }
                        else
                        {
                            meta.Practitioners.Add((Guid)doc.EntityID);
                            meta.PractitionerIdString.Add(doc.PractitionerIdentifier);
                        }
                    }

                    // Process the event timing
                    var tq = sch.GetAppointmentTimingQuantity().FirstOrDefault();
                    if (DateTimeOffset.TryParseExact(tq.StartDateTime.TimeOfAnEvent.Value, "yyyyMMddHHmmss", null as IFormatProvider, System.Globalization.DateTimeStyles.AssumeLocal, out DateTimeOffset sd))
                    {
                        meta.StartDate = sd;
                    }

                    if (DateTimeOffset.TryParseExact(tq.EndDateTime.TimeOfAnEvent.Value, "yyyyMMddHHmmss", null as IFormatProvider, System.Globalization.DateTimeStyles.AssumeLocal, out DateTimeOffset ed))
                    {
                        meta.StopDate = ed;
                    }

                    meta.Duration = TimeSpan.Parse(tq.Duration.Value);

                    // Process visit and the providers
                    var pv1 = message.GetPATIENT().PV1;
                    meta.CareEventIdString.Add($"AlternateVisitId:{pv1.AlternateVisitID.ID.Value}");
                    meta.PatientIdString.Add($"Class:{pv1.PatientClass.Value}");

                    var attendingDoc = pv1.GetAttendingDoctor();
                    foreach (var doctor in attendingDoc)
                    {
                        var doc = Task.Run(() => docrepo.QueryFluent(d => d.PractitionerIdentifier == doctor.IDNumber.Value)).Result.FirstOrDefault();
                        if (doc == null)
                        {
                            var newdoc = AddPractitioner(doctor);
                            docrepo.CreateRecord(newdoc);
                            meta.Practitioners.Add((Guid)newdoc.EntityID);
                            meta.PractitionerIdString.Add(newdoc.PractitionerIdentifier);
                            newdoc.Dispose();
                        }
                        else
                        {
                            meta.Practitioners.Add((Guid)doc.EntityID);
                            meta.PractitionerIdString.Add(doc.PractitionerIdentifier);
                        }
                        //meta.PractitionerIdString.Add($"Referring:{doc.IDNumber} {doc.FamilyLastName.FamilyName.Value}, {doc.GivenName.Value}, {doc.MiddleInitialOrName.Value} {doc.SuffixEgJRorIII.Value}");
                    }

                    var referringDoc = pv1.GetReferringDoctor();
                    foreach (var doctor in referringDoc)
                    {
                        var doc = Task.Run(() => docrepo.QueryFluent(d => d.PractitionerIdentifier == doctor.IDNumber.Value)).Result.FirstOrDefault();
                        if (doc == null)
                        {
                            var newdoc = AddPractitioner(doctor);
                            docrepo.CreateRecord(newdoc);
                            meta.ReferringPractitioners.Add((Guid)newdoc.EntityID);
                            meta.ReferringPractitionerIdString.Add(newdoc.PractitionerIdentifier);
                            newdoc.Dispose();
                        }
                        else
                        {
                            meta.ReferringPractitioners.Add((Guid)doc.EntityID);
                            meta.ReferringPractitionerIdString.Add(doc.PractitionerIdentifier);
                        }
                        //meta.PractitionerIdString.Add($"Referring:{doc.IDNumber} {doc.FamilyLastName.FamilyName.Value}, {doc.GivenName.Value}, {doc.MiddleInitialOrName.Value} {doc.SuffixEgJRorIII.Value}");
                    }

                    // Process Patient
                    var pid = message.GetPATIENT().PID;
                    meta.PatientIdString.Add($"Id:{pid.PatientID.ID}");
                    meta.PatientIdString.Add($"Account:{pid.PatientAccountNumber.ID.Value}");

                    var names = pid.GetPatientName();
                    foreach (var name in names)
                    {
                        var pat = Task.Run(() => patrepo.QueryFluent(d => d.PrimaryPatientIdString == pid.PatientID.ID.Value)).Result.FirstOrDefault();
                        if (pat == null)
                        {
                            var newpat = AddPatient(name, pid.PatientID.ID.Value);
                            newpat.PatientClass = pv1.PatientClass.Value;

                            patrepo.CreateRecord(newpat);

                            meta.Patients.Add((Guid)newpat.EntityID);
                            meta.PatientIdString.Add(newpat.PrimaryPatientIdString);
                            newpat.Dispose();
                        }
                        else
                        {
                            meta.Patients.Add((Guid)pat.EntityID);
                            meta.PatientIdString.Add(pat.PrimaryPatientIdString);
                        }
                    }

                    // Process Observations
                    foreach (var obx in message.GetPATIENT().OBXs)
                    {
                        var metaobx = new Observation(new IdentityKey { OwnerId = meta.EntityID, TenantId = Guid.Empty })
                        {
                            EntityID = Guid.NewGuid(),
                            CreateDate = DateTimeOffset.Now,
                            LastUpdate = DateTimeOffset.Now,
                            IsActive = true,
                            IsDeleted = false,
                        };

                        if (DateTimeOffset.TryParseExact(tq.StartDateTime.TimeOfAnEvent.Value, "yyyyMMddHHmmss", null as IFormatProvider, System.Globalization.DateTimeStyles.AssumeLocal, out DateTimeOffset start))
                        {
                            metaobx.StartDate = sd;
                        }

                        metaobx.PatientId = meta.Patients.FirstOrDefault();
                        metaobx.PractitionerId = meta.Practitioners.FirstOrDefault();
                        metaobx.Items.Add(new ObservationItem(new IdentityKey { TenantId = metaobx.TenantId, OwnerId = metaobx.EntityID })
                        {
                            ObservationType = ObservationType.Note,
                            Description = obx.ObservationSubID.Description.ToString(),
                        });

                        obxrepo.CreateRecord(metaobx);
                    }

                    // Process Location
                    //var locations = message.GetRESOURCES().LOCATION_RESOURCEs;
                    foreach (var location in message.GetRESOURCES().LOCATION_RESOURCEs)
                    {
                        var loc = Task.Run(() => locrepo.QueryFluent(d => d.Name == location.AIL.LocationResourceID.PointOfCare.Value)).Result.FirstOrDefault();
                        if (loc == null)
                        {
                            var newloc = AddLocation(location);
                            locrepo.CreateRecord(newloc);
                            meta.Locations.Add((Guid)newloc.EntityID);
                            meta.LocationIdString.Add(newloc.Name);
                            newloc.Dispose();
                        }
                        else
                        {
                            meta.Locations.Add((Guid)loc.EntityID);
                            meta.LocationIdString.Add(loc.Name);
                        }

                        meta.LocationIdString.Add($"PointOfCare:{location.AIL.LocationResourceID.PointOfCare.Value}");
                        meta.LocationIdString.Add($"Facility:{location.AIL.LocationResourceID.Facility.NamespaceID.Value}");
                        meta.LocationIdString.Add($"Building:{location.AIL.LocationResourceID.Building.Value}");
                        meta.LocationIdString.Add($"Floor:{location.AIL.LocationResourceID.Floor.Value}");
                        meta.LocationIdString.Add($"Room:{location.AIL.LocationResourceID.Room.Value}");
                        meta.LocationIdString.Add($"Bed:{location.AIL.LocationResourceID.Bed.Value}");
                        meta.LocationIdString.Add($"Type:{location.AIL.LocationResourceID.PersonLocationType.Value}");
                    }

                    switch (messagekeys.Item2)
                    {
                        // New Appointment
                        case "S12":
                            meta.EncounterStatus = EncounterStatus.New;
                            break;

                        case "S13":
                            meta.EncounterStatus = EncounterStatus.Rescheduled;
                            break;

                        case "S14":
                            meta.EncounterStatus = EncounterStatus.Changed;
                            break;

                        case "S15":
                            meta.EncounterStatus = EncounterStatus.Cancelled;
                            break;

                        case "S17":
                            meta.EncounterStatus = EncounterStatus.Deleted;
                            break;

                        case "S26":
                            meta.EncounterStatus = EncounterStatus.Noshow;
                            break;

                        default:
                            break;
                    }
                }

                return meta as OEntity;
            }
            else
            {
                return default;
            }

            throw new InvalidDataException($"Expected SIU or SRM, Got {messagekeys.Item1}^{messagekeys.Item2}");
        }
        */
        private async Task<OEntity> ConvertMeta_ToV2_SIU()
        {
            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMeta_ToV2_SRM()
        {
            throw new NotImplementedException();
        }

        public async Task<OEntity> Convert(IEntity payload)
        {
           
            Dictionary<string, TaskDelegate> jumpTable = new Dictionary<string, TaskDelegate>()
            {
                //{ @"SIU_S12/CareEvent", ConvertV2_SIU_ToMeta },
                //{ @"SIU_S13/CareEvent", ConvertV2_SIU_ToMeta },
               // { @"SIU_S14/CareEvent", ConvertV2_SIU_ToMeta },
                //{ @"SIU_S15/CareEvent", ConvertV2_SIU_ToMeta },
                //{ @"SIU_S17/CareEvent", ConvertV2_SIU_ToMeta },
                //{ @"SIU_S26/CareEvent", ConvertV2_SIU_ToMeta },
                //{ @"SRM_S01/CareEvent", ConvertV2_SRM_ToMeta },
                //{ @"SRM_S02/CareEvent", ConvertV2_SRM_ToMeta },
                //{ @"SRM_S03/CareEvent", ConvertV2_SRM_ToMeta },
                //{ @"SRM_S04/CareEvent", ConvertV2_SRM_ToMeta },
                //{ @"SRM_S06/CareEvent", ConvertV2_SRM_ToMeta },
                { @"CareEvent/SIU_S12", ConvertMeta_ToV2_SIU },
                { @"CareEvent/SIU_S13", ConvertMeta_ToV2_SIU },
                { @"CareEvent/SIU_S14", ConvertMeta_ToV2_SIU },
                { @"CareEvent/SIU_S15", ConvertMeta_ToV2_SIU },
                { @"CareEvent/SIU_S17", ConvertMeta_ToV2_SIU },
                { @"CareEvent/SIU_S26", ConvertMeta_ToV2_SIU },
                { @"CareEvent/SRM_S01", ConvertMeta_ToV2_SRM },
                { @"CareEvent/SRM_S02", ConvertMeta_ToV2_SRM },
                { @"CareEvent/SRM_S03", ConvertMeta_ToV2_SRM },
                { @"CareEvent/SRM_S04", ConvertMeta_ToV2_SRM },
                { @"CareEvent/SRM_S06", ConvertMeta_ToV2_SRM }
            };

            payloadIN = payload as IEntity;

            var jumpkey = $"{typeof(IEntity).Name}/{typeof(OEntity).Name}";
            if (jumpTable.TryGetValue(jumpkey, out TaskDelegate funcC))
            {
                return await funcC();
            }

            return default;
        }

        public IEnumerable<OEntity> CollectOEntityItemListItem()
        {
            throw new NotImplementedException();
        }

        public Task<object?> Transform(object data)
        {
            throw new NotImplementedException();
        }
    }
}