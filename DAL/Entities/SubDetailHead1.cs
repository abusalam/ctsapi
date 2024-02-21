using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("sub_detail_head", Schema = "master")]
public partial class SubDetailHead1
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

    [Column("detail_head_id")]
    public short? DetailHeadId { get; set; }

    [ForeignKey("DetailHeadId")]
    [InverseProperty("SubDetailHead1s")]
    public virtual DetailHead? DetailHead { get; set; }
}
