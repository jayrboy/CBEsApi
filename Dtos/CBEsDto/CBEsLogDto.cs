using CBEsApi.Models;

namespace CBEsApi.Dtos.CBEsDto
{
    public class CBEsLogDto
    {
        public int Id { get; set; }

        public string? ThaiName { get; set; }

        public string? EngName { get; set; }

        public string? ShortName { get; set; }

        public string? Detail { get; set; }
        public int? Year { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsLastDelete { get; set; }

        public int? UpdateBy { get; set; }

        public int? CbesLogHeader { get; set; }

    }
}