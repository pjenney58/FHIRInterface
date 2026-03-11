/*
 MIT License - Contact.cs

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
    public enum ContactType
    {
        Mother,
        Father,
        Friend,
        NextOfKin,
        Guardian,
        Clevel,
        DepartmentManager,
        PracticeManager,
        ProgramManager,
        Educator,
        Patient,
        Other
    }

    public class Contact : Entity
    {
        public ContactType Type { get; set; }
        public PersonName? Name { get; set; }
        public Address? Address { get; set; } = new();
        public DisposableList<ContactMethod> ContactMethods { get; set; } = new();

        public Contact()
        { }

        public Contact(Guid tenantId, Guid ownerId)
            : base(tenantId, ownerId) { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Name?.Dispose();
                Address?.Dispose();
                ContactMethods?.Dispose();
            }
        }
    }
}