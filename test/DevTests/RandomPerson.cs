/*
 MIT License - RandomPerson.cs

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

namespace RandomDataGenerator
{
    public class Coordinates
    {
        public string? latitude { get; set; } = string.Empty;
        public string? longitude { get; set; } = string.Empty;
    }

    public class Dob
    {
        public DateTime date { get; set; }
        public int age { get; set; }
    }

    public class Id
    {
        public string? name { get; set; } = string.Empty;
        public string? value { get; set; } = string.Empty;
    }

    public class Info
    {
        public string? seed { get; set; }
        public int results { get; set; }
        public int page { get; set; }
        public string? version { get; set; }
    }

    public class Location
    {
        public Street? street { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
        public object? postcode { get; set; }
        public Coordinates? coordinates { get; set; }
        public Timezone? timezone { get; set; }
    }

    public class Login
    {
        public string? uuid { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? salt { get; set; }
        public string? md5 { get; set; }
        public string? sha1 { get; set; }
        public string? sha256 { get; set; }
    }

    public class Name
    {
        public string? title { get; set; }
        public string? first { get; set; }
        public string? last { get; set; }
    }

    public class Picture
    {
        public string? large { get; set; }
        public string? medium { get; set; }
        public string? thumbnail { get; set; }
    }

    public class Registered
    {
        public DateTime date { get; set; }
        public int age { get; set; }
    }

    public class Result
    {
        public string? gender { get; set; }
        public Name? name { get; set; }
        public Location? location { get; set; }
        public string? email { get; set; }
        public Login? login { get; set; }
        public Dob? dob { get; set; }
        public Registered? registered { get; set; }
        public string? phone { get; set; }
        public string? cell { get; set; }
        public Id? id { get; set; }
        public Picture? picture { get; set; }
        public string? nat { get; set; }
    }

    public class Root
    {
        public List<Result>? results { get; set; }
        public Info? info { get; set; }
    }

    public class Street
    {
        public int number { get; set; }
        public string? name { get; set; }
    }

    public class Timezone
    {
        public string? offset { get; set; }
        public string? description { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
}