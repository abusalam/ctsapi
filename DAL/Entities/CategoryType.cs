using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("category_type", Schema = "cts_master")]
public partial class CategoryType
{
    [Key]
    [Column("category_type_id")]
    public long CategoryTypeId { get; set; }

    [Column("stamp_category")]
    [StringLength(2)]
    public string StampCategory { get; set; } = null!;

    [Column("created_at")]
    public TimeOnly? CreatedAt { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("updated_at")]
    public TimeOnly? UpdatedAt { get; set; }

    [Column("updated_by")]
    public long? UpdatedBy { get; set; }
}
