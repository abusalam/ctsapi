using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
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

    [Column("receipt_id")]
    public long ReceiptId { get; set; }

    [Column("ppo_id")]
    public int PpoId { get; set; }

    [Column("ppo_no")]
    [StringLength(100)]
    public string PpoNo { get; set; } = null!;

    /// <summary>
    /// P - Pension; F - Family Pension; C - CPF;
    /// </summary>
    [Column("ppo_type")]
    [MaxLength(1)]
    public char PpoType { get; set; }

    /// <summary>
    /// E - Employed; L - Widow Daughter; U - Unmarried Daughter; V - Divorced Daughter; N - Minor Son; R - Minor Daughter; P - Handicapped Son; G - Handicapped Daughter; J - Dependent Father; K - Dependent Mother; H - Husband; W - Wife;
    /// </summary>
    [Column("ppo_sub_type")]
    [MaxLength(1)]
    public char PpoSubType { get; set; }

    [Column("category_id")]
    public long CategoryId { get; set; }

    [Column("pensioner_name")]
    [StringLength(100)]
    public string PensionerName { get; set; } = null!;

    [Column("date_of_birth")]
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// M - Male; F - Female;
    /// </summary>
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

    /// <summary>
    /// H - Hindu; M - Muslim; O - Other;
    /// </summary>
    [Column("religion")]
    [MaxLength(1)]
    public char Religion { get; set; }

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
    public bool ActiveFlag { get; set; }

    [InverseProperty("Pensioner")]
    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Pensioners")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Pensioner")]
    public virtual ICollection<LifeCertificate> LifeCertificates { get; set; } = new List<LifeCertificate>();

    [InverseProperty("Pensioner")]
    public virtual ICollection<Nominee> Nominees { get; set; } = new List<Nominee>();

    [ForeignKey("PhotoFileId")]
    [InverseProperty("PensionerPhotoFiles")]
    public virtual UploadedFile? PhotoFile { get; set; }

    [InverseProperty("Pensioner")]
    public virtual ICollection<PpoBill> PpoBills { get; set; } = new List<PpoBill>();

    [InverseProperty("Pensioner")]
    public virtual ICollection<PpoStatusFlag> PpoStatusFlags { get; set; } = new List<PpoStatusFlag>();

    [ForeignKey("ReceiptId")]
    [InverseProperty("Pensioners")]
    public virtual PpoReceipt Receipt { get; set; } = null!;

    [ForeignKey("SignatureFileId")]
    [InverseProperty("PensionerSignatureFiles")]
    public virtual UploadedFile? SignatureFile { get; set; }
}
