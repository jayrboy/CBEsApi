using System;
using System.Collections.Generic;

namespace CBEsApi.Models;

public partial class CbesTargetResultLogHeader
{
    public int Id { get; set; }

    public int? Round { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CbesTargetResultLogTypeId { get; set; }

    public int? CbesProcessTargetLogId { get; set; }

    public int? CbesProcessResultLogId { get; set; }

    public int? CbesId { get; set; }

    public virtual Cbe? Cbes { get; set; }

    public virtual CbesProcessResultLog? CbesProcessResultLog { get; set; }

    public virtual ICollection<CbesProcessResultLog> CbesProcessResultLogs { get; set; } = new List<CbesProcessResultLog>();

    public virtual CbesProcessTargetLog? CbesProcessTargetLog { get; set; }

    public virtual ICollection<CbesProcessTargetLog> CbesProcessTargetLogs { get; set; } = new List<CbesProcessTargetLog>();

    public virtual ICollection<CbesReportFormLog> CbesReportFormLogs { get; set; } = new List<CbesReportFormLog>();

    public virtual CbesTragetResultLogType? CbesTargetResultLogType { get; set; }
}
