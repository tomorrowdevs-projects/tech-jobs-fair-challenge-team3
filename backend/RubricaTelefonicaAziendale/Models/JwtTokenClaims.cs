using System;

namespace RubricaTelefonicaAziendale.Models
{
   public class JwtTokenClaims
    {
        public String? UserId { get; set; } = null!;
        public String? Fullname { get; set; }
        public String? Username { get; set; } = null!;
        public String? Email { get; set; }
        public String? Role { get; set; }
    }
}