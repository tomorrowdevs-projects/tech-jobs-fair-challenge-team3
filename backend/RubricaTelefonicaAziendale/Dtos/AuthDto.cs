namespace RubricaTelefonicaAziendale.Dtos
{
    public class LoginRequest
    {
        public String Username { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
    }
    
    public class AuthDto
    {
        public UserDto? User { get; set; }
        public String? Token { get; set; }
    }
}