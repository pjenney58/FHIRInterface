/*
 MIT License - DoseDay.cs

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

using Microsoft.EntityFrameworkCore;

namespace DataShapes.Model
{
    public class DoseDay : Entity
    {
        public DoseDay() {}
        
        public DoseDay(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) {}

        public DateTimeOffset ThisDoseDay { get; set; }
        public int DayOfMonth { get => ThisDoseDay.Day; }
        public List<DoseEvent> DoseEvents { get; set; } = new();

        public decimal DailyQty { get => DoseEvents.Sum(s => s.MinmumCountOrDefault); }

        // Check to see if the dose counts varry per time if there are more than 1 dosetimes "Take 1
        // tablet twice a day and 2 at bedtime"
        public bool IsVariableDose
        {
            get => DoseEvents.Count > 0 && DoseEvents.GroupBy(q => q.MinmumCountOrDefault).Count() > 0;
        }

        private string? _dosetimepattern;

        public string? DoseTimePattern // 0800~1400~2000
        {
            get
            {
                setpatterns();
                return _dosetimepattern;
            }
        }

        private string? _doseqtypattern;

        public string? DoseQtyPattern // 1~2~1
        {
            get
            {
                setpatterns();
                return _doseqtypattern;
            }
        }

        private void setpatterns()
        {
            if (DoseEvents == null)
            {
                return;
            }

            _doseqtypattern = string.Empty;
            _doseqtypattern = string.Empty;

            if (DoseEvents?.Count > 1)
            {
                var orderedtimes = DoseEvents.OrderBy(o => o.Time);

                for (int i = 0; i < DoseEvents.Count; i++)
                {
                    if (i != DoseEvents.Count - 1)
                    {
                        _dosetimepattern += $"{orderedtimes.ElementAt(i).Time}~";
                        _doseqtypattern += $"{orderedtimes.ElementAt(i).MinmumCountOrDefault}~";
                    }
                    else
                    {
                        _dosetimepattern += $"{orderedtimes.ElementAt(i).Time}";
                        _doseqtypattern += $"{orderedtimes.ElementAt(i).MinmumCountOrDefault}";
                    }
                }
            }
            else if (DoseEvents?.Count == 1)
            {
                _dosetimepattern = DoseEvents?.FirstOrDefault()?.Time.ToString("HHmm");
                _doseqtypattern = DoseEvents?.FirstOrDefault()?.MinmumCountOrDefault.ToString();
            }
        }
    }
}