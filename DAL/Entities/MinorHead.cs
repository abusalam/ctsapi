using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("minor_head", Schema = "master")]
public partial class MinorHead
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("code")]
    [StringLength(3)]
    public string? Code { get; set; }

    [Column("name")]
    [StringLength(150)]
    public string? Name { get; set; }

    [Column("sub_major_id")]
    public short? SubMajorId { get; set; }

    [InverseProperty("MinorHead")]
    public virtual ICollection<SchemeHead> SchemeHeads { get; set; } = new List<SchemeHead>();

    [ForeignKey("SubMajorId")]
    [InverseProperty("MinorHeads")]
    public virtual SubMajorHead? SubMajor { get; set; }
}
