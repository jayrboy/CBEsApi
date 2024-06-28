using CBEsApi.Dtos.CBEsPermission;

namespace CBEsApi.Dtos.CBEsRole
{
    public class CbesRolePermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}