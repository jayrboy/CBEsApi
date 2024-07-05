namespace CBEsApi.Dtos.CBEsRole
{
    public class CbesUserDto
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Fullname { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DateTime? LoginDate { get; set; }

        public int? LoginFailedCount { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsLog { get; set; }

        public int? CreateBy { get; set; }

        public int? UpdateBy { get; set; }

        public int? PositionId { get; set; }

        public int? UserWithRoleId { get; set; }
    }
}