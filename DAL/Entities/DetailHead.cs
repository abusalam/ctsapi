using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("detail_head", Schema = "master")]
[Index("Code", Name = "detail_head_code_key", IsUnique = true)]
public partial class DetailHead
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("code")]
    [StringLength(2)]
    public string Code { get; set; } = null!;

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    [InverseProperty("DetailHead")]
    public virtual ICollection<SubDetailHead> SubDetailHeads { get; set; } = new List<SubDetailHead>();
}
