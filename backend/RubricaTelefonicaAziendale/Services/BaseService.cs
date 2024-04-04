using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RubricaTelefonicaAziendale.Entities;
using RubricaTelefonicaAziendale.Models;

namespace RubricaTelefonicaAziendale.Services
{
    public abstract class BaseService
    {
        protected TjfChallengeContext db { get; }
        protected StoredProceduresHandler sph { get; }
        protected HttpContext? http { get; }
        protected IServiceProvider? service { get; }
        protected JwtSettings? jwtSettings { get; }
        protected JwtTokenClaims? claims { get; }
        public BaseService(TjfChallengeContext dataContext,
                            IHttpContextAccessor httpContextAccessor,
                            IServiceProvider serviceProvider,
                            IOptions<JwtSettings> jwtSettingsOptions)
        {
            this.db = dataContext;
            this.sph = new StoredProceduresHandler(dataContext);
            this.http = httpContextAccessor.HttpContext;
            this.service = serviceProvider;
            this.jwtSettings = jwtSettingsOptions.Value;
            this.claims = DecodeToken(http?.Request?.Headers?.Authorization ?? String.Empty);
        }

        private JwtTokenClaims? DecodeToken(String? Token)
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
                JwtTokenClaims jtc = new()
                {
                    UserId = jwtToken?.Claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value,
                    Fullname = jwtToken?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    Username = jwtToken?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = jwtToken?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    Role = jwtToken?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
                return jtc;
            }
            catch
            {
                return null;
            }
        }


        public async void LogEvent(String message, Object? rawdata)
        {
            Logs entry = new()
            {
                UsersId = claims?.UserId ?? "",
                IpAddress = http?.Connection?.RemoteIpAddress?.ToString() ?? http?.Request?.Headers["X-Forwarded-For"].ToString(),
                Endpoint = $"{http?.Request?.Scheme}://{http?.Request?.Host}",
                Message = message,
                RawData = rawdata != null ? JsonConvert.SerializeObject(rawdata) : null,
                Timestamp = DateTime.Now
            };
            await db.Logs.AddAsync(entry);
            await db.SaveChangesAsync();
        }

        public async void LogException(Exception ex)
        {
            Logs entry = new()
            {
                UsersId = claims?.UserId ?? "",
                IpAddress = http?.Connection?.RemoteIpAddress?.ToString() ?? http?.Request?.Headers["X-Forwarded-For"].ToString(),
                Endpoint = $"{http?.Request?.Scheme}://{http?.Request?.Host}",
                Message = "ERROR " + Environment.NewLine + " -- Message: " + ex.Message + Environment.NewLine + " -- Source: " + ex.Source,
                RawData = JsonConvert.SerializeObject(ex),
                Timestamp = DateTime.Now
            };
            await db.Logs.AddAsync(entry);
            await db.SaveChangesAsync();
        }

    }
}