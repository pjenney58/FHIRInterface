/*
 MIT License - RandomData.cs

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

/*
{
  "_id": "{{uuid}}",
  "name": "{{firstname}} {{surname}}",
  "dob": "{{date 2014-01-01}}",
  "address": {
    "street": "{{street}}",
    "town": "{{town}}",
    "postode": "{{postcode}}"
  },
  "telephone": "{{tel}}",
  "pets": ["{{cat}}","{{dog}}"],
  "score": {{float 1 10 1}},
  "email": "{{email}}",
  "url": "{{website}}",
  "description": "{{words 20}}",
  "verified": {{boolean 0.75}},
  "salary": {{float 10000 70000 0}}
}
 */

using Bogus;
using System.Text.Json.Serialization;
using System.Text.Json;
using RandomDataGenerator;

namespace RandomDataGenerator
{
    public static class RandomData
    {
        private static List<string> TextCollection = new List<string>();
        private static string locale = "en";

        /// <summary> Set's the current local for data generation: Locale Code Language af_ZA
        /// Afrikaans fr_CH French(Switzerland) ar Arabic ge Georgian az Azerbaijani hr Hrvatski cz
        /// Czech id_ID Indonesia de German it Italian de_AT German(Austria) ja Japanese de_CH
        /// German(Switzerland) ko Korean el Greek lv Latvian nb_NO Norwegian en English en_AU
        /// English(Australia) en_ZA English(South Africa) en_BORK English(Bork) en_AU_ocker
        /// English(Australia Ocker) en_CA English(Canada) en_IE English(Ireland) en_GB
        /// English(Great Britain) en_US English(United States) ne Nepalese nl Dutch nl_BE
        /// Dutch(Belgium) pl Polish pt_BR Portuguese(Brazil) pt_PT Portuguese(Portugal) en_IND
        /// English(India) ro Romanian en_NG Nigeria(English) ru Russian sk Slovakian sv Swedish es
        /// Spanish tr Turkish es_MX Spanish(Mexico) uk Ukrainian fa Farsi vi Vietnamese fi Finnish
        /// zh_CN Chinese fr French zh_TW Chinese(Taiwan) fr_CA French(Canada) zu_ZA Zulu(South Africa)
        public static void SetLocale(string locale)
        {
            locale = locale.ToLower();
        }

