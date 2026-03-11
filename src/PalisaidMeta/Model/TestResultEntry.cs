// units
// Dates
// Readings
// Ranges
// Encounter
// Patient ID
// Practitioner ID
// Location ID

namespace PalisaidMeta.Model
{
    public enum Units
    {
        None,
        Mg,
        Ml,
        Gm,
        Kg,
        Oz,
        Lbs
    }

    public enum TestType
    {
        None,
        A1C,
        LipidPanel,
        CBC,
        BloodCount,
        Urinalysis,
        AvgBloodSugar,
        UltrasoundHeadAndNeck,
        HPLC,
        BloodPressure,
        PumpTitration,
        ManualTitration
        // ...
    }

    public class TestResultEntry : Entity
    {
        public string? TestName { get; set; }
        public string? Value { get; set; }
        public string? ValueUnits { get; set; }
        public string? BottomRangeValue { get; set; }
        public string? TopRangeValue { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TestName = null;
                Value = null;
                ValueUnits = null;
                BottomRangeValue = null;
                TopRangeValue = null;
            }
        }
    }

    public class TestResults : Entity
    {
        // Test Metadata
        public Guid TestedPatientId { get; set; }

        public Guid RequestedByPractionerId { get; set; }
        public Guid TestLocationId { get; set; }
        public Guid TestEncounterId { get; set; }

        public DateTimeOffset RunDate { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }

        public DisposableList<Note> TestNotes { get; set; } = new();
        public DisposableList<TestResultEntry> Results { get; set; } = new();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RunDate =
                StartDateTime =
                EndDateTime = DateTimeOffset.MinValue;

                TestedPatientId =
                RequestedByPractionerId =
                TestLocationId =
                TestEncounterId = Guid.Empty;

                TestNotes.Dispose();
                Results.Dispose();
            }
        }
    }
}