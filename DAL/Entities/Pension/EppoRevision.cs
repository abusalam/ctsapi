using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("eppo_revisions", Schema = "cts_pension")]
[Index("PensionApplnNo", Name = "eppo_revisions_pension_appln_no_key", IsUnique = true)]
public partial class EppoRevision
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

    [Column("ppo_no")]
    [StringLength(100)]
    public string PpoNo { get; set; } = null!;

    [Column("issuing_letter_no")]
    [StringLength(500)]
    public string? IssuingLetterNo { get; set; }

    [Column("issuing_letter_date")]
    public DateOnly? IssuingLetterDate { get; set; }

    [Column("fresh_revision_flag")]
    [MaxLength(1)]
    public char FreshRevisionFlag { get; set; }

    [Column("ppo_type_code")]
    [MaxLength(1)]
    public char PpoTypeCode { get; set; }

    [Column("ppo_sub_type")]
    [MaxLength(1)]
    public char PpoSubType { get; set; }

    [Column("pen_cat_id")]
    public int PenCatId { get; set; }

    [Column("employee_last_pay")]
    public int? EmployeeLastPay { get; set; }

    [Column("employee_last_pay_notional")]
    public int? EmployeeLastPayNotional { get; set; }

    [Column("commuted_pension_amount")]
    public int CommutedPensionAmount { get; set; }

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

    [ForeignKey("EppoFileId")]
    [InverseProperty("EppoRevisions")]
    public virtual UploadedFile? EppoFile { get; set; }
}
