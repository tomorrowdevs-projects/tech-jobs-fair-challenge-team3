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
        Task<Users?> Register(Users user);
        Task<Users?> Update(Users user);
        Task<Boolean> Delete(String id);

        JwtTokenClaims? WhoAmI();
        Task<Boolean> ExistUserWithUsername(String username);
        String GenerateToken(Users? user);
        Task<Users?> GetUserById(String userid);
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

        public async Task<Users?> Register(Users user)
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

        public async Task<Users?> Update(Users user)
        {
            Users? userupdated = null;
            try
            {
                bool updated = await userService.Update(user);
                if (updated)
                    userupdated = await userService.GetByUsername(user.Username);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return userupdated;
        }

        public async Task<Boolean> Delete(String id)
        {
            int res = -1;
            try
            {
                Users? user = await GetUserById(id);
                if (user == null) return res > 0;
                this.db.Remove(user);
                res = await this.db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return res > 0;
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

        public String GenerateToken(Users? user)
        {
            Claim[] claims = [
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("id", user?.Id.ToString() ?? ""),
                new Claim("fullname", user?.Lastname + " " + user?.Firstname),
                new Claim("username", user?.Username ?? ""),
                new Claim("role-id", user?.Role?.Id.ToString() ?? ""),
                new Claim("role", user?.Role?.Description ?? ""),
            ];
            JwtSecurityTokenHandler tokenHandler = new();
            Byte[] key = System.Text.Encoding.UTF8.GetBytes(jwtSettings?.IssuerSigningKey ?? "");
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = jwtSettings?.ValidIssuer ?? "",
                Audience = jwtSettings?.ValidAudience ?? "",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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

        public async Task<Roles?> GetRoleByDesc(String roledesc)
        {
            Roles? role = await roleService.GetByDescription(roledesc);
            if (role == null) return null;
            else return role;
        }

    }
}