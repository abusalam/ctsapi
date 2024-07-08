using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_vendor", Schema = "cts_master")]
public partial class StampVendor
{
    [Key]
    [Column("stamp_vendor_id")]
    public long StampVendorId { get; set; }

    [Column("financial_year_table_id")]
    public long? FinancialYearTableId { get; set; }

    [Column("license_no", TypeName = "character varying")]
    public string LicenseNo { get; set; } = null!;

    [Column("address", TypeName = "character varying")]
    public string Address { get; set; } = null!;

    [Column("phone_number")]
    public long PhoneNumber { get; set; }

    [Column("effective_from")]
    public DateOnly EffectiveFrom { get; set; }

    [Column("valid_upto")]
    public DateOnly ValidUpto { get; set; }

    [Column("pan_number")]
    [StringLength(10)]
    public string PanNumber { get; set; } = null!;

    [Required]
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

    [Column("vendor_type")]
    public long VendorType { get; set; }

    [Column("vendorPhoto")]
    public string VendorPhoto { get; set; } = null!;

    [Column("vendorPanPhoto")]
    public string VendorPanPhoto { get; set; } = null!;

    [Column("vendorLicencePhoto")]
    public string VendorLicencePhoto { get; set; } = null!;

    [Column("vendor_name", TypeName = "character varying")]
    public string? VendorName { get; set; }

    [InverseProperty("Vendor")]
    public virtual ICollection<VendorStampRequisition> VendorStampRequisitions { get; set; } = new List<VendorStampRequisition>();

    [ForeignKey("VendorType")]
    [InverseProperty("StampVendors")]
    public virtual StampVendorType VendorTypeNavigation { get; set; } = null!;
}
