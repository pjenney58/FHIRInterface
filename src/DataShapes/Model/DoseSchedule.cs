/*
 MIT License - DoseSchedule.cs

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
    public class DoseSchedule : Entity
    {
        public DoseSchedule() { }

        public DoseSchedule(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) { }

        /// <summary>
        /// Schdule name or pattern
        /// </summary>
        public string? DoseScheduleName { get; set; }

        /// <summary>
        /// Common pattern names QS, BID, TID, QID, HS, ...
        /// </summary>
        public string? DoseRepeatPattern { get; set; } // Common pattern names

        // How to build a dose schedule:
        //
        // 1. Create a collection of Daily doses, eg:
        //
        // DailyDoses.Add(new DoseEvent( { Time = new TimeOnly(8,00), MinmumCountOrDefault = 1,
        // MaximumCount = 1 });
        //
        // DailyDoses.Add(new DoseEvent( { Time = new TimeOnly(16,00), MinmumCountOrDefault = 1,
        // MaximumCount =1 });
        //
        // DailyDoses.Add(new DoseEvent( { Time = new TimeOnly(20,00), MinmumCountOrDefault = 1,
        // MaximumCount = 2 });
        //
        // dosepattern:  0800:1~1600:1~2000:2
        //
        // Sig:  Take on tablet twice a day and 2 at bedtime

        public DisposableList<DoseEvent>? DailyDoses { get; set; } = new();

        // Set what days the dose events occur on if(RxType == RxType.Daily {
        // DoseEvents.AddRange(DailyDoses); setpatterns();
        //
        // var days = Duration.Count;
        //
        // var FirstDate = Duraton.StartDate; foreach(var day in days) { DoseDays.Add {new DoseDay(
        // { ThisDoseDay = FirstDate,
        //
        // }}

        public DisposableList<DoseDay> DoseDays { get; set; } = new();

        [NotMapped]
        public string DoseQty
        {
            get => DoseDays != null && DoseDays.Count > 0
                ? DoseDays.FirstOrDefault().DoseEvents.FirstOrDefault().MinmumCountOrDefault.ToString()
                : "0";
        }

        [NotMapped]
        public string TotalDailyDoseQty
        {
            get => DoseDays != null && DoseDays.Count > 0
                ? DoseDays.FirstOrDefault().DoseEvents.Sum(q => q.MinmumCountOrDefault).ToString()
                : "0";
        }

        [NotMapped]
        public bool IsTitrating { get => DoseDays.GroupBy(p => p.DailyQty).Count() > 0; }
        public bool IsPrn { get; set; }

        private string? _dayOfMonthList;

        /// <summary>
        /// Dose days of month
        /// </summary>
        
        [NotMapped]
        public string? DayOfMonthList // 1,6,9,22
        {
            get
            {
                if (DoseDays.Count > 0)
                {
                    _dayOfMonthList = string.Empty;
                    for (var i = 0; i < 31; i++)
                    {
                        _dayOfMonthList += $"{DoseDays.ElementAt(i)?.ThisDoseDay.Day},";
                    }

                    _dayOfMonthList?.Remove(_dayOfMonthList.LastIndexOf(','), 1);
                }
                return _dayOfMonthList;
            }
        }

        private string? _dayOfWeekList;

        /// <summary>
        /// Dose days of week
        /// </summary>

        [NotMapped]
        public string? DayOfWeekList // 0,2,4
        {
            get
            {
                if (DoseDays.Count > 0)
                {
                    _dayOfWeekList = string.Empty;
                    for (var i = 0; i < 7; i++)
                    {
                        _dayOfWeekList += $"{(int)DoseDays.ElementAt(i).ThisDoseDay.DayOfWeek},";
                    }

                    _dayOfWeekList?.Remove(_dayOfWeekList.LastIndexOf(','), 1);
                }

                return _dayOfWeekList;
            }
        }

        public int AlternatingRepeatDays { get; set; } = 1;

        // Clean up
        protected override void Dispose(bool disposing)
        {
            DailyDoses?.Dispose();

            _dayOfMonthList = null;
            _dayOfWeekList = null;
            DoseRepeatPattern = null;
        }
    }
}