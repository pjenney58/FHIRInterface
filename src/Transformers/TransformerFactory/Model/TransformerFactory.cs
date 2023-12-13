
using NLog.Filters;
using TransformerFactory.Interface;

namespace TransformerFactory.Model
{    
    public static class TransformerFactory 
    {
        private static HL7Format format = HL7Format.Fhir;
        private static SourceSystems source = SourceSystems.Epic;
        private static readonly IBaseEventLogger eventLogger = new BaseEventLogger("TransformerFactory");
        private static Guid TenantId = Constants.Transform;

        public static ITransformer Create<IEntity,OEntity>(Guid tenant, Hl7Version version) 
            where OEntity : class, new()
            where IEntity : class, new()
        {
            return Create<IEntity, OEntity>(tenant, HL7Format.Fhir, version, SourceSystems.Epic);
        }

        public static ITransformer Create<IEntity, OEntity>(Guid tenant, 
                                                            HL7Format fmt, 
                                                            Hl7Version version, 
                                                            SourceSystems src)     
            where OEntity : class, new()
            where IEntity : class, new()
        {
            try
            {
                TenantId = tenant;

                // Set Default Values
                if (format != HL7Format.None)
                {
                    format = fmt;
                }

                if (src != SourceSystems.None)
                {
                    source = src;
                }

                // Only two things to do:
                // 1. Determine what the input and outputs are
                // 2. Return the proper adapter
                //
                // is this an N^2 problem?

                var IN = typeof(IEntity);
                var OUT = typeof(OEntity);

                // Observations contain a list of ObservationItems
                if (OUT.Name == "ObservationItem")
                {
                    switch (version)
                    {
                        case Hl7Version.Dstu2:
                            return new Dstu2.ObservationItemAdapter<IEntity, OEntity>(tenant, format, version, source);

                        case Hl7Version.Stu3:
                            return new Stu3.ObservationItemAdapter<IEntity, OEntity>(tenant, format, version, source);

                        case Hl7Version.R4:
                            return new R4.ObservationItemAdapter<IEntity, OEntity>(tenant, format, version, source);

                        default:
                            throw new ArgumentNullException("ObservationItem Handler");
                    }
                }

                if (IN.FullName.ToLower().Contains("fhir"))
                {
                    switch (IN.Name.ToLower())
                    {
                        case "address":
                            switch (version)
                            {
                                case Hl7Version.Dstu2:
                                    return new Dstu2.AddressAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.Stu3:
                                    return new Stu3.AddressAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.R4:
                                    return new R4.AddressTransformer<IEntity, OEntity>(tenant, format, version, source);

                                default:
                                    throw new ArgumentNullException("Address Handler");
                            }

                        case "additionalinfo":
                            switch (version)
                            {
                                case Hl7Version.Dstu2:
                                    return new Dstu2.AdditionalInfoAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.Stu3:
                                    return new Stu3.AdditionalInfoAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.R4:
                                    return new R4.AdditionalInfoAdapter<IEntity, OEntity>(tenant, format, version, source);

                                default:
                                    throw new ArgumentNullException("AdditionalInfo Handler");
                            }

                        case "doseschedule":
                            switch (version)
                            {
                                case Hl7Version.Dstu2:
                                    return new Dstu2.DoseScheduleAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.Stu3:
                                    return new Stu3.DoseScheduleAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.R4:
                                    return new R4.DoseScheduleTransformer<IEntity, OEntity>(tenant, format, version, source);

                                default:
                                    throw new ArgumentNullException("DoseSchedule Handler");
                            }

                        case "encounter":
                            switch (version)
                            {
                                case Hl7Version.Dstu2:
                                    return new Dstu2.EncounterAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.Stu3:
                                    return new Stu3.EncounterAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.R4:
                                    return new R4.EncounterAdapter<IEntity, OEntity>(tenant, format, version, source);

                                default:
                                    throw new ArgumentNullException("Encounter Handler");
                            }

                        case "location":
                            switch (version)
                            {
                                case Hl7Version.Dstu2:
                                    return new Dstu2.LocationAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.Stu3:
                                    return new Stu3.LocationAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.R4:
                                    return new R4.LocationAdapter<IEntity, OEntity>(version);

                                default:
                                    throw new ArgumentNullException("Location Handler");
                            }

                        case "humanname":
                            switch (version)
                            {
                                case Hl7Version.Dstu2:
                                    return new Dstu2.NameAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.Stu3:
                                    return new Stu3.NameAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.R4:
                                    return new R4.NameAdapter<IEntity, OEntity>(tenant, format, version, source);

                                default:
                                    throw new ArgumentNullException("Name Handler");
                            }

                        case "observation":
                            switch (version)
                            {
                                case Hl7Version.Dstu2:
                                    return new Dstu2.ObservationAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.Stu3:
                                    return new Stu3.ObservationAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.R4:
                                    return new R4.ObservationAdapter<IEntity, OEntity>(tenant, format, version, source);

                                default:
                                    throw new ArgumentNullException("Observation Handler");
                            }

                        case "patient":
                            switch (version)
                            {
                                case Hl7Version.Dstu2:
                                    return new Dstu2.PatientAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.Stu3:
                                    return new Stu3.PatientAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.R4:
                                    return new R4.PatientAdapter<IEntity, OEntity>(tenant, format, version, source);

                                default:
                                    throw new ArgumentNullException("Patient Handler");
                            }

                        case "practitioner":
                            switch (version)
                            {
                                case Hl7Version.Dstu2:
                                    return new Dstu2.PractitionerAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.Stu3:
                                    return new Stu3.PractitionerAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.R4:
                                    return new R4.PractitionerAdapter<IEntity, OEntity>(tenant, format, version, source);

                                default:
                                    throw new ArgumentNullException("Practitioner Handler");
                            }

                        case "medicationrequest":
                            switch (version)
                            {
                                case Hl7Version.Dstu2:
                                    return new Dstu2.PrescriptionAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.Stu3:
                                    return new Stu3.PrescriptionAdapter<IEntity, OEntity>(tenant, format, version, source);

                                case Hl7Version.R4:
                                    return new R4.PrescriptionAdapter<IEntity, OEntity>(tenant, format, version, source);

                                default:
                                    throw new ArgumentNullException("Prescription Handler");
                            }

                        // Hl7 v2 using NHapi
                        //case "schedule":
                        //    return new ScheduleAdapter<IEntity, OEntity>(tenant, format, version, source);

                        case "communication":
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    switch (OUT.Name.ToLower())
                    {
                        case "encounter":
                        //return new ScheduleAdapter<IEntity, OEntity>(tenant, format, version, source);

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                eventLogger.ReportWarning($"Failed creating adapter {ex.Message}");
            }

            return default;
        }
    }
}