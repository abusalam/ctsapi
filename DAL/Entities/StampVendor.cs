using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_vendor", Schema = "cts_master")]
public partial class StampVendor
{
    [Column("license_no", TypeName = "character varying")]
    public string LicenseNo { get; set; } = null!;

    [Column("address", TypeName = "character varying")]
    public string Address { get; set; } = null!;

    [Column("phone_number")]
    public long? PhoneNumber { get; set; }

    [Column("effective_from")]
    public DateOnly EffectiveFrom { get; set; }

    [Column("valid_upto")]
    public DateOnly ValidUpto { get; set; }

    [Column("pan_number")]
    [StringLength(10)]
    public string PanNumber { get; set; } = null!;

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("active_at_grips")]
    public bool? ActiveAtGrips { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public long? UpdatedBy { get; set; }

    [Key]
    [Column("stamp_vendor_id")]
    public long StampVendorId { get; set; }

    [Column("vendor_type_id")]
    public long VendorTypeId { get; set; }

    [Column("financial_year_table_id")]
    public long? FinancialYearTableId { get; set; }

    [ForeignKey("VendorTypeId")]
    [InverseProperty("StampVendors")]
    public virtual VendorType VendorType { get; set; } = null!;
}
