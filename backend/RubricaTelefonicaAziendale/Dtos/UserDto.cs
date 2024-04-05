using RubricaTelefonicaAziendale.Entities;

namespace RubricaTelefonicaAziendale.Dtos
{
    public class UserDto
    {
        public String Id { get; set; } = String.Empty;
        public String? Firstname { get; set; }
        public String? Lastname { get; set; }
        public String Username { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
        public String RoleId { get; set; } = String.Empty;
        public String RoleDesc { get; set; } = String.Empty;
        public String Picture { get; set; } = String.Empty;

        public static UserDto ConvertToDto(Users obj)
        {
            return new UserDto()
            {
                Id = obj?.Id.ToString() ?? "",
                Firstname = obj?.Firstname,
                Lastname = obj?.Lastname,
                Username = obj?.Username ?? "",
                RoleId = obj?.Role?.Id.ToString() ?? "",
                RoleDesc = obj?.Role?.Description ?? String.Empty,
                Picture = obj?.Picture ?? Common.DefaultUserPicture,
            };
        }

        public static Users ConvertToEntity(UserDto obj, Users dbobj)
        {
            dbobj.Firstname = obj.Firstname ?? String.Empty;
            dbobj.Lastname = obj.Lastname ?? String.Empty;
            dbobj.Username = obj.Username;
            dbobj.Password = obj.Password;
            return dbobj;
        }
    }
}