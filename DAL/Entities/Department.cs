using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("department", Schema = "master")]
[Index("DemandCode", Name = "Uk_demand_code", IsUnique = true)]
[Index("DemandCode", Name = "department_demand_code_key", IsUnique = true)]
public partial class Department
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("code")]
    [StringLength(2)]
    public string? Code { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("demand_code")]
    [StringLength(2)]
    public string DemandCode { get; set; } = null!;

    public virtual ICollection<DemandMajorMapping> DemandMajorMappings { get; set; } = new List<DemandMajorMapping>();

    public virtual ICollection<SchemeHead> SchemeHeads { get; set; } = new List<SchemeHead>();

    public virtual ICollection<TpBill> TpBills { get; set; } = new List<TpBill>();
}
