using RubricaTelefonicaAziendale.Entities;

namespace RubricaTelefonicaAziendale.Dtos
{
    public class PersonDto
    {
        public String Id { get; set; } = String.Empty;
        public String? Firstname { get; set; }
        public String? Lastname { get; set; }
        public String? Picture { get; set; }
        public Boolean IsEmployee { get; set; }
        public Boolean IsCustomer { get; set; }
        public Boolean IsPartner { get; set; }
        public List<ContactsDto> ContactsList { get; set; } = [];
        public List<GroupsDto> GroupsList { get; set; } = [];

        public static PersonDto ConvertToDto(People obj)
        {
            PersonDto dto = new()
            {
                Id = obj?.Id.ToString() ?? "",
                Firstname = obj?.Firstname,
                Lastname = obj?.Lastname,
                Picture = obj?.Picture ?? Common.DefaultPersonPicture,
                IsEmployee = obj?.IsEmployee == true,
                IsCustomer = obj?.IsCustomer == true,
                IsPartner = obj?.IsPartner == true,
            };
            foreach(Contacts c in (obj?.Contact ?? new List<Contacts>()))
            {
                dto.ContactsList.Add(ContactsDto.ConvertToDto(c));
            }
            foreach(Groups g in (obj?.Group ?? new List<Groups>()))
            {
                dto.GroupsList.Add(GroupsDto.ConvertToDto(g));
            }
            return dto;
        }

    }
}