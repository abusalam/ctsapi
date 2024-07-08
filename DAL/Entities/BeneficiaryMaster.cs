using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("beneficiary_master", Schema = "epradan")]
public partial class BeneficiaryMaster
{
    [Column("acc_no")]
    [StringLength(18)]
    public string? AccNo { get; set; }

    [Column("account_type")]
    public int? AccountType { get; set; }

    [Column("aadhaar_no")]
    [StringLength(14)]
    public string? AadhaarNo { get; set; }

    [Column("beneficiary_address", TypeName = "character varying")]
    public string? BeneficiaryAddress { get; set; }

    [Column("bank_code")]
    public int? BankCode { get; set; }

    [Column("beneficiary_id_ext")]
    [StringLength(20)]
    public string? BeneficiaryIdExt { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by_userid")]
    public int? CreatedByUserid { get; set; }

    [Column("dob")]
    public DateOnly? Dob { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("emp_type")]
    public int? EmpType { get; set; }

    [Column("gpf_no")]
    [StringLength(15)]
    public string? GpfNo { get; set; }

    [Column("gpf_pran_number")]
    [StringLength(50)]
    public string? GpfPranNumber { get; set; }

    [Column("group_id")]
    public int? GroupId { get; set; }

    [Column("gst_tan")]
    [StringLength(50)]
    public string? GstTan { get; set; }

    [Column("hrms_id")]
    [StringLength(20)]
    public string? HrmsId { get; set; }

    [Column("ifsc_code")]
    [StringLength(11)]
    public string? IfscCode { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("is_aadhaar_verified")]
    public bool? IsAadhaarVerified { get; set; }

    [Column("is_court_case")]
    public bool? IsCourtCase { get; set; }

    [Column("is_death_case")]
    public bool? IsDeathCase { get; set; }

    [Column("mobile_no")]
    [StringLength(10)]
    public string? MobileNo { get; set; }

    [Column("pan_no")]
    [StringLength(10)]
    public string? PanNo { get; set; }

    [Column("beneficiary_name", TypeName = "character varying")]
    public string? BeneficiaryName { get; set; }

    [Column("beneficiary_type")]
    public int? BeneficiaryType { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("trsy_code")]
    [StringLength(3)]
    public string? TrsyCode { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by_userid")]
    public int? UpdatedByUserid { get; set; }

    [Column("vendor_gstin")]
    [StringLength(50)]
    public string? VendorGstin { get; set; }

    [Column("verified_by_ddo")]
    [StringLength(9)]
    public string? VerifiedByDdo { get; set; }

    [Column("ldg_block/wbb", TypeName = "character varying")]
    public string? LdgBlockWbb { get; set; }

    [Column("lgd_municipality/ward", TypeName = "character varying")]
    public string? LgdMunicipalityWard { get; set; }

    [Column("lgd_district", TypeName = "character varying")]
    public string? LgdDistrict { get; set; }

    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("is_global")]
    public bool IsGlobal { get; set; }

    [ForeignKey("BankCode")]
    [InverseProperty("BeneficiaryMasters")]
    public virtual BankMaster? BankCodeNavigation { get; set; }

    [ForeignKey("BeneficiaryType")]
    [InverseProperty("BeneficiaryMasters")]
    public virtual BeneficiaryType1? BeneficiaryTypeNavigation { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("BeneficiaryMasters")]
    public virtual GroupType? Group { get; set; }

    [ForeignKey("IfscCode")]
    [InverseProperty("BeneficiaryMasters")]
    public virtual RbiIfscStock? IfscCodeNavigation { get; set; }

    public virtual Ddo? VerifiedByDdoNavigation { get; set; }
}
