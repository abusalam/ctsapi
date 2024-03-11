using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("department", Schema = "master")]
[Index("DemandCode", Name = "Uk_demand_code", IsUnique = true)]
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

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
}
