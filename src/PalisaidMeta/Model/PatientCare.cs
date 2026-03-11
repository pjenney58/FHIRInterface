/*
 MIT License - PatientCare.cs

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
    public enum PatientCareCodeStatus : byte
    {
        NotSet,
        DNR,
        LimitedCode,
        FullCode
    }

    public enum PatientCareCodeStatusLimitedCodeOptions : byte
    {
        NotSet,
        Chemical,
        DNI
    }

    public class PatientCare : Entity
    {
        public PatientCare()
        { }

        public PatientCare(Guid tenantId, Guid ownerId)
            : base(tenantId, ownerId) { }

        public string? CareId { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public string? MedicareId { get; set; }
        public string? MedicaidId { get; set; }
        public string? PrimaryInsuranceName { get; set; }
        public string? PrimaryPolicyId { get; set; }
        public string? PrimaryGroupId { get; set; }
        public string? AlternateInsuranceName { get; set; }
        public string? AlternatePolicyId { get; set; }
        public string? AlternateGroupId { get; set; }
        public bool HipaaDataTransmitAuthentication { get; set; }
        public PatientCareCodeStatus CodeStatus { get; set; }
        public PatientCareCodeStatusLimitedCodeOptions CodeStatusLimitedCodeOptions { get; set; }
        public string? HipaaChangeAuthorLogin { get; set; }
        public DateTime HipaaChangeDate { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CareId = null;
                SocialSecurityNumber = null;
                MedicareId = null;
                MedicareId = null;
                PrimaryInsuranceName = null;
                PrimaryPolicyId = null;
                PrimaryGroupId = null;
                AlternateInsuranceName = null;
                AlternatePolicyId = null;
                AlternateGroupId = null;
                HipaaChangeAuthorLogin = null;
            }
        }
    }
}