        public static string String()
        {
            if (!TextCollection.Any())
            {
                TextCollection = new Faker().Lorem.Sentences(500, "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            var rng = new Random();
            var randomElement = TextCollection[rng.Next(TextCollection.Count)];

            return randomElement;
        }

        public static string String(int len)
        {
            if (len > TextCollection[0].Length)
            {
                var s = string.Empty;

                while (s.Length < len)
                {
                    s += String();
                }

                return s.Substring(0, len);
            }

            return String();
        }

        public static Person GetOnePerson()
        {
            return Bool() ? GetOneMalePerson() : GetOneFemalePerson();
        }

        public static IEnumerable<Person> GetPeople(int count = 1)
        {
            List<Person> people = new List<Person>();

            for (int i = 0; i < count; i++)
            {
                people.Add(new Faker().Person);
            }

            return people;
        }

        public static Person GetOneMalePerson()
        {
            while (true)
            {
                var name = new Faker(locale).Person;
                if (name.Gender == Bogus.DataSets.Name.Gender.Male)
                {
                    return name;
                }
            }
        }

        public static Person GetOneFemalePerson()
        {
            while (true)
            {
                var name = new Faker(locale).Person;
                if (name.Gender == Bogus.DataSets.Name.Gender.Female)
                {
                    return name;
                }
            }
        }

        public static Person GetOneNonBinaryPerson()
        {
            return new Faker(locale).Person;
        }

        [Obsolete("Use GetOnePerson()")]
        public static async Task<Result> GetRandomPersonAsync()
        {
            while (true)
            {
                try
                {
                    var rp = await Task.Run(() => GetName().results[0]);
                    return rp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to get a good random person, trying again {ex.Message}"); // oops
                }
            }
        }

        [Obsolete("Use GetOnePerson()")]
        public static Result GetOneRantomPerson()
        {
            while (true)
            {
                try
                {
                    var rp = GetName().results[0];
                    return rp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to get a good random person, trying again {ex.Message}"); // oops
                }
            }
        }

        public static Root GetName(bool male = true)
        {
            // US Census Dta API Key = 32356f407e03f354bebb76be32985b2018d19409
            var debugText = string.Empty;

            var options = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.WriteAsString
            };

            var requestUrl = "https://randomuser.me/api/";
            try
            {
                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(requestUrl)))
                    {
                        request.Headers.Add("Accept", "application/json");

                        using (var response = client.SendAsync(request).Result)
                        {
                            response.EnsureSuccessStatusCode();
                            var text = response.Content.ReadAsStringAsync().Result;
                            debugText = text;
                            return JsonSerializer.Deserialize<Root>(text, options);
                        }
                    }
                }
            }
            catch (HttpRequestException hex)
            {
                Console.WriteLine(hex.Message);
                throw;
                //return default(T);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}\n{debugText}");
                throw;
            }
        }

        public enum Gender
        {
            Male,
            Female,
            Nonbinary,
            Other
        };

        public static Gender GetGender()
        {
            return (Gender)Integer(0, 3);
        }

        private static string[] GetLorum(int lineCount)
        {
            return new Faker().Lorem.Sentences(lineCount, "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);
        }

        private static string GetLoremSlug(int wordcount)
        {
            return new Faker().Lorem.Slug(wordcount);
        }

        private static string GetLoremWord()
        {
            return new Faker().Lorem.Word();
        }

        public static string TrimString()
        {
            return FullTrim(String());
        }

        public static string TrimString(int len)
        {
            var val = FullTrim(String());

            if (val.Length <= len)
            {
                while (val.Length <= len)
                {
                    val = FullTrim(String());
                }
            }

            return val.Substring(0, len);
        }

        // ------- Format Utilities
        private static string FullTrim(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }

            char[] junk = { ' ', '-', '.', ',', ' ', ';', ':', '(', ')', '?', '*', '!' };

            while (val?.IndexOfAny(junk) > -1)
            {
                val = val.Remove(val.IndexOfAny(junk), 1);
            }

            return val;
        }

        public static string EmailAddress(bool legacy = false)
        {
            if (!legacy)
            {
                return new Faker().Person.Email.ToLower();
            }

            return $"{TrimString(5)}.{TrimString(7)}@{TrimString(8)}.{TrimString(4)}";
        }

        public static double Double(int multiplier)
        {
            return new Random((int)DateTime.Now.Ticks & 0x0000FFFF).NextDouble() * multiplier;
        }

        public static decimal Decimal(int multiplier)
        {
            return Convert.ToDecimal(new Random((int)DateTime.Now.Ticks & 0x0000FFFF).NextDouble() * multiplier);
        }

        public static int Integer(int minVal = 1, int maxVal = int.MaxValue)
        {
            return new Random((int)DateTime.Now.Ticks & 0x0000FFFF).Next(minVal, maxVal);
        }

        public static string NDC(int len)
        {
            if (len != 9 && len != 11) throw new ArgumentException("value must be 9 or 11");

            string ndc = "";

            for (var i = 0; i < 11; i++)
            {
                ndc += Integer(0, 10).ToString();
            }

            return ndc;
        }

        public static Tuple<DateTimeOffset, TimeSpan, int> GetAppointmentTimeDate(int minYear = 1900, int maxYear = 2050)
        {
            var dto = AnyDateTimeOffset(DateTimeOffset.Now.AddYears(-10).Year, DateTimeOffset.Now.AddYears(10).Year).ToLocalTime() + new TimeSpan(Integer(0600, 2000));
            var duration = (int)(Math.Round(Convert.ToDecimal(Integer(1, 8)))) * 15;

            return new Tuple<DateTimeOffset, TimeSpan, int>(dto.ToLocalTime(), dto.ToLocalTime().TimeOfDay, duration);
        }

        public static DateOnly AnyDate(int minYear = 1900, int maxYear = 2050)
        {
            var rnd = new Random(Guid.NewGuid().GetHashCode());

            if (minYear >= maxYear)
            {
                maxYear = minYear + 2;
            }

            var year = rnd.Next(minYear, maxYear);
            var month = rnd.Next(1, 13);
            var days = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);

            return new DateOnly(year, month, days);
        }

        public static DateOnly PastDate(int minYear = 1900)
        {
            return AnyDate(minYear, DateTime.Now.Year);
        }

        public static DateOnly FutureDate(int maxYear = 2050)
        {
            return AnyDate(DateTime.Now.Year, maxYear);
        }

        public static DateTime AnyDateTime(int minYear = 1900, int maxYear = 2050)
        {
            var rnd = new Random(Guid.NewGuid().GetHashCode());

            if (minYear >= maxYear)
            {
                maxYear = minYear + 2;
            }

            var year = rnd.Next(minYear, maxYear);
            var month = rnd.Next(1, 13);
            var days = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);

            return new DateTime(year, month, days, rnd.Next(0, 24), rnd.Next(0, 60), rnd.Next(0, 60), rnd.Next(0, 1000));
        }

        public static DateTime PastDateTime(int minYear = 1900)
        {
            return AnyDateTime(minYear, DateTime.Now.Year);
        }

        public static DateTime FutureDateTime(int maxYear = 2050)
        {
            return AnyDateTime(DateTime.Now.Year, maxYear);
        }

        public static DateTimeOffset AnyDateTimeOffset(int minYear = 1900, int maxYear = 2050)
        {
            return new DateTimeOffset(AnyDateTime(minYear, maxYear).ToLocalTime());
        }

        public static DateTimeOffset PastDateTimeOffset(int minYear = 1900)
        {
            return new DateTimeOffset(PastDateTime(minYear).ToLocalTime());
        }

        public static DateTimeOffset FutureDateTimeOffset(int maxYear = 2050)
        {
            return new DateTimeOffset(FutureDateTime(maxYear).ToLocalTime());
        }

        public static int Bit()
        {
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            return rnd.Next(0, 2);
        }

        public static bool Bool()
        {
            return Bit() == 1;
        }

        public static string USPhoneNumber()
        {
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            return $"({rnd.Next(1, 1000).ToString("D3")}){rnd.Next(1, 1000).ToString("D3")}-{rnd.Next(0000, 10000).ToString("D4")}";
        }

        public static string CountryCode()
        {
            return Integer(1, 1000).ToString("D3");
        }

        public static string AreaCode()
        {
            return Integer(1, 1000).ToString("D3");
        }

        public static string Exchange()
        {
            return Integer(1, 1000).ToString("D3");
        }

        public static string Number()
        {
            return Integer(0, 1000).ToString("D4");
        }

        public static string SSN()
        {
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            return $"{rnd.Next(1, 1000).ToString("D3")}-{rnd.Next(1, 100).ToString("D2")}-{rnd.Next(1000, 10000).ToString("D4")}";
        }

        public static string DoseTimes(int numTimes = 1)
        {
            List<string> quarterHours = new List<string>()
            {
                "00",
                "25",
                "33",
                "50",
                "66",
                "75"
            };

            var rnd = new Random(Guid.NewGuid().GetHashCode());

            var doseTime = $"{Integer(0, 25).ToString("D2")}{Integer(0, 61).ToString("D2")}" +
                           $"{Integer(0, 13).ToString("D2")}{quarterHours[rnd.Next(quarterHours.Count)]}";

            if (numTimes > 1)
            {
                for (var i = 1; i < numTimes; i++)
                {
                    doseTime += $"{Integer(0, 25).ToString("D2")}{Integer(0, 61).ToString("D2")}" +
                                $"{Integer(0, 13).ToString("D2")}{quarterHours[rnd.Next(quarterHours.Count)]}";
                }
            }

            return doseTime;
        }

        public static string ShortDEA()
        {
            return $"{TrimString(2).ToUpper()}{Integer(1000000, 10000000)}";
        }

        public static string Address()
        {
            return $"{Integer(100, 10000)} {TrimString(25)} {TrimString(3)}.";
        }
    }
}