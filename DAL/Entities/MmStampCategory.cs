using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("mm_stamp_category", Schema = "cts_master")]
public partial class MmStampCategory
{
    [Column("int_stamp_category_id")]
    [Precision(5, 0)]
    public decimal? IntStampCategoryId { get; set; }

    [Column("stamp_category")]
    [StringLength(2)]
    public string? StampCategory { get; set; }

    [Column("description")]
    [StringLength(40)]
    public string? Description { get; set; }

    [Column("active_flag")]
    [StringLength(1)]
    public string? ActiveFlag { get; set; }

    [Column("user_id")]
    [Precision(8, 0)]
    public decimal? UserId { get; set; }

    [Column("created_timestamp")]
    public DateOnly CreatedTimestamp { get; set; }

    [Column("modified_user_id")]
    [Precision(8, 0)]
    public decimal? ModifiedUserId { get; set; }

    [Column("modified_timestamp")]
    public DateOnly ModifiedTimestamp { get; set; }
}
