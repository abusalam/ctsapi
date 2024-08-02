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
[Index("TreasuryCode", "PpoId", "BillNo", Name = "ppo_bills_treasury_code_ppo_id_bill_no_key", IsUnique = true)]
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

    [Column("ppo_id")]
    public int PpoId { get; set; }

    [Column("from_date")]
    public DateOnly FromDate { get; set; }

    [Column("to_date")]
    public DateOnly ToDate { get; set; }

    /// <summary>
    /// F - First Bill; R - Regular Bill;
    /// </summary>
    [Column("bill_type")]
    [MaxLength(1)]
    public char BillType { get; set; }

    /// <summary>
    /// BillNo is to identify the treasury bill
    /// </summary>
    [Column("bill_no")]
    [StringLength(100)]
    public string BillNo { get; set; } = null!;

    [Column("bill_date")]
    public DateOnly BillDate { get; set; }

    [Column("treasury_voucher_no")]
    [StringLength(100)]
    public string? TreasuryVoucherNo { get; set; }

    [Column("treasury_voucher_date")]
    public DateOnly? TreasuryVoucherDate { get; set; }

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

    [Column("bill_gross_amount")]
    public int BillGrossAmount { get; set; }

    [Column("bill_net_amount")]
    public int BillNetAmount { get; set; }

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

    [InverseProperty("Bill")]
    public virtual ICollection<PpoBillBreakup> PpoBillBreakups { get; set; } = new List<PpoBillBreakup>();

    [InverseProperty("Bill")]
    public virtual ICollection<PpoBillBytransfer> PpoBillBytransfers { get; set; } = new List<PpoBillBytransfer>();
}
