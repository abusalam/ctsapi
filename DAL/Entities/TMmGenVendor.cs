using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("t_mm_gen_vendor", Schema = "cts_master")]
public partial class TMmGenVendor
{
    [Column("int_vendor_id")]
    [Precision(6, 0)]
    public decimal? IntVendorId { get; set; }

    [Column("int_treasury_code")]
    [StringLength(5)]
    public string? IntTreasuryCode { get; set; }

    [Column("vendor_id")]
    [Precision(4, 0)]
    public decimal? VendorId { get; set; }

    [Column("vendor_name")]
    [StringLength(60)]
    public string? VendorName { get; set; }

    [Column("vendor_type")]
    [StringLength(1)]
    public string? VendorType { get; set; }

    [Column("license_no")]
    [StringLength(30)]
    public string? LicenseNo { get; set; }

    [Column("pan")]
    [StringLength(10)]
    public string? Pan { get; set; }

    [Column("effective_from")]
    public DateOnly? EffectiveFrom { get; set; }

    [Column("valid_upto")]
    public DateOnly? ValidUpto { get; set; }

    [Column("address")]
    [StringLength(200)]
    public string? Address { get; set; }

    [Column("phone_no")]
    [StringLength(30)]
    public string? PhoneNo { get; set; }

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
