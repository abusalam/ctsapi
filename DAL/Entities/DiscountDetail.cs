using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("discount_details", Schema = "cts_master")]
public partial class DiscountDetail
{
    [Key]
    [Column("discount_id")]
    public long DiscountId { get; set; }

    [Column("vendor_type")]
    public long VendorType { get; set; }

    [Column("stamp_category")]
    [StringLength(2)]
    public string StampCategory { get; set; } = null!;

    [Column("denomination_from")]
    [Precision(10, 2)]
    public decimal DenominationFrom { get; set; }

    [Column("denomination_to")]
    [Precision(10, 2)]
    public decimal DenominationTo { get; set; }

    [Column("discount")]
    [Precision(10, 2)]
    public decimal Discount { get; set; }

    [Required]
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

    [ForeignKey("VendorType")]
    [InverseProperty("DiscountDetails")]
    public virtual StampVendorType VendorTypeNavigation { get; set; } = null!;
}
