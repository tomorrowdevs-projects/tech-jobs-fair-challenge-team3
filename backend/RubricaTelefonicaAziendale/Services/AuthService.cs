using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RubricaTelefonicaAziendale.Entities;
using RubricaTelefonicaAziendale.Models;

namespace RubricaTelefonicaAziendale.Services
{
    public interface IAuthService
    {
        Task<Users?> Login(String username, String password);
        Task<Users?> Register(Users user, Roles role);

        JwtTokenClaims? WhoAmI();

        Task<Boolean> ExistUserWithUsername(String username);

        String GenerateToken(Users? user, Roles? role);

        Task<Users?> GetUserById(String userid);

        Task<Roles?> GetRoleByUserId(String userid);
        Task<Roles?> GetRoleByDesc(String roledesc);
    }

    public class AuthService : BaseService, IAuthService
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;

        public AuthService(TjfChallengeContext dataContext,
                            IHttpContextAccessor httpContextAccessor,
                            IServiceProvider serviceProvider,
                            IOptions<JwtSettings> jwtSettingsOptions,
                            IUserService userService,
                            IRoleService roleService)
            : base(dataContext, httpContextAccessor, serviceProvider, jwtSettingsOptions)
        {
            this.userService = userService;
            this.roleService = roleService;
        }

        public async Task<Users?> Login(String username, String password)
        {
            Users? user = null;
            try
            {
                user = await userService.GetByUsername(username);
                if (user == null) return null;
                if (!VerifyPassword(password, user.Salt!, user.Password!)) return null;
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return user;
        }

        public async Task<Users?> Register(Users user, Roles role)
        {
            Users? userinserted = null;
            try
            {
                bool added = await userService.Insert(user);
                if (added)
                    userinserted = await userService.GetByUsername(user.Username);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return userinserted;
        }

        public JwtTokenClaims? WhoAmI()
        {
            return base.claims;
        }

        public async Task<Boolean> ExistUserWithUsername(String username)
        {
            Users? user = await userService.GetByUsername(username);
            return user != null;
        }

        private Boolean VerifyPassword(String Password, String storedSalt, String storedHash)
        {
            // hash the given password using the stored salt
            byte[] salt = Convert.FromBase64String(storedSalt);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                            password: Password,
                                            salt: salt,
                                            prf: KeyDerivationPrf.HMACSHA512,
                                            iterationCount: 10000,
                                            numBytesRequested: 1024 / 8));
            // compare both hashes
            return (String.Compare(storedHash, hash) == 0);
        }

        public String GenerateToken(Users? user, Roles? role)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user?.Id?.ToString() ?? ""),
                new Claim(ClaimTypes.Name, user?.Lastname + " " + user?.Firstname),
                new Claim(ClaimTypes.NameIdentifier, user?.Username ?? ""),
                new Claim(ClaimTypes.Role, role?.Description ?? "")
            };

            var key = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings?.IssuerSigningKey ?? ""));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                jwtSettings?.ValidIssuer ?? "",
                jwtSettings?.ValidAudience ?? "",
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtTokenClaims? DecodeToken(String Token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(Token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings?.IssuerSigningKey ?? "")),
                    ValidateLifetime = false
                }, out SecurityToken validatedToken);
                // check decoded token is valid
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                // extract all data inside token
                JwtTokenClaims jtc = new JwtTokenClaims();
                jtc.UserId = jwtToken?.Claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
                jtc.Fullname = jwtToken?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                jtc.Username = jwtToken?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                jtc.Email = jwtToken?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                jtc.Role = jwtToken?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                return jtc;
            }
            catch
            {
                return null;
            }
        }


        public async Task<Users?> GetUserById(String userid)
        {
            if (userid != null)
            {
                Users? obj = await userService!.GetByID(userid);
                if (obj is null) return null;
                else return obj;
            }
            else return null;
        }

        public async Task<Roles?> GetRoleByUserId(String userid)
        {
            if (userid != null)
            {
                Roles? role = await roleService!.GetByUserId(userid);
                if (role is null) return null;
                else return role;
            }
            else return null;
        }

        public async Task<Roles?> GetRoleByDesc(String roledesc)
        {
            Roles? role = await roleService.GetByDescription(roledesc);
            if (role == null) return null;
            else return role;
        }

    }
}