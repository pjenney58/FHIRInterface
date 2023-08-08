/*
 MIT License - AdapterFactory.cs

Copyright (c) 2021 - Present by Sand Drift Software, LLC
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using Microsoft.EntityFrameworkCore.Metadata;

namespace Hl7Harmonizer.Adapters.Model
{
    /// <summary>
    /// Returns an HL7 version normalized adapter of type IEntity to OEntity
    /// </summary>
    public class AdapterFactory<IEntity, OEntity>
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private static HL7Format format = HL7Format.Fhir;
        private static SourceSystems source = SourceSystems.Epic;
        private static readonly IBaseEventLogger eventLogger = new BaseEventLogger("AdapterFactory");
        private static Guid TenantId = Constants.Transform;

        public static IAdapter<IEntity, OEntity> GetAdapter(Guid tenant, HL7Format fmt, Hl7Version version, SourceSystems src)
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
                    return new ObservationItemAdapter<IEntity, OEntity>(tenant, format, version, source);
                }

                if (IN.FullName.ToLower().Contains("fhir"))
                {
                    switch (IN.Name.ToLower())
                    {
                        case "address":
                            return new AddressAdapter<IEntity, OEntity>(tenant, format, version, source);

                        case "additionalinfo":
                            return new AdditionalInfoAdapter<IEntity, OEntity>(tenant, format, version, source);

                        case "doseschedule":
                            return new DoseScheduleAdapter<IEntity, OEntity>(tenant, format, version, source);

                        // TODO: Need to be able to swap namespaces in code - current documentation for PackageReference/Alias does not work
                        case "encounter":
                            switch (version)
                            {
                                case Hl7Version.R4:
                                    return new EncounterR4Adapter<IEntity, OEntity>(tenant, format, version, source);

                                default:
                                    return new EncounterAdapter<IEntity, OEntity>(tenant, format, version, source);
                            }

                        case "location":
                            return new LocationAdapter<IEntity, OEntity>(version);

                        case "humanname":
                            return new NameAdapter<IEntity, OEntity>(tenant, format, version, source);

                        case "observation":
                            return new ObservationAdapter<IEntity, OEntity>(tenant, format, version, source);

                        case "patient":
                            return new PatientAdapter<IEntity, OEntity>(tenant, format, version, source);

                        case "practitioner":
                            return new PractitionerAdapter<IEntity, OEntity>(tenant, format, version, source);

                        case "prescription":
                            return new PrescriptionAdapter<IEntity, OEntity>(tenant, format, version, source);

                        case "schedule":
                            return new ScheduleAdapter<IEntity, OEntity>(tenant, format, version, source);

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
                        case "careevent":
                            return new ScheduleAdapter<IEntity, OEntity>(tenant, format, version, source);

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

        public static IAdapter<IEntity, OEntity> GetAdapter(Guid tenant, Hl7Version version)
        {
            return GetAdapter(tenant, HL7Format.None, version, SourceSystems.None);
        }
    }
}