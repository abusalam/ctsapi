using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

[Table("pensioners", Schema = "cts_pension")]
[Index("PpoId", "TreasuryCode", Name = "pensioners_ppo_id_treasury_code_key", IsUnique = true)]
[Index("PpoNo", Name = "pensioners_ppo_no_key", IsUnique = true)]
public partial class Pensioner
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year")]
    public int FinancialYear { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("ppo_id")]
    public int PpoId { get; set; }

    [Column("ppo_no")]
    [StringLength(100)]
    public string PpoNo { get; set; } = null!;

    [Column("ppo_type")]
    [MaxLength(1)]
    public char PpoType { get; set; }

    [Column("ppo_sub_type")]
    [MaxLength(1)]
    public char PpoSubType { get; set; }

    [Column("psa_type")]
    [MaxLength(1)]
    public char PsaType { get; set; }

    [Column("ppo_category")]
    [MaxLength(1)]
    public char PpoCategory { get; set; }

    [Column("ppo_sub_category")]
    [MaxLength(1)]
    public char PpoSubCategory { get; set; }

    [Column("pensioner_name")]
    [StringLength(100)]
    public string PensionerName { get; set; } = null!;

    [Column("date_of_birth")]
    public DateOnly DateOfBirth { get; set; }

    [Column("gender")]
    [MaxLength(1)]
    public char? Gender { get; set; }

    [Column("mobile_number")]
    [StringLength(10)]
    public string? MobileNumber { get; set; }

    [Column("email_id")]
    [StringLength(100)]
    public string? EmailId { get; set; }

    [Column("pensioner_address")]
    [StringLength(500)]
    public string? PensionerAddress { get; set; }

    [Column("identification_mark")]
    [StringLength(100)]
    public string? IdentificationMark { get; set; }

    [Column("pan_no")]
    [StringLength(10)]
    public string? PanNo { get; set; }

    [Column("aadhaar_no")]
    [StringLength(12)]
    public string? AadhaarNo { get; set; }

    [Column("date_of_retirement")]
    public DateOnly DateOfRetirement { get; set; }

    [Column("date_of_commencement")]
    public DateOnly DateOfCommencement { get; set; }

    [Column("basic_pension_amount")]
    public int BasicPensionAmount { get; set; }

    [Column("commuted_pension_amount")]
    public int CommutedPensionAmount { get; set; }

    [Column("enhance_pension_amount")]
    public int EnhancePensionAmount { get; set; }

    [Column("reduced_pension_amount")]
    public int ReducedPensionAmount { get; set; }

    [Column("religion")]
    [MaxLength(1)]
    public char Religion { get; set; }

    [Column("subdivision")]
    [MaxLength(1)]
    public char Subdivision { get; set; }

    [Column("photo_file_id")]
    public long? PhotoFileId { get; set; }

    [Column("signature_file_id")]
    public long? SignatureFileId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [Column("active_flag")]
    public bool? ActiveFlag { get; set; }

    [ForeignKey("PhotoFileId")]
    [InverseProperty("PensionerPhotoFiles")]
    public virtual UploadedFile? PhotoFile { get; set; }

    [ForeignKey("SignatureFileId")]
    [InverseProperty("PensionerSignatureFiles")]
    public virtual UploadedFile? SignatureFile { get; set; }
}
