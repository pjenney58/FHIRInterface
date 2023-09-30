// Units
// Dates
// Readings
// Ranges
// Encounter
// Patient ID
// Practitioner ID
// Location ID

namespace DataShapes.Model
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

    public class TestResultValue : Entity
    {
        private bool disposedValue;

        public DateTimeOffset Date { get; set; }
        public decimal Value { get; set; }  // Note, gRPC needs a double as it doesnt implement decimal.
        public Units Unit { get; set; }
        public TestType TestType { get; set; }

        public TestResultValue(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId)
        { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Date = DateTime.MaxValue;
                Value = 0M;
                Unit = Units.None;
                TestType = TestType.None;
            }
        }
    }

    public class TestResult : Entity
    {
        DataShapeContext _context;

        // Test Metadata
        public Guid TestedPatientId { get; set; }
        public Guid RequestedByPractionerId { get; set; }
        public Guid TestLocationId { get; set; }
        public Guid TestEncounterId { get; set; }

        public TestType TestType { get; set; }
        public DateTimeOffset RunDate { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }

        public DisposableList<Note> TestNotes { get; set; } = new();
        public DisposableList<TestResultValue> Items { get; set; } = new();

        public TestResult(Guid ownerId, Guid tenantId)
            : base(ownerId, tenantId)
        {
            RunDate = DateTimeOffset.Now;
            _context = new DataShapeContext();
        }

        public int ResultCount => Items.Count;

        public async Task GetData() =>
            Items = (DisposableList<TestResultValue>)await Task.Run(() => _context.TestResultValuess
                                                               .Where(v =>
                                                                      v.TenantId == TenantId &&
                                                                      v.OwnerId == TestedPatientId &&
                                                                      v.TestType == TestType &&
                                                                      v.Date >= StartDateTime &&
                                                                      v.Date <= EndDateTime).ToList());
        

        public async Task AddData(TestResultValue value)
        {
            await _context.AddAsync(value);
            await _context.SaveChangesAsync();
        }

        public async Task AddData(DisposableList<TestResultValue> value)
        {
            await _context.AddAsync(value);
            await _context.SaveChangesAsync();
        }

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
                Items.Dispose();
            }
        }
    }
}