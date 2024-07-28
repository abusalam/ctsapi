using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema
/// </summary>
[Table("ppo_bill_components", Schema = "cts_pension")]
[Index("TreasuryCode", "PpoId", "BillId", "RateId", Name = "ppo_bill_components_treasury_code_ppo_id_bill_id_rate_id_key", IsUnique = true)]
public partial class PpoBillComponent
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

    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("rate_id")]
    public long RateId { get; set; }

    [Column("from_date")]
    public DateOnly FromDate { get; set; }

    [Column("to_date")]
    public DateOnly ToDate { get; set; }

    [Column("breakup_amount")]
    public int BreakupAmount { get; set; }

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

    [ForeignKey("BillId")]
    [InverseProperty("PpoBillComponents")]
    public virtual PpoBill Bill { get; set; } = null!;

    [ForeignKey("RateId")]
    [InverseProperty("PpoBillComponents")]
    public virtual ComponentRate Rate { get; set; } = null!;
}
