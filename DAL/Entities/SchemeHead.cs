using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("scheme_head", Schema = "master")]
public partial class SchemeHead
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("demand_code")]
    [StringLength(2)]
    public string DemandCode { get; set; } = null!;

    [Column("code")]
    [StringLength(3)]
    public string Code { get; set; } = null!;

    [Column("name")]
    [StringLength(300)]
    public string? Name { get; set; }

    [Column("minor_head_id")]
    public short MinorHeadId { get; set; }

    public virtual Department DemandCodeNavigation { get; set; } = null!;

    [ForeignKey("MinorHeadId")]
    [InverseProperty("SchemeHeads")]
    public virtual MinorHead MinorHead { get; set; } = null!;
}
