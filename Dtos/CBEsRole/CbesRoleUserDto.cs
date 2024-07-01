using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBEsApi.Dtos.CBEsRole
{
    public class CbesRoleUserDto
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool isDeleted { get; set; }
        public bool isLastDelete { get; set; }
        public int CreateBy { get; set; }
        public int UpdateBy { get; set; }
        public List<UserDto>? Users { get; set; }
    }
}