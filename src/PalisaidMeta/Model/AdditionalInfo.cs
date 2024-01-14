/*
 MIT License - AdditionalInfo.cs

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

namespace PalisaidMeta.Model
{
    /// <summary>
    /// Proxy for Fhir Extension
    /// </summary>
    public class AdditionalInfo
    {
        public AdditionalInfo()
        {
        }

        public string? Url { get; set; }
        public string? ValueBase64Binary { get; set; }
        public bool? ValueBoolean { get; set; }
        public string? ValueCanonical { get; set; }
        public string? ValueCode { get; set; }
        public string? ValueDate { get; set; }
        public string? ValueDateTime { get; set; }
        public decimal? ValueDecimal { get; set; }
        public string? ValueId { get; set; }
        public string? ValueInstant { get; set; }
        public int? ValueInteger { get; set; }
        public string? ValueMarkdown { get; set; }
        public string? ValueOid { get; set; }
        public uint? ValuePositiveInt { get; set; }
        public string? ValueString { get; set; }
        public string? ValueTime { get; set; }
        public string? ValueUri { get; set; }
        public string? ValueUrl { get; set; }
        public Guid? ValueUuid { get; set; }
        public Address? ValueAddress { get; set; }
        public string? ValueAge { get; set; }
        public Note? ValueAnnotation { get; set; }
        public string? ValueAttachment { get; set; }

        //public CodeableConcept ValueCodeableConcept { get; set; }
        //public Coding ValueCoding { get; set; }
        //public ContactPoint ValueContactPoint { get; set; }

        public int? ValueCount { get; set; }
        public decimal? ValueDistance { get; set; }

        //public Duration? ValueDuration { get; set; }
        public PersonName? ValueHumanName { get; set; }

        public Identifier? ValueIdentifier { get; set; }
        public decimal? ValueMoney { get; set; }

        //public Duration? ValuePeriod { get; set; }
        public decimal? ValueQuantity { get; set; }

        public Range? ValueRange { get; set; }
        public string? ValueRatio { get; set; }
        public string? ValueReference { get; set; }
        public object? ValueSampledData { get; set; }
        public object? ValueSignature { get; set; }
        public int? ValueTiming { get; set; }
        public Contact? ValueContactDetail { get; set; }
        public string? ValueContributor { get; set; }
        public string? ValueDataRequirement { get; set; }
        public string? ValueExpression { get; set; }
        public object? ValueParameterDefinition { get; set; }
        public string? ValueRelatedArtifact { get; set; }
        public string? ValueUsageContext { get; set; }
        public DoseSchedule? ValueDosage { get; set; }
        public string? ValueMeta { get; set; }
    }
}