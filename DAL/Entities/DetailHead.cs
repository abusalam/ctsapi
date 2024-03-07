using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("detail_head", Schema = "master")]
public partial class DetailHead
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

    [InverseProperty("DetailHead")]
    public virtual ICollection<SubDetailHead> SubDetailHeads { get; set; } = new List<SubDetailHead>();
}
