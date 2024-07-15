using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("mm_gen_stamp", Schema = "cts_master")]
public partial class MmGenStamp
{
    [Column("stamp_id")]
    [Precision(5, 0)]
    public decimal? StampId { get; set; }

    [Column("int_treasury_code")]
    [StringLength(5)]
    public string? IntTreasuryCode { get; set; }

    [Column("stamp_category_id")]
    [Precision(5, 0)]
    public decimal? StampCategoryId { get; set; }

    [Column("denomination_id")]
    [Precision(3, 0)]
    public decimal? DenominationId { get; set; }

    [Column("label_id")]
    [Precision(3, 0)]
    public decimal? LabelId { get; set; }

    [Column("active_flag")]
    [StringLength(1)]
    public string? ActiveFlag { get; set; }

    [Column("created_user_id")]
    [Precision(8, 0)]
    public decimal? CreatedUserId { get; set; }

    [Column("created_timestamp")]
    public DateOnly CreatedTimestamp { get; set; }

    [Column("modified_user_id")]
    [Precision(8, 0)]
    public decimal? ModifiedUserId { get; set; }

    [Column("modified_timestamp")]
    public DateOnly ModifiedTimestamp { get; set; }
}
