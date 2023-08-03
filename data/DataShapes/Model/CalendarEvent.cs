/*
 MIT License - CalendarEvent.cs

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataShapes.Model
{
    public enum CalendarEventStatus
    {
        Planned,
        Arrived,
        Triaged,
        InProgress,
        Onleave,
        Finished,
        Cancelled,
        EnteredInError,
        Noshow,
        Requested,
        Scheduled,
        Complete,
        ReadyForReview,
        Unknown
    }

    public enum CalendarEventPriority
    {
        Unknown,
        Low,
        Normal,
        High,
        Critical
    }

    public class CalendarInvite : Entity
    {
        public DateTimeOffset SentDate { get; set; }
        public DateTimeOffset ResponseDate { get; set; }
        public string? Email { get; set; }
        public string? EventTitle { get; set; }
        public string? EventText { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    public class CalendarEvent : Entity
    {
        public CalendarEventStatus EventStatus { get; set; }
        public CalendarEventPriority EventPriority { get; set; }
        public DateTimeOffset EventDate { get; set; }
        public string? EventTitle { get; set; }
        public string? EventText { get; set; }

        public DisposableList<Code> EventCodes { get; set; } = new();
        public DisposableList<CalendarInvite> ParticipantInvites { get; set; } = new();

        /// <summary>
        /// Uri's for anything attached to the event like video conference, document references, etc,
        /// </summary>
        public List<Uri> EventLinks { get; set; } = new();

        public Location Place { get; set; } = new();
        public Practitioner Host { get; set; } = new();
        public DisposableList<Patient> Invitees { get; set; } = new();

        public CalendarEvent() { }

        public CalendarEvent(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId) { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                EventCodes.Dispose();
                ParticipantInvites.Dispose();

                EventLinks.Clear();
                EventLinks.TrimExcess();

                EventText = string.Empty;
                EventDate = DateTimeOffset.MinValue;
            }
        }
    }
}