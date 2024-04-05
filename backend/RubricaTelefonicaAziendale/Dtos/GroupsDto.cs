using RubricaTelefonicaAziendale.Entities;

namespace RubricaTelefonicaAziendale.Dtos
{
    public class GroupsDto
    {
        public String Id { get; set; } = String.Empty;
        public String? Name { get; set; }

        public static GroupsDto ConvertToDto(Groups obj)
        {
            return new GroupsDto()
            {
                Id = obj?.Id.ToString() ?? "",
                Name = obj?.Name,
            };
        }
        
    }
}