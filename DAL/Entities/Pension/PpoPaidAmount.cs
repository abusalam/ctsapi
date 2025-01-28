using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("ppo_paid_amounts", Schema = "cts_pension")]
[Index("TreasuryCode", "PpoId", "PpoBillId", "BreakupId", "PaidFromDate", Name = "ppo_paid_amounts_treasury_code_ppo_id_ppo_bill_id_breakup_i_key", IsUnique = true)]
public partial class PpoPaidAmount
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

    /// <summary>
    /// BillId is to identify the bill on which the actual payment made
    /// </summary>
    [Column("ppo_bill_id")]
    public long PpoBillId { get; set; }

    [Column("breakup_id")]
    public long BreakupId { get; set; }

    [Column("paid_from_date")]
    public DateOnly PaidFromDate { get; set; }

    [Column("paid_to_date")]
    public DateOnly PaidToDate { get; set; }

    [Column("breakup_amount")]
    public int BreakupAmount { get; set; }

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

    [ForeignKey("BreakupId")]
    [InverseProperty("PpoPaidAmounts")]
    public virtual Breakup Breakup { get; set; } = null!;

    [ForeignKey("PpoBillId")]
    [InverseProperty("PpoPaidAmounts")]
    public virtual PpoBill PpoBill { get; set; } = null!;
}
