using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("sub_major_head", Schema = "master")]
public partial class SubMajorHead
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("code")]
    [StringLength(2)]
    public string? Code { get; set; }

    [Column("name")]
    [StringLength(150)]
    public string? Name { get; set; }

    [Column("major_head_id")]
    public short? MajorHeadId { get; set; }

    [ForeignKey("MajorHeadId")]
    [InverseProperty("SubMajorHeads")]
    public virtual MajorHead? MajorHead { get; set; }

    [InverseProperty("SubMajor")]
    public virtual ICollection<MinorHead> MinorHeads { get; set; } = new List<MinorHead>();
}
