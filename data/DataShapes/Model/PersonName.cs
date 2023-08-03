/*
 MIT License - PersonName.cs

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



namespace DataShapes.Model
{
    [Serializable]
    public class PersonName : Entity
    {
        public PersonName() { }
        
        public PersonName(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) { }

        public List<string?>? Prefix { get; set; } = new();

        [NotMapped]
        public string? Title
        {
            get => (Prefix != null && Prefix.Count > 0) ? Prefix[0] : null;
            set { Prefix?.Add(value); }
        }

        public List<string?>? GivenName { get; set; } = new();

        [NotMapped]
        public string? FirstName
        {
            get => (GivenName != null && GivenName.Count > 0) ? GivenName[0] : null;
            set { GivenName?.Insert(0, value); }
        }

        public string? FamilyName { get; set; }
        public string? MiddleName { get; set; }

        [NotMapped]
        public string? MiddleInitial { get => !string.IsNullOrEmpty(MiddleName) ? MiddleName?.Substring(0, 1).ToUpper() : null; }
        public string? KnownByName { get; set; }
        public List<string?>? Suffix { get; set; } = new();

        [NotMapped]
        public string? Degree
        {
            get => (Suffix != null && Suffix.Count > 0) ? Suffix[0] : null;
            set { Suffix?.Add(value); }
        }

        /// <summary>
        /// How long this name was used/has been used
        /// </summary>
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset StopDate { get; set; }

        /// <summary>
        /// There can be multiple geven names, prefixes, and suffixes. Here we generate a list of
        /// all possible combinations of names with a compounded collection of suffixes
        ///
        /// Mr. Fredrick Flintstone Esq Dmf Mr. Fred Flintstone Esq Dmf Mr. Hifi Flintstone Esq Dmf ...
        /// </summary>
        /// <returns> </returns>
        //[NotMapped]
        //public DisposableList<PersonName>? PersonNames { get; set; }

        private void MakeNameList()
        {
            //PersonNames?.Clear();
            var suffixlist = string.Empty;

            if (Suffix != null)
            {
                foreach (var s in Suffix)
                {
                    suffixlist += $" {s}";
                }
            }

            if (GivenName != null)
            {
                foreach (var n in GivenName)
                {
                    if (Prefix != null)
                    {
                        foreach (var p in Prefix)
                        {
                            var pn = new PersonName();
                            pn.GivenName?.Add(n);
                            pn.Prefix?.Add(p);
                            pn.Suffix?.Add(suffixlist);
                            pn.FamilyName = FamilyName;
                            //PersonNames?.Add(pn);
                        }
                    }
                }
            }
        }

        //public DisposableList<PersonName>? GetNames()
        //{
        //    MakeNameList();
        //    return PersonNames;
        //}

        protected override void Dispose(bool disposing)
        {
            Prefix?.Clear();
            GivenName?.Clear();
            Suffix?.Clear();

            Prefix = null;
            GivenName = null;
            Suffix = null;
            FamilyName = null;
            MiddleName = null;
            KnownByName = null;
            StartDate = DateTimeOffset.MinValue;
            StopDate = DateTimeOffset.MinValue;
            //PersonNames?.Dispose();
        }
    }
}