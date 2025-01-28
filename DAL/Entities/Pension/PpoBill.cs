using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("ppo_bills", Schema = "cts_pension")]
public partial class PpoBill
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year")]
    public int FinancialYear { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("pensioner_id")]
    public long PensionerId { get; set; }

    [Column("ppo_id")]
    public int PpoId { get; set; }

    /// <summary>
    /// F - First Bill; R - Regular Bill;
    /// </summary>
    [Column("bill_type")]
    [MaxLength(1)]
    public char BillType { get; set; }

    /// <summary>
    /// UTRNo to refer to the actual transaction of the payment
    /// </summary>
    [Column("utr_no")]
    [StringLength(100)]
    public string? UtrNo { get; set; }

    /// <summary>
    /// UTRAt timestamp when the UTR is received
    /// </summary>
    [Column("utr_at", TypeName = "timestamp without time zone")]
    public DateTime? UtrAt { get; set; }

    [Column("gross_amount")]
    public int GrossAmount { get; set; }

    [Column("bytransfer_amount")]
    public int BytransferAmount { get; set; }

    [Column("net_amount")]
    public int NetAmount { get; set; }

    [Column("account_holder_name")]
    [StringLength(100)]
    public string AccountHolderName { get; set; } = null!;

    [Column("bank_ac_no")]
    [StringLength(30)]
    public string BankAcNo { get; set; } = null!;

    [Column("ifsc_code")]
    [StringLength(11)]
    public string IfscCode { get; set; } = null!;

    [Column("payment_status")]
    [MaxLength(1)]
    public char PaymentStatus { get; set; }

    [Column("correction_status")]
    [MaxLength(1)]
    public char CorrectionStatus { get; set; }

    [Column("failed_reason")]
    [StringLength(500)]
    public string? FailedReason { get; set; }

    [Column("remarks")]
    [StringLength(500)]
    public string? Remarks { get; set; }

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

    [ForeignKey("BillId")]
    [InverseProperty("PpoBills")]
    public virtual Bill Bill { get; set; } = null!;

    [InverseProperty("PpoBill")]
    public virtual ICollection<BillBytransfer> BillBytransfers { get; set; } = new List<BillBytransfer>();

    [ForeignKey("PensionerId")]
    [InverseProperty("PpoBills")]
    public virtual Pensioner Pensioner { get; set; } = null!;

    [InverseProperty("PpoBill")]
    public virtual ICollection<PpoBillBreakup> PpoBillBreakups { get; set; } = new List<PpoBillBreakup>();

    [InverseProperty("PpoBill")]
    public virtual ICollection<PpoPaidAmount> PpoPaidAmounts { get; set; } = new List<PpoPaidAmount>();
}
