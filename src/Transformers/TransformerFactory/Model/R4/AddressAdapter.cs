/*
 MIT License - AddressAdapter.cs

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

//extern alias r5;
//extern alias r4;
//extern alias r4b;
//extern alias stu3;
//extern alias dstu2;

using Hl7.Fhir.Model;

//using R5 = r5::Hl7.Fhir.Model;
//using R4 = r4::Hl7.Fhir.Model;
//using R4b = r4b::Hl7.Fhir.Model;
//using Stu3 = stu3::Hl7.Fhir.Model;
//using Dstu2 = dstu2::Hl7.Fhir.Model;

using DataShapes.Model;
using Task = System.Threading.Tasks.Task;
using Transformers.Interface;

namespace Transformers.Model.R4
{
    public class AddressTransformer<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;
        private OEntity? payloadOUT;

        public delegate OEntity VoidDelegate();
        public delegate Task<OEntity?> TaskDelegate();

        public Hl7Version version { get; set; }
        public HL7Format format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public AddressTransformer(Guid tenant, HL7Format format, Hl7Version version, SourceSystems source)
        {
            this.tenant = tenant;
            this.format = format;
            this.version = version;
            this.source = source;
        }

        private async Task<OEntity?> ConvertFhirToMeta()
        {
            var fhir = payloadIN as Hl7.Fhir.Model.Address;
            if (fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }

            var meta = new DataShapes.Model.Address();
            if (meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            meta.TenantId = this.tenant;
            meta.EntityId = fhir.ElementId == null
               ? Guid.NewGuid()
               : Guid.Parse(fhir.ElementId);

            await Task.Run(() =>
            {
                if (fhir.Line != null)
                {
                    if (fhir.Line.Count() > 0 && fhir.Line.ElementAt(0) != null)
                    {
                        meta.Address1 = fhir.Line.ElementAt(0);
                    }

                    if (fhir.Line.Count() > 1 && fhir.Line.ElementAt(1) != null)
                    {
                        meta.Address2 = fhir.Line.ElementAt(1);
                    }

                    if (fhir.Line.Count() >= 2 && fhir.Line.ElementAt(1) != null)
                    {
                        meta.Address3 = fhir.Line.ElementAt(2);
                    }
                }

                meta.City = fhir.City;
                meta.State = fhir.State;
                meta.PostalCode = fhir.PostalCode;
                meta.Country = fhir.Country;
                meta.District = fhir.District;

                if (fhir.Period != null)
                {
                    meta.StartDate = DateTime.Parse(fhir.Period.Start);
                    meta.StopDate = DateTime.Parse(fhir.Period.End);
                }
            });

            return meta as OEntity;
        }


        private async Task<OEntity?> ConvertMetaToFhir()
        {
            
            var fhir = new Hl7.Fhir.Model.Address();
        
            if (fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }

            var meta = new DataShapes.Model.Address();
            if (meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            fhir.ElementId = meta.EntityId.ToString();

            await Task.Run(() =>
            {
                var lines = new List<string>()
                {
                    meta.Address1,
                    meta.Address2,
                    meta.Address3
                };

                fhir.Line = lines as IEnumerable<string>;
                fhir.City = meta.City;
                fhir.State = meta.State;
                fhir.PostalCode = meta.PostalCode;
                fhir.Country = meta.Country;
                fhir.District = meta.District;

                if (meta.StartDate != DateTime.MinValue)
                {
                    fhir.Period = new();
                    fhir.Period.Start = meta.StartDate.ToString("yyyyMMdd");
                    fhir.Period.End = meta.StopDate.ToString("yyyyMMdd");
                }
            });

            return meta as OEntity;
        }


        public async Task<object?> Transform(object payload)
        {
            payloadIN = payload as IEntity;

            // Use full names to differenciate
            Dictionary<Tuple<string, Hl7Version>, TaskDelegate> jumpTable = new()
            {
                { new Tuple<string, Hl7Version>(@"Hl7.Fhir.Model.Address => DataShapes.Model.Address", Hl7Version.R4), ConvertFhirToMeta },
                { new Tuple<string, Hl7Version>(@"DataShapes.Model.Address => Hl7.Fhir.Model.Address", Hl7Version.R4), ConvertMetaToFhir }            
            };

            var jumpkey = new Tuple<string, Hl7Version>($"{typeof(IEntity).FullName} => {typeof(OEntity).FullName}", version);

            if (jumpTable.TryGetValue(jumpkey, out TaskDelegate? funcC))
            {
                return await funcC();
            }

            return default;
        }
    }
}