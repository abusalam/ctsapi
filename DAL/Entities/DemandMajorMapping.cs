using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("demand_major_mapping", Schema = "master")]
public partial class DemandMajorMapping
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("demand_code")]
    [StringLength(2)]
    public string DemandCode { get; set; } = null!;

    [Column("major_head_code")]
    [StringLength(4)]
    public string MajorHeadCode { get; set; } = null!;

    public virtual Department DemandCodeNavigation { get; set; } = null!;

    public virtual MajorHead MajorHeadCodeNavigation { get; set; } = null!;
}
