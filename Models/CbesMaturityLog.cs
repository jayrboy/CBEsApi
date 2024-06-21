using System;
using System.Collections.Generic;

namespace CBEsApi.Models;

public partial class CbesMaturityLog
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Round { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CbesProcessLogId { get; set; }

    public int? CbesMaturityId { get; set; }

    public int? CbesLogHeaderId { get; set; }

    public virtual CbesLogHeader? CbesLogHeader { get; set; }

    public virtual CbesMaturity? CbesMaturity { get; set; }

    public virtual CbesProcessLog? CbesProcessLog { get; set; }
}
