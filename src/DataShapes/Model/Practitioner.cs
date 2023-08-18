/*
 MIT License - Practitioner.cs

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

//using MongoDB.Bson.Serialization.Serializers;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataShapes.Model
{
    public enum PractitionerTypes
    {
        Unknown,
        LPN,
        RN,
        MD,
        PhD,
        Technician
    };

    public enum Gender
    {
        Male,
        Female,
        Nonbinary,
        Other
    };

    /// <summary>
    /// <c> Practitioner </c> A meta representation of a practitioner. A practitioner is a person
    /// that provides a service or services in a health care environment
    /// </summary>
    [Serializable]
    public class Practitioner : Entity
    {
        public bool IsRefering { get; set; }

        public Practitioner() { }
        
        /// <summary>
        /// Any provider type
        /// </summary>
        /// <param name="key"> </param>
        public Practitioner(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) { }

        /// <summary>
        /// Practitioners plain text Id as defined in the EHR system
        /// </summary>
        public string? PractitionerIdentifier { get; set; }

        /// <summary>
        /// Basic representation of education type/focus - not to be confused with speciality definition
        /// </summary>
        public PractitionerTypes PractitionerType { get; set; }

        /// <summary>
        /// Filter for doctor vs other provider types
        /// </summary>
        [NotMapped]
        public bool IsMD => PractitionerType == PractitionerTypes.MD;

        /// <summary>
        /// Identifiers - list of ids, it's a FHIR thing
        /// </summary>
        public DisposableList<Identifier> FhirIdentifiers { get; set; } = new();

        /// <summary>
        /// Places the practitioner works, teaches, etc.
        /// </summary>
        public DisposableList<Location> Locations { get; set; } = new();

        /// <summary>
        /// Licenses, for example key: DEA value: AC1234567
        /// </summary>
        public Dictionary<string, string> LicensesAndQualifications { get; set; } = new();

        /// <summary>
        /// Name based on the FHIR HumanName It, a list because there can be multiple give names
        /// that can be treated as aliases, one per alias" <seealso cref="PersonName" />
        /// </summary>
        public DisposableList<PersonName>? Name { get; set; } = new();

        /// <summary>
        /// All addresses for practioner incliding Office, Hospital, Clinic, Personal, etc ...
        /// </summary>
        public DisposableList<Address> Addresses { get; set; } = new();

        /// <summary>
        /// A collection of ways to communicate with this person
        /// </summary>
        public DisposableList<ContactMethod>? ContactMethods { get; set; } = new();

        public string? PrimaryLanguage { get; set; }
        public Gender Gender { get; set; }
        public DateTimeOffset BirthDate { get; set; }

        public bool IsDeceased { get; set; }
        public DateTimeOffset DeceasedDate { get; set; }

        /// <summary>
        /// Primary practitioner address extracted from address list
        /// </summary>
        [NotMapped]
        public Address? PrimaryAddress
        {
            get => Addresses.FirstOrDefault(a => a.AddressType == AddressType.Primary);
        }

        /// <summary>
        /// The primary phone number
        /// </summary>
        [NotMapped]
        public Phone? PrimaryPhone
        {
            get => ContactMethods != null && ContactMethods.Count > 0
                       ? ContactMethods?.FirstOrDefault(cm => cm?.Phone?.Priority == PhonePriority.Primary)?.Phone
                        : new Phone(this.OwnerId, this.TenantId);
        }

        /// <summary>
        /// A shortcut to the practioners DEA id
        /// </summary>
        [NotMapped]
        public string? DEA
        {
            get => LicensesAndQualifications.FirstOrDefault(l => l.Key == "DEA").Value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PractitionerIdentifier = null;
                Locations?.Dispose();
                Name?.Dispose();
                Addresses?.Dispose();
                ContactMethods?.Dispose();
                FhirIdentifiers?.Dispose();
                Addresses?.Dispose();
                LicensesAndQualifications.Clear();
            }
        }
    }
}