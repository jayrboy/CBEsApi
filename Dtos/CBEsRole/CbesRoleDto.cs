namespace CBEsApi.Dtos.CBEsRole
{
    public class CbesRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }  // สมมติว่ามีคุณสมบัติ RoleName ใน CbesRole
        public List<UserDto> Users { get; set; }
    }
}