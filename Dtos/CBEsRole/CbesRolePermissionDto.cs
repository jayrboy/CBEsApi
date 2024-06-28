
namespace CBEsApi.Dtos.CBEsRole
{
    public class CbesRolePermissionDto
    {
        public string? Name { get; set; }
        public List<PermissionDto>? Permissions { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
    }
}