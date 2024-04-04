using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RubricaTelefonicaAziendale.Entities;
using RubricaTelefonicaAziendale.Models;

namespace RubricaTelefonicaAziendale.Services
{
    public interface IRoleService
    {
        Task<Roles?> GetById(String id);
        Task<Roles?> GetByDescription(String description);
    }

    public class RoleService : BaseService, IRoleService 
    {
        public RoleService(TjfChallengeContext dataContext,
                            IHttpContextAccessor httpContextAccessor,
                            IServiceProvider serviceProvider,
                            IOptions<JwtSettings> jwtSettingsOptions)
            : base(dataContext, httpContextAccessor, serviceProvider, jwtSettingsOptions)
        { }

        public async Task<Roles?> GetById(String id)
        {
            if (this.db.Roles == null) return null;
            return await this.db.Roles.AsNoTracking()
                                        .Where(c => c.Id == id)
                                        .FirstOrDefaultAsync();
        }

        public async Task<Roles?> GetByDescription(String description)
        {
            if (this.db.Roles == null) return null;
            return await this.db.Roles.AsNoTracking()
                                        .Where(c => c.Description == description)
                                        .FirstOrDefaultAsync();
        }

    }
}