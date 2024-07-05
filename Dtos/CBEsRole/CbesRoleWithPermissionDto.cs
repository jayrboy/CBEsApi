namespace CBEsApi.Dtos.CBEsRole
{
    public class CbesRoleWithPermissionDto
    {
        public int Id { get; set; }
        public bool? IsChecked { get; set; }
        public bool? IsDeleted { get; set; }
        public int? RoleId { get; set; }
        public int? PermissionId { get; set; }
        public PermissionDto Permission { get; set; }
    }
}