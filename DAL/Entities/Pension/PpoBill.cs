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

    [Column("bank_account_id")]
    public long BankAccountId { get; set; }

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

    [ForeignKey("BankAccountId")]
    [InverseProperty("PpoBills")]
    public virtual BankAccount BankAccount { get; set; } = null!;

    [ForeignKey("BillId")]
    [InverseProperty("PpoBills")]
    public virtual Bill Bill { get; set; } = null!;

    [InverseProperty("PpoBill")]
    public virtual ICollection<Bytransfer> Bytransfers { get; set; } = new List<Bytransfer>();

    [ForeignKey("PensionerId")]
    [InverseProperty("PpoBills")]
    public virtual Pensioner Pensioner { get; set; } = null!;

    [InverseProperty("PpoBill")]
    public virtual ICollection<PpoBillBreakup> PpoBillBreakups { get; set; } = new List<PpoBillBreakup>();
}
