using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("mm_stamp_discount", Schema = "cts_master")]
public partial class MmStampDiscount
{
    [Column("int_discount_id")]
    [Precision(5, 0)]
    public decimal? IntDiscountId { get; set; }

    [Column("vendor_type")]
    [StringLength(1)]
    public string? VendorType { get; set; }

    [Column("int_stamp_category_id")]
    [Precision(5, 0)]
    public decimal? IntStampCategoryId { get; set; }

    [Column("denomination_from")]
    [Precision(3, 0)]
    public decimal? DenominationFrom { get; set; }

    [Column("denomination_to")]
    [Precision(3, 0)]
    public decimal? DenominationTo { get; set; }

    [Column("discount")]
    [Precision(6, 2)]
    public decimal? Discount { get; set; }

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
