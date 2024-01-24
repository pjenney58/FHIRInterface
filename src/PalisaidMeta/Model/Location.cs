/*
 MIT License - Location.cs

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
    public enum LocationType
    {
        Hospital,
        UrgentCare,
        Clinic,
        LongTermCareFacility,
        NursingHome,
        Hospice,

        RetailPharmacy,
        ClosedDoorPharmacy,
        CentralFillPharmacy,

        PractitionerOffice,
        PatientHome
    };

    [Serializable]
    public class Location : Entity
    {
        public Location() { }
      
        public Location(Guid tenantId, Guid ownerId)
            : base(tenantId, ownerId) { }


        public string? Name { get; set; }
        public string? Description { get; set; }
        public LocationType? LocationType { get; set; }

        public Dictionary<string, string>? Licenses { get; set; } = new();
        public DisposableList<Contact>? Contacts { get; set; } = new();
        public DisposableList<Address>? Addresses { get; set; } = new();
        public DisposableList<ContactMethod>? ContactMethods { get; set; } = new();
        public DisposableList<Accreditation>? Accreditations { get; set; } = new();
        public int StarRating { get; set; }

        [NotMapped]
        public string? DEA
        {
            get => Licenses?.FirstOrDefault(d => d.Key == "DEA").Value;
        }

        [NotMapped]
        public string? NPI
        {
            get => Licenses?.FirstOrDefault(d => d.Key == "NPI").Value;
        }

        [NotMapped]
        public Address? PrimaryAddress
        {
            get => Addresses?.FirstOrDefault(a => a.AddressType == AddressType.Primary);
            set
            {
                if (value != null)
                {
                    value.AddressType = AddressType.Primary;
                    Addresses?.Add(value);
                }
            }
        }

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
                Name = null;
                Description = null;
                LocationType = null;

                Licenses?.Clear();

                Addresses?.Dispose();
                Contacts?.Dispose();
                ContactMethods?.Dispose();
            }
        }
    }
}