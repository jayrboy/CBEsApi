using CBEsApi.Models;

namespace CBEsApi.Dtos.CBEsRoleDto
{
    public class CbesUserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public bool? IsDeleted { get; set; }
        public int PositionId { get; set; }

    }
}