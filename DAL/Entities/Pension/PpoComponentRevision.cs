using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("ppo_component_revisions", Schema = "cts_pension")]
[Index("PpoId", "TreasuryCode", "BreakupId", "FromDate", Name = "ppo_component_revisions_ppo_id_treasury_code_breakup_id_fro_key", IsUnique = true)]
public partial class PpoComponentRevision
{
    /// <summary>
    /// RevisionId
    /// </summary>
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("ppo_id")]
    public int PpoId { get; set; }

    [Column("breakup_id")]
    public long BreakupId { get; set; }

    /// <summary>
    /// From date is the Date of Commencement of pension of the pensioner
    /// </summary>
    [Column("from_date")]
    public DateOnly FromDate { get; set; }

    /// <summary>
    /// To date (will be null for regular active bills)
    /// </summary>
    [Column("to_date")]
    public DateOnly? ToDate { get; set; }

    /// <summary>
    /// Amount per month is the actual amount paid for the mentioned period
    /// </summary>
    [Column("amount_per_month")]
    public int AmountPerMonth { get; set; }

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

    [ForeignKey("BreakupId")]
    [InverseProperty("PpoComponentRevisions")]
    public virtual Breakup Breakup { get; set; } = null!;

    [InverseProperty("Revision")]
    public virtual ICollection<PpoBillBreakup> PpoBillBreakups { get; set; } = new List<PpoBillBreakup>();
}
