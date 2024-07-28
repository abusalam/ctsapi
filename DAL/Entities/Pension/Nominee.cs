using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema
/// </summary>
[Table("nominees", Schema = "cts_pension")]
[Index("PpoId", "TreasuryCode", Name = "nominees_ppo_id_treasury_code_key", IsUnique = true)]
public partial class Nominee
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

    [Column("nominee_name")]
    [StringLength(100)]
    public string NomineeName { get; set; } = null!;

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

    [Column("nominee_address")]
    [StringLength(500)]
    public string? NomineeAddress { get; set; }

    [Column("identification_mark")]
    [StringLength(100)]
    public string? IdentificationMark { get; set; }

    [Column("pan_no")]
    [StringLength(10)]
    public string? PanNo { get; set; }

    [Column("aadhaar_no")]
    [StringLength(12)]
    public string? AadhaarNo { get; set; }

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
    [InverseProperty("NomineePhotoFiles")]
    public virtual UploadedFile? PhotoFile { get; set; }

    [ForeignKey("SignatureFileId")]
    [InverseProperty("NomineeSignatureFiles")]
    public virtual UploadedFile? SignatureFile { get; set; }
}
