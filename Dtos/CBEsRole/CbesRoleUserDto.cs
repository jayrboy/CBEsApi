using CBEsApi.Dtos.CBEsUser;

namespace CBEsApi.Dtos.CBEsRole
{
    public class CbesRoleUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserDto> Users { get; set; }
    }
}