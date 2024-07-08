namespace CBEsApi.Dtos.CBEsRole
{
    public class CbesRoleUserDto
    {
        public int ID { get; set; }
        public bool? IsDeleted { get; set; }
        public UserDto? Users { get; set; }
    }
}