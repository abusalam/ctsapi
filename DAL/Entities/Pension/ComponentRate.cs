using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("component_rates", Schema = "cts_pension")]
[Index("CategoryId", "BreakupId", "EffectiveFromDate", Name = "component_rates_category_id_breakup_id_effective_from_date_key", IsUnique = true)]
public partial class ComponentRate
{
    /// <summary>
    /// RateId will identify the component rate revised or introduced
    /// </summary>
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("category_id")]
    public long CategoryId { get; set; }

    [Column("breakup_id")]
    public long BreakupId { get; set; }

    /// <summary>
    /// Effective from date the component rate is revised or introduced
    /// </summary>
    [Column("effective_from_date")]
    public DateOnly EffectiveFromDate { get; set; }

    [Column("rate_amount")]
    public int RateAmount { get; set; }

    /// <summary>
    /// P - Percentage; A - Amount;
    /// </summary>
    [Column("rate_type")]
    [MaxLength(1)]
    public char RateType { get; set; }

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
    [InverseProperty("ComponentRates")]
    public virtual Breakup Breakup { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("ComponentRates")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Rate")]
    public virtual ICollection<PpoComponentRevision> PpoComponentRevisions { get; set; } = new List<PpoComponentRevision>();
}
