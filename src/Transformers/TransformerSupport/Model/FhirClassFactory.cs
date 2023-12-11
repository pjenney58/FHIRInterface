/*
 MIT License - FhirClassFactory.cs

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

using DataShapes.Model;
using Hl7.Fhir.Model;

//using R5 = r5::Hl7.Fhir.Model;
//using R4 = r4::Hl7.Fhir.Model;
//using R4b = r4b::Hl7.Fhir.Model;
//using Stu3 = stu3::Hl7.Fhir.Model;
//using Dstu2 = dstu2::Hl7.Fhir.Model;

using System;

namespace Transformers.Model
{
    /// <summary>
    /// <c> FhirClassFactory </c> Builds and delivers a class normalized to a specific fhir version
    /// </summary>
    public static class FhirClassFactory
    {
        private static T NormalizeVersion<T>(object version)
        {
            switch (version)
            {
                case Hl7Version.Dstu2:
                // return version as Stu2.T;

                case Hl7Version.Stu3:
                // return version as Stu3.T;

                case Hl7Version.R4:
                // return version as R4.T;

                case Hl7Version.R4b:
                // return version as R4B.T;

                case Hl7Version.R5:
                // return version as R5.T;

                default:
                    return (T)version;
            }
        }

        public static object GetClass(string type, Hl7Version version)
        {
            switch (type.ToLower())
            {
                case "address":
                    return new Hl7.Fhir.Model.Address();
                //return NormalizeVersion(new Hl7.Fhir.Model.Address());

                case "appointment":
                    return new Hl7.Fhir.Model.Appointment();
                    //return NormalizeVersion(new Hl7.Fhir.Model.Appointment());
            }

            return default;
        }
    }
}