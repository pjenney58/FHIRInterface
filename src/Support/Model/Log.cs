namespace Support.Model
{
    public class Log
    {
        public Guid LogId { get; set; }
        public string? LogItem { get; set; }
        public string? LogItemDescription { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}