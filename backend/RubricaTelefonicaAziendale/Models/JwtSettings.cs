namespace RubricaTelefonicaAziendale.Models
{
    public class JwtSettings
    {
        public Boolean ValidateIssuerSigningKey { get; set; }
        public String IssuerSigningKey { get; set; } = String.Empty;
        public Boolean ValidateIssuer { get; set; }
        public String ValidIssuer { get; set; } = String.Empty;
        public Boolean ValidateAudience { get; set; }
        public String ValidAudience { get; set; } = String.Empty;
        public Boolean RequireExpirationTime { get; set; }
        public Boolean ValidateLifetime { get; set; }
        public Int32 ExpirationMinutes { get; set; }
    }
}