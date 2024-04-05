using RubricaTelefonicaAziendale.Entities;

namespace RubricaTelefonicaAziendale.Dtos
{
    public class PeopleDto
    {
        public String Id { get; set; } = String.Empty;
        public String? Firstname { get; set; }
        public String? Lastname { get; set; }
        public String? Picture { get; set; }
        public Boolean IsEmployee { get; set; }
        public Boolean IsCustomer { get; set; }
        public Boolean IsPartner { get; set; }
        public String? PhoneNumber { get; set; }
        public String? Email { get; set; }
        public String? Address { get; set; }
        public String? SocialAccount { get; set; }
        public String? Groups { get; set; }


        public static PeopleDto ConvertToDto(People obj)
        {
            return new PeopleDto()
            {
                Id = obj?.Id.ToString() ?? "",
                Firstname = obj?.Firstname,
                Lastname = obj?.Lastname,
                Picture = obj?.Picture ?? Common.DefaultPeoplePicture,
                IsEmployee = obj?.IsEmployee == true,
                IsCustomer = obj?.IsCustomer == true,
                IsPartner = obj?.IsPartner == true,
                PhoneNumber = obj?.Contact?.FirstOrDefault(x => x.ContactType.Type.ToUpper().Contains("PHONE"))?.ToString(),
                Email = obj?.Contact?.FirstOrDefault(x => x.ContactType.Type.ToUpper().Contains("EMAIL"))?.ToString(),
                Address = obj?.Contact?.FirstOrDefault(x => x.ContactType.Type.ToUpper().Contains("ADDRESS"))?.ToString(),
                SocialAccount = obj?.Contact?.FirstOrDefault(x => x.ContactType.Type.ToUpper().Contains("ACCOUNT"))?.ToString(),
                Groups = String.Join(", ", obj?.Group ?? new List<Groups>())
            };
        }

        public static People ConvertToEntity(PeopleDto obj, People dbobj)
        {
            dbobj.Firstname = obj.Firstname ?? String.Empty;
            dbobj.Lastname = obj.Lastname ?? String.Empty;
            dbobj.Picture = obj.Picture;
            dbobj.IsEmployee = obj.IsEmployee;
            dbobj.IsCustomer = obj.IsCustomer;
            dbobj.IsPartner = obj.IsPartner;
            return dbobj;
        }
    }

}