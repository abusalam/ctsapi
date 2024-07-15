using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("md_stamp_subcategory", Schema = "cts_master")]
public partial class MdStampSubcategory
{
    [Column("stamp_subcategory_id")]
    [Precision(5, 0)]
    public decimal? StampSubcategoryId { get; set; }

    [Column("stamp_category")]
    [StringLength(2)]
    public string? StampCategory { get; set; }

    [Column("stamp_subcategory")]
    [StringLength(2)]
    public string? StampSubcategory { get; set; }

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
