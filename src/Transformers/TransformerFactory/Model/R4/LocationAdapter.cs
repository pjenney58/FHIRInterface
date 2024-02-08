/*
 MIT License - LocationAdapter.cs

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
using Transformers.Interface;
using Support;

namespace Transformers.Model.R4
{
    public class LocationTransformer<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;
        private OEntity? payloadOUT;

        public delegate OEntity VoidDelegate();
        public delegate Task<OEntity> TaskDelegate();

        public InputVersion version { get; set; }
        public InputFormat format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        private readonly IBaseEventLogger logger = new BaseEventLogger(nameof(LocationTransformer<IEntity, OEntity>));

        public LocationTransformer(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
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
            logger.ReportInfo("Hl7.Fhir.Model.Location to PalisaidMeta.Model.Location");
            
            var fhir = payloadIN as Hl7.Fhir.Model.Location;
            if (fhir == null)
            {
                throw new ArgumentNullException(logger.ReportError("FHIR payload is null",false));
            }

            var meta = new PalisaidMeta.Model.Location()
            {
                TenantId = tenant,
                EntityId = Guid.Parse(fhir.Id),
                CreateDate = DateTimeOffset.Now,
                LastUpdate = DateTimeOffset.Now,
                LocationType = LocationType.Clinic
            };

            if (meta == null)
            {
                throw new ArgumentNullException(logger.ReportError("Meta payload is null",false));
            }

            meta.Name = fhir.Name;
            meta.Description = fhir.Description;

            if (fhir.ManagingOrganization != null)
            {
                meta.OwnerId = Guid.Parse(fhir.ManagingOrganization.Identifier.Value);
            }

            try
            {
                // Known addresses
                var addressTransformer = TransformerFactory.Create<Hl7.Fhir.Model.Address, PalisaidMeta.Model.Address>(tenant, format, version, source);
                var address = await addressTransformer.Transform(fhir.Address);
                if (address != null)
                {
                    meta.Addresses?.Add(await addressTransformer.Transform(fhir.Address) as Address ?? new Address(){ Address1 = "Bogus" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.ReportDebug($"Location Adapter/Address Exception: {ex.Message}");
            }

            return meta as OEntity ?? throw new ArgumentNullException(logger.ReportError("meta is null",false));
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

        public LocationTransformer(InputVersion version)
        {
            this.version = version;
        }

        private async Task<OEntity> ConvertV2_MSG_ToMeta()
        {
            // var meta = new PalisaidMeta.Model.{Type}(); var message = payloadIN as NHapi.Model.{Version}.Message.{MSG};

            throw new NotImplementedException();
        }

        private async Task<OEntity> ConvertMetaToV2_MSG()
        {
            // var meta = new PalisaidMeta.Model.{Type}(); var message = payloadIN as NHapi.Model.{Version}.Message.{MSG};
            throw new NotImplementedException();
        }

        public async Task<object?> Transform(object payload)
        {
            // Override this with the appropriate key conditions - replace MSG as desired. There may
            // be several similar messages required, e.g. SIU & SRM Override this with the
            // appropriate key conditions - replace MSG as desired. There may be several similar
            // messages required, e.g. SIU & SRM
            Dictionary<Tuple<string, InputVersion>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.Location => PalisaidMeta.Model.Location", InputVersion.HL7FhirDstu2), ConvertR5FhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model.Location => Hl7.Fhir.Model.Location", InputVersion.HL7FhirDstu2), ConvertMetaToR5Fhir },
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.Location => PalisaidMeta.Model.Location", InputVersion.HL7HhirStu3), ConvertR3FhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model.Location => Hl7.Fhir.Model.Location", InputVersion.HL7HhirStu3), ConvertMetaToR3Fhir },
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.Location => PalisaidMeta.Model.Location", InputVersion.HL7FhirR4), ConvertFhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model.Location => Hl7.Fhir.Model.Location", InputVersion.HL7FhirR4), ConvertMetaToFhir },
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.Location => PalisaidMeta.Model.Location", InputVersion.HL7FhirR5), ConvertR5FhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model.Location => Hl7.Fhir.Model.Location", InputVersion.HL7FhirR5), ConvertMetaToR5Fhir }
            };

            payloadIN = payload as IEntity;

            var jumpkey = new Tuple<string, InputVersion>($"{typeof(IEntity).FullName} => {typeof(OEntity).FullName}", version);
            if (jumpTable.TryGetValue(jumpkey, out TaskDelegate? funcC))
            {
                return await funcC();
            }

            return default;
        }

        public IEnumerable<OEntity> CollectOEntityItemListItem()
        {
            throw new NotImplementedException();
        }
    }
}