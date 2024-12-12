using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("treasuries", Schema = "cts_pension")]
[Index("TreasuryCode", Name = "treasuries_treasury_code_key", IsUnique = true)]
public partial class Treasury
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("treasury_code")]
    [StringLength(4)]
    public string TreasuryCode { get; set; } = null!;

    [Column("treasury_name")]
    [StringLength(100)]
    public string? TreasuryName { get; set; }

    [Column("district_code")]
    [StringLength(2)]
    public string? DistrictCode { get; set; }

    [Column("treasury_address")]
    [StringLength(200)]
    public string? TreasuryAddress { get; set; }

    [Column("address1")]
    [StringLength(200)]
    public string? Address1 { get; set; }

    [Column("address2")]
    [StringLength(200)]
    public string? Address2 { get; set; }

    [Column("phone_no1")]
    [StringLength(20)]
    public string? PhoneNo1 { get; set; }

    [Column("phone_no2")]
    [StringLength(20)]
    public string? PhoneNo2 { get; set; }

    [Column("fax")]
    [StringLength(20)]
    public string? Fax { get; set; }

    [Column("e_mail")]
    [StringLength(50)]
    public string? EMail { get; set; }

    [Column("pincode")]
    [StringLength(6)]
    public string? Pincode { get; set; }

    [Column("int_treasury_code")]
    [StringLength(5)]
    public string? IntTreasuryCode { get; set; }

    [Column("pension_flag")]
    [MaxLength(1)]
    public char? PensionFlag { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [Column("active_flag")]
    public bool ActiveFlag { get; set; }
}
