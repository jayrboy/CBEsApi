using System;
using System.Collections.Generic;

namespace CBEsApi.Models;

public partial class CbesWithSubSupervisorLog
{
    public int Id { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public int? UserId { get; set; }

    public int? CbesLogHeaderId { get; set; }
}
