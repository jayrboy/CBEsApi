using CBEsApi.Models;

namespace CBEsApi.Dtos.CBEsDto
{
    public class CBEsDto
    {
        public int Id { get; set; }

        public string? ThaiName { get; set; }

        public string? EngName { get; set; }

        public string? ShortName { get; set; }

        public string? Detail { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsLastDelete { get; set; }

        public int? CreateBy { get; set; }

        public List<CBEsProcessDto> CbesProcesses { get; set; }
    }
}