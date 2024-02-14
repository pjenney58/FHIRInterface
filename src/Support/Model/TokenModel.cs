namespace Support.Model
{
    public class TokenModel
    {
        public string? accessToken { get; set; }
        public string? refreshToken { get; set; }
        public DateTimeOffset? validTo { get; set; }
        public Guid userId { get; set; }
    }
}

