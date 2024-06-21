using System;
using System.Collections.Generic;

namespace CBEsApi.Models;

public partial class CbesPosition
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsLastDelete { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public virtual ICollection<CbesUser> CbesUsers { get; set; } = new List<CbesUser>();

    public virtual ICollection<CbesWithSubSupervisor> CbesWithSubSupervisors { get; set; } = new List<CbesWithSubSupervisor>();

    public virtual ICollection<CbeswithSupervisor> CbeswithSupervisors { get; set; } = new List<CbeswithSupervisor>();

    public virtual CbesUser? CreateByNavigation { get; set; }

    public virtual ICollection<MaturityWithSupervisor> MaturityWithSupervisors { get; set; } = new List<MaturityWithSupervisor>();

    public virtual CbesUser? UpdateByNavigation { get; set; }
}
