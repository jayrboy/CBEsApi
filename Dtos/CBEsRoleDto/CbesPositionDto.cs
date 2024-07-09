using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBEsApi.Dtos.CBEsRoleDto
{
    public class CbesPositionDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsLastDelete { get; set; }

        public int? CreateBy { get; set; }

        public int? UpdateBy { get; set; }

    }
}