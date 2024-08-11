using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("breakups", Schema = "cts_pension")]
[Index("ComponentName", Name = "breakups_component_name_key", IsUnique = true)]
public partial class Breakup
{
    /// <summary>
    /// BreakupId
    /// </summary>
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("component_name")]
    [StringLength(100)]
    public string ComponentName { get; set; } = null!;

    /// <summary>
    /// P - Payment; D - Deduction;
    /// </summary>
    [Column("component_type")]
    [MaxLength(1)]
    public char ComponentType { get; set; }

    /// <summary>
    /// Relief Allowed (Yes/No)
    /// </summary>
    [Column("relief_flag")]
    public bool ReliefFlag { get; set; }

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

    [InverseProperty("Breakup")]
    public virtual ICollection<ComponentRate> ComponentRates { get; set; } = new List<ComponentRate>();

    [InverseProperty("Breakup")]
    public virtual ICollection<PpoComponentRevision> PpoComponentRevisions { get; set; } = new List<PpoComponentRevision>();
}
