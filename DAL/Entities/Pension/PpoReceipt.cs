using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

[Table("ppo_receipts", Schema = "cts_pension")]
[Index("TreasuryReceiptNo", Name = "ppo_receipts_treasury_receipt_no_key", IsUnique = true)]
public partial class PpoReceipt
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year")]
    public int FinancialYear { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("treasury_receipt_no")]
    [StringLength(100)]
    public string TreasuryReceiptNo { get; set; } = null!;

    [Column("ppo_no")]
    [StringLength(100)]
    public string PpoNo { get; set; } = null!;

    [Column("pensioner_name")]
    [StringLength(100)]
    public string? PensionerName { get; set; }

    [Column("date_of_commencement")]
    public DateOnly DateOfCommencement { get; set; }

    [Column("mobile_number")]
    [StringLength(10)]
    public string? MobileNumber { get; set; }

    [Column("receipt_date")]
    public DateOnly ReceiptDate { get; set; }

    [Column("psa_code")]
    [MaxLength(1)]
    public char PsaCode { get; set; }

    [Column("ppo_type")]
    [MaxLength(1)]
    public char PpoType { get; set; }

    [Column("ppo_status")]
    [StringLength(100)]
    public string PpoStatus { get; set; } = null!;

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
}
