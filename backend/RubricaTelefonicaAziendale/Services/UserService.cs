using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RubricaTelefonicaAziendale.Entities;
using RubricaTelefonicaAziendale.Models;

namespace RubricaTelefonicaAziendale.Services
{
    public interface IUserService
    {
        // GET
        Task<Users?> GetByID(String id);
        Task<Users?> GetByUsername(String username);

        // CRUD
        Task<Boolean> Insert(Users obj);
        Task<Boolean> Update(Users obj);
        Task<Boolean> Delete(String id);
    }

    public class UserService : BaseService, IUserService
    {
        public UserService(TjfChallengeContext dataContext,
                                        IHttpContextAccessor httpContextAccessor,
                                        IServiceProvider serviceProvider,
                                        IOptions<JwtSettings> jwtSettingsOptions)
            : base(dataContext, httpContextAccessor, serviceProvider, jwtSettingsOptions)
        { }

        // GET
        public async Task<Users?> GetByID(String id)
        {
            Users? obj = null;
            try
            {
                obj = await this.db.Users.FindAsync(id.ToLower());
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return obj;
        }
        public async Task<Users?> GetByUsername(String username)
        {
            Users? obj = null;
            try
            {
                obj = await this.db.Users.AsNoTracking()
                                            .Where(c => c.Username == username)
                                            .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return obj;
        }

        // CRUD
        public async Task<Boolean> Insert(Users user)
        {
            int res = -1;
            try
            {
                await this.db.Users.AddAsync(user);
                res = await this.db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return res > 0;
        }
        public async Task<Boolean> Update(Users user)
        {
            int res = -1;
            try
            {
                this.db.Update(user);
                res = await this.db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return res > 0;
        }
        public async Task<Boolean> Delete(String id)
        {
            int res = -1;
            try
            {
                Users? user = await GetByID(id);
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

    }

}