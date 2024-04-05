using RubricaTelefonicaAziendale.Entities;

namespace RubricaTelefonicaAziendale.Dtos
{
    public class ContactsDto
    {
        public String Id { get; set; } = String.Empty;
        public String? TypeId { get; set; }
        public String? Type { get; set; }
        public String? Contact { get; set; }

        public static ContactsDto ConvertToDto(Contacts obj)
        {
            return new ContactsDto()
            {
                Id = obj?.Id.ToString() ?? "",
                TypeId = (obj?.ContactType?.Id ?? Guid.Empty).ToString(),
                Type = obj?.ContactType?.Type ?? "",
                Contact = obj?.Contact,
            };
        }

    }
}