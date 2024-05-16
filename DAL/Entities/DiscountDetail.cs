using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("discount_details", Schema = "cts_master")]
public partial class DiscountDetail
{
    [Column("denomination_from")]
    [Precision(10, 2)]
    public decimal DenominationFrom { get; set; }

    [Column("denomination_to")]
    [Precision(10, 2)]
    public decimal DenominationTo { get; set; }

    [Column("discount")]
    [Precision(10, 2)]
    public decimal Discount { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public long? UpdatedBy { get; set; }

    [Column("vendor_type_id")]
    public long VendorTypeId { get; set; }

    [Column("stamp_category_id")]
    public long StampCategoryId { get; set; }

    [Key]
    [Column("discount_id")]
    public long DiscountId { get; set; }

    [ForeignKey("StampCategoryId")]
    [InverseProperty("DiscountDetails")]
    public virtual StampCategory StampCategory { get; set; } = null!;

    [ForeignKey("VendorTypeId")]
    [InverseProperty("DiscountDetails")]
    public virtual VendorType VendorType { get; set; } = null!;
}
