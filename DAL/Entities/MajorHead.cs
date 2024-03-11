using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("major_head", Schema = "master")]
[Index("Code", Name = "UK_major_head_code", IsUnique = true)]
public partial class MajorHead
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("code")]
    [StringLength(4)]
    public string Code { get; set; } = null!;

    [Column("name")]
    [StringLength(150)]
    public string? Name { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
}
