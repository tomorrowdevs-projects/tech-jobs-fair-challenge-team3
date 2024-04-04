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
        Task<Roles?> GetByUserId(String? id);

        Task<Boolean> SetByUserId(String UserId, String RoleId);
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

        public async Task<Roles?> GetByUserId(String? id)
        {
            if (this.db.UserRoles == null) return null;
            if (this.db.Roles == null) return null;
            if (id == null) return null;
            UserRoles? ur = await this.db.UserRoles.AsNoTracking()
                                                    .Where(c => c.UsersId == id!)
                                                    .FirstOrDefaultAsync();
            if (ur == null) return null;
            return await this.db.Roles.AsNoTracking()
                                        .Where(c => c.Id == ur.RolesId)
                                        .FirstOrDefaultAsync();
        }

        public async Task<Boolean> SetByUserId(String UserId, String RoleId)
        {
            int res = -1;
            try
            {
                UserRoles? ur = await this.db.UserRoles.AsNoTracking()
                                                        .Where(c => c.UsersId == UserId)
                                                        .FirstOrDefaultAsync();
                if (ur != null) this.db.UserRoles.Remove(ur);
                await this.db.UserRoles.AddAsync(new UserRoles() { UsersId = UserId, RolesId = RoleId });
                res = await this.db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return res>0;
        }

    }
}