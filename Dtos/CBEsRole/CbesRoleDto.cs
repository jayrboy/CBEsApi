using CBEsApi.Dtos.CBEsRole;

namespace CBEsApi.Dtos.CBEsRole
{
    public class CbesRoleDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<UserDto>? Users { get; set; }
        public List<PermissionDto>? Permissions { get; set; }
    }
}