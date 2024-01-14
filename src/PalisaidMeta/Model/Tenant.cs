/*
 MIT License - Customer.cs

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
    [Serializable]
    public class Tenant : Entity
    {
        public string? Name { get; set; }
        public string? Department { get; set; }
        public string? WorkGroup { get; set; }
        public string? Team { get; set; }
        public string? ManagerName { get; set; }
        public string? AdminName { get; set; }

        [NotMapped]
        public Address? PrimaryAddress
        {
            get => Addresses?.FirstOrDefault(a => a.AddressType == AddressType.Primary);
        }

        public DisposableList<ContactMethod>? ContactMethods { get; set; } = new();
        public DisposableList<Address>? Addresses { get; set; } = new();
        public DisposableList<Contact>? Contacts { get; set; } = new();
        public DisposableList<PaymentMethod>? PaymentMethods { get; set; } = new();

        public Tenant() { }
       

        /// <summary>
        /// The root object for each partition
        /// </summary>
        /// <param name="key"> </param>
        public Tenant(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) { }
    
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Name = string.Empty;
                Department = string.Empty;
                WorkGroup = string.Empty;
                ManagerName = string.Empty;
                AdminName = string.Empty;

                PrimaryAddress?.Dispose();
                ContactMethods?.Dispose();
                Addresses?.Dispose();
                Contacts?.Dispose();
                PaymentMethods?.Dispose();

                Addresses?.Clear();
                Addresses?.TrimExcess();
                Contacts?.Clear();
                Contacts?.TrimExcess();
                PaymentMethods?.Clear();
                PaymentMethods?.TrimExcess();
            }
        }
    }
}