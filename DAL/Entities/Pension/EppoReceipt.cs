using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("eppo_receipts", Schema = "cts_pension")]
[Index("PensionApplnNo", Name = "eppo_receipts_pension_appln_no_key", IsUnique = true)]
public partial class EppoReceipt
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year")]
    public int FinancialYear { get; set; }

    [Column("treasury_code")]
    [StringLength(5)]
    public string TreasuryCode { get; set; } = null!;

    [Column("ppo_id")]
    public int? PpoId { get; set; }

    [Column("pension_appln_no")]
    [StringLength(100)]
    public string PensionApplnNo { get; set; } = null!;

    [Column("fresh_revision_flag")]
    [MaxLength(1)]
    public char FreshRevisionFlag { get; set; }

    [Column("ppo_type_code")]
    [MaxLength(1)]
    public char PpoTypeCode { get; set; }

    [Column("ppo_no")]
    [StringLength(100)]
    public string PpoNo { get; set; } = null!;

    [Column("issuing_letter_no")]
    [StringLength(500)]
    public string? IssuingLetterNo { get; set; }

    [Column("issuing_letter_date")]
    public DateOnly? IssuingLetterDate { get; set; }

    [Column("pen_cat_id")]
    public int PenCatId { get; set; }

    [Column("sanction_authority")]
    [StringLength(500)]
    public string SanctionAuthority { get; set; } = null!;

    [Column("sanction_no")]
    [StringLength(500)]
    public string SanctionNo { get; set; } = null!;

    [Column("sanction_date")]
    public DateOnly SanctionDate { get; set; }

    [Column("provisional_pension_status")]
    [MaxLength(1)]
    public char ProvisionalPensionStatus { get; set; }

    [Column("pensioner_name")]
    [StringLength(100)]
    public string PensionerName { get; set; } = null!;

    [Column("religion")]
    [MaxLength(1)]
    public char Religion { get; set; }

    [Column("pensioner_address")]
    [StringLength(500)]
    public string? PensionerAddress { get; set; }

    [Column("mobile_number")]
    [StringLength(10)]
    public string? MobileNumber { get; set; }

    [Column("aadhaar_no")]
    [StringLength(12)]
    public string? AadhaarNo { get; set; }

    [Column("date_of_birth")]
    public DateOnly DateOfBirth { get; set; }

    [Column("date_of_retirement")]
    public DateOnly DateOfRetirement { get; set; }

    [Column("date_of_death")]
    public DateOnly? DateOfDeath { get; set; }

    [Column("qualifying_service_gross_years")]
    public int? QualifyingServiceGrossYears { get; set; }

    [Column("qualifying_service_gross_months")]
    public int? QualifyingServiceGrossMonths { get; set; }

    [Column("qualifying_service_gross_days")]
    public int? QualifyingServiceGrossDays { get; set; }

    [Column("employee_last_pay")]
    public int? EmployeeLastPay { get; set; }

    [Column("employee_last_pay_notional")]
    public int? EmployeeLastPayNotional { get; set; }

    [Column("commuted_pension_amount")]
    public int CommutedPensionAmount { get; set; }

    [Column("withdrawn")]
    public bool? Withdrawn { get; set; }

    [Column("withdraw_date")]
    public DateOnly? WithdrawDate { get; set; }

    [Column("withdraw_reason")]
    [StringLength(500)]
    public string? WithdrawReason { get; set; }

    [Column("photo_file_id")]
    public long? PhotoFileId { get; set; }

    [Column("signature_file_id")]
    public long? SignatureFileId { get; set; }

    [Column("eppo_file_id")]
    public long? EppoFileId { get; set; }

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

    [InverseProperty("EppoReceipt")]
    public virtual ICollection<EppoAmount> EppoAmounts { get; set; } = new List<EppoAmount>();

    [ForeignKey("EppoFileId")]
    [InverseProperty("EppoReceiptEppoFiles")]
    public virtual UploadedFile? EppoFile { get; set; }

    [InverseProperty("EppoReceipt")]
    public virtual ICollection<EppoNominee> EppoNominees { get; set; } = new List<EppoNominee>();

    [ForeignKey("PhotoFileId")]
    [InverseProperty("EppoReceiptPhotoFiles")]
    public virtual UploadedFile? PhotoFile { get; set; }

    [ForeignKey("SignatureFileId")]
    [InverseProperty("EppoReceiptSignatureFiles")]
    public virtual UploadedFile? SignatureFile { get; set; }
}
