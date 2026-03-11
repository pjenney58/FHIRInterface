/*
 MIT License - Patient.cs

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

using System.ComponentModel.DataAnnotations.Schema;

namespace PalisaidMeta.Model
{
    public class Patient : Entity
    {
        public Patient()
        { }

        public Patient(Guid tenantId, Guid ownerId)
            : base(tenantId, ownerId) { }

        public string? PrimaryPatientIdString { get; set; }
        public string? AlternatePatientIdString { get; set; }
        public List<string?> Accounts { get; set; } = new();
        public string? PatientClass { get; set; }

        // Override Location address info with patient's
        public bool UsePatientInfo { get; set; }

        public PersonName? Name { get; set; } = new();
        public Gender Gender { get; set; }
        public DisposableList<SpokenLanguage> Languages { get; set; } = new();
        public DateTimeOffset BirthDate { get; set; } = DateTimeOffset.Now;
        public bool IsDeceased { get; set; }
        public DateTimeOffset DeceasedDate { get; set; } = DateTimeOffset.Now;
        public byte[]? Photo { get; set; }
        public string? NursingStation { get; set; }
        public string? Floor { get; set; }
        public string? Room { get; set; }
        public string? Bed { get; set; }

        public PatientCare? PatientCare { get; set; }
        public DateTimeOffset AdmissionDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset DischargeDate { get; set; } = DateTimeOffset.Now;

        public DisposableList<Observation>? Observations { get; set; } = new();
        public DisposableList<Diagnosis>? Diagnoses { get; set; } = new();
        public DisposableList<Prescription>? Prescriptions { get; set; } = new();
        public DisposableList<Treatment>? Treatments { get; set; } = new();
        public DisposableList<Address>? Addresses { get; set; } = new();
        public DisposableList<PatientPractitioner>? Practitioners { get; set; } = new();
        public DisposableList<Location>? Locations { get; set; } = new();
        public DisposableList<ContactMethod>? ContactMethods { get; set; } = new();
        public DisposableList<Device> Devices { get; set; } = new();

        /// <summary>
        /// Identifiers - list of ids from Hl7-Fhir
        /// </summary>
        public DisposableList<Identifier> HL7Identifiers { get; set; } = new();

        /// <summary>
        /// Primary spoken language
        /// </summary>
        [NotMapped]
        public SpokenLanguage? PrimaryLanguage
        {
            get => Languages != null && Languages.Count > 0
                ? Languages?.FirstOrDefault(l => l.Use == SpokenLanguageUse.Primary)
                : new SpokenLanguage(this.TenantId, this.OwnerId);
        }

        /// <summary>
        /// Current facility if proper
        /// </summary>
        [NotMapped]
        public Location? PrimaryFacility
        {
            get => Locations != null && Locations.Count > 0
                ? Locations?.FirstOrDefault(l => l.LocationType == LocationType.NursingHome)
                : new Location(this.TenantId, this.OwnerId);
        }

        /// <summary>
        /// Primary pharmacy
        /// </summary>
        [NotMapped]
        public Location? Pharmacy
        {
            get => Locations != null && Locations.Count > 0
                ? Locations?.FirstOrDefault(l => l.LocationType == LocationType.RetailPharmacy)
                : new Location(this.TenantId, this.OwnerId);
        }

        [NotMapped]
        public Practitioner? PrimaryPractitioner
        {
            get => Practitioners != null && Practitioners.Count > 0
                ? Practitioners?.FirstOrDefault(p => p.Relationship == PractitionerRelationship.Primary)?.Practitioner
                : new Practitioner(this.TenantId, this.OwnerId);
        }

        [NotMapped]
        public Practitioner? AlternatePractitioner
        {
            get => Practitioners != null && Practitioners.Count > 0
                ? Practitioners?.FirstOrDefault(p => p.Relationship == PractitionerRelationship.Secondary)?.Practitioner
                : new Practitioner(this.TenantId, this.OwnerId);
        }

        /// <summary>
        /// Primary address
        /// </summary>
        [NotMapped]
        public Address? PrimaryAddress
        {
            get => Addresses != null && Addresses.Count > 0
                    ? Addresses?.FirstOrDefault(a => a.AddressType == AddressType.Primary)
                    : new Address(this.TenantId, this.OwnerId);
        }

        /// <summary>
        /// The primary phone number
        /// </summary>
        [NotMapped]
        public Phone? PrimaryPhone
        {
            get => ContactMethods != null && ContactMethods.Count > 0
                    ? ContactMethods?.FirstOrDefault(cm => cm?.Phone?.Priority == PhonePriority.Primary)?.Phone
                    : new Phone(this.TenantId, this.OwnerId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PrimaryPatientIdString = null;
                AlternatePatientIdString = null;

                Locations?.Clear();
                Addresses?.Clear();

                ContactMethods?.Dispose();
                Practitioners?.Dispose();
                Treatments?.Dispose();
                Prescriptions?.Dispose();
                Observations?.Dispose();
                Diagnoses?.Dispose();
                Name?.Dispose();
                HL7Identifiers?.Dispose();

                BirthDate = DateTime.MinValue;
                Photo = null;
                NursingStation = null;
                Floor = null;
                Room = null;
                Bed = null;

                AdmissionDate = DateTime.MinValue;
                DischargeDate = DateTime.MinValue;
            }
        }
    }
}