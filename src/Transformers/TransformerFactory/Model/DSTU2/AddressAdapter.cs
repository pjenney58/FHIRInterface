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

using PalisaidMeta.Model;
using Transformers.Interface;
using Task = System.Threading.Tasks.Task;


namespace Transformers.Model.Dstu2
{
    public class AddressAdapter<IEntity, OEntity> : ITransformer
        where OEntity : class, new()
        where IEntity : class, new()
    {
        private IEntity? payloadIN;
        private OEntity? payloadOUT;

        public delegate OEntity VoidDelegate();
        public delegate Task<OEntity?> TaskDelegate();

        public InputVersion version { get; set; }
        public InputFormat format { get; set; }
        public SourceSystems source { get; set; } = SourceSystems.Epic;
        public Guid tenant { get; set; }

        public AddressAdapter(Guid tenant, InputFormat format, InputVersion version, SourceSystems source)
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

            var meta = new PalisaidMeta.Model.Address();
            if (meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            meta.TenantId = this.tenant;
            meta.EntityId = fhir.ElementId ?? Guid.NewGuid().ToString();
            
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
            var meta = payloadIN as PalisaidMeta.Model.Address;
            if(meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            var fhir = new Hl7.Fhir.Model.Address();
            if(fhir == null)
            {
                throw new ArgumentNullException(nameof(fhir));
            }
            
            fhir.ElementId = meta.EntityId;

            await Task.Run(() =>
            {
                var lines = new List<string>
                {
                    !string.IsNullOrEmpty(meta.Address1) ? meta.Address1 : "Empty",
                    !string.IsNullOrEmpty(meta.Address2) ? meta.Address2 : "Empty",
                    !string.IsNullOrEmpty(meta.Address3) ? meta.Address3 : "Empty"
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
            this.payloadIN = payload as IEntity;

            // Use full names to differenciate
            Dictionary<Tuple<string, InputVersion>, TaskDelegate> jumpTable = new()

            {
                { new Tuple<string, InputVersion>(@"Hl7.Fhir.Model.Address => PalisaidMeta.Model..Address", InputVersion.HL7FhirDstu2), ConvertFhirToMeta },
                { new Tuple<string, InputVersion>(@"PalisaidMeta.Model..Address => Hl7.Fhir.Model.Address", InputVersion.HL7FhirDstu2), ConvertMetaToFhir }            
            };

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