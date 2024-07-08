namespace CBEsApi.Dtos.CBEsRole
{
    public class CbesRoleUserDto
    {
        public int ID { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public UserDto? Users { get; set; } // Change Users from List<UserDto> to UserDto
    }
}