namespace Support.Model
{
    public class TokenModel
    {
        public string? access_token { get; set; }
        public string? refresh_token { get; set; }
        public DateTimeOffset? valid_to { get; set; }
        public string? entity_id { get; set; }
    }
}