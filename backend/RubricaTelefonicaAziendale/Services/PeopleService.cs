using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RubricaTelefonicaAziendale.Dtos;
using RubricaTelefonicaAziendale.Entities;
using RubricaTelefonicaAziendale.Models;

namespace RubricaTelefonicaAziendale.Services
{
    public interface IPeopleService
    {
        // GET
        Task<ListDto<PeopleDto>> GetListAsync(PeopleListRequest request);
        Task<People?> GetByID(String id);

        // CRUD
        Task<Boolean> Insert(People obj);
        Task<Boolean> Update(People obj);
        Task<Boolean> Delete(String id);
    }

    public class PeopleService : BaseService, IPeopleService
    {
        public PeopleService(TjfChallengeContext dataContext,
                            IHttpContextAccessor httpContextAccessor,
                            IServiceProvider serviceProvider,
                            IOptions<JwtSettings> jwtSettingsOptions)
            : base(dataContext, httpContextAccessor, serviceProvider, jwtSettingsOptions)
        { }

        // GET
        public async Task<ListDto<PeopleDto>> GetListAsync(PeopleListRequest request)
        {
            ListDto<PeopleDto> response = new();
            try
            {
                // executing stored procedure
                // var sp_params = new List<StoredProcedureParams>();
                // string sorting = String.Empty;
                // foreach (SortParams prm in request.Sorting)
                // {
                //     if (prm.Column != "string" && prm.Direction != "string")
                //         sorting = prm.Column + " " + prm.Direction.ToUpper() + ", ";
                // }
                // if (String.IsNullOrEmpty(sorting)) sorting = "Lastname ASC";
                // else sorting = sorting.Substring(sorting.Length - 2);
                // sp_params.Add(new() { ParamName = "@Sorting", ParamValue = sorting, ParamType = DbType.String });

                // request.Firstname = null;
                // request.Lastname = null;
                // request.IsEmployee = null;
                // request.IsCustomer = null;
                // request.IsPartner = null;
                // sp_params.Add(new() { ParamName = "@Firstname", ParamValue = request.Firstname, ParamType = DbType.String });
                // sp_params.Add(new() { ParamName = "@Lastname", ParamValue = request.Lastname, ParamType = DbType.String });
                // sp_params.Add(new() { ParamName = "@IsEmployee", ParamValue = request.IsEmployee, ParamType = DbType.Boolean });
                // sp_params.Add(new() { ParamName = "@IsCustomer", ParamValue = request.IsCustomer, ParamType = DbType.Boolean });
                // sp_params.Add(new() { ParamName = "@IsPartner", ParamValue = request.IsPartner, ParamType = DbType.Boolean });

                // sp_params.Add(new() { ParamName = "@Start", ParamValue = request.Page * request.EntriesPerPage, ParamType = DbType.Int32 });
                // sp_params.Add(new() { ParamName = "@Length", ParamValue = request.EntriesPerPage, ParamType = DbType.Int32 });

                // dynamic results = await sph.ExecuteStoredProcedure<People>("PeopleGetList", sp_params);

                if (request.EntriesPerPage == 0) request.EntriesPerPage = 10;
                int start = request.Page * request.EntriesPerPage;
                int length = (request.Page + 1) * request.EntriesPerPage;



                dynamic results = await base.db.People.Include(x=>x.Contact).ThenInclude(x=>x.ContactType)
                                                        .Include(x=>x.Group)
                                                        .Skip(start).Take(length)
                                                        .ToListAsync();
                List<PeopleDto> dtos = new List<PeopleDto>();
                foreach (People p in results)
                {
                    dtos.Add(PeopleDto.ConvertToDto(p));
                };
                // filling response
                response.RecordsTotal = await db.People.CountAsync();
                response.RecordsFiltered = results?.Count ?? 0;
                response.Data = dtos ?? new List<PeopleDto>();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return response;
        }
        public async Task<People?> GetByID(String id)
        {
            People? obj = null;
            try
            {
                Guid personid = Guid.Parse(id);
                obj = await this.db.People.Include(x => x.Contact).ThenInclude(x => x.ContactType)
                                            .Include(x => x.Group)
                                            .FirstOrDefaultAsync(x => x.Id == personid);
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return obj;
        }

        // CRUD
        public async Task<Boolean> Insert(People obj)
        {
            int res = -1;
            try
            {
                // obj.InsertTimestamp = DateTime.UtcNow;
                // obj.InsertUser = claims?.UserId;
                await this.db.People.AddAsync(obj);
                res = await this.db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return res > 0;
        }
        public async Task<Boolean> Update(People obj)
        {
            int res = -1;
            try
            {
                // obj.UpdateTimestamp = DateTime.UtcNow;
                // obj.UpdateUser = claims?.UserId;
                this.db.Update(obj);
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
                People? obj = await GetByID(id);
                if (obj == null) return res > 0;
                this.db.Remove(obj);
                res = await this.db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                base.LogException(ex);
            }
            return res > 0;
        }

    }

    public class PeopleListRequest : ListRequest
    {
        public String? Firstname { get; set; }
        public String? Lastname { get; set; }
        public Boolean? IsEmployee { get; set; }
        public Boolean? IsCustomer { get; set; }
        public Boolean? IsPartner { get; set; }
    }
}