using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("classifications", Schema = "cts_pension")]
[Index("ClassificationName", Name = "classifications_classification_name_key", IsUnique = true)]
public partial class Classification
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("classification_name")]
    [StringLength(100)]
    public string ClassificationName { get; set; } = null!;

    [Column("account_head_id")]
    public long AccountHeadId { get; set; }

    /// <summary>
    /// [PD] P - Payment; D - Deduction;
    /// </summary>
    [Column("due_draw_flag")]
    [MaxLength(1)]
    public char DueDrawFlag { get; set; }

    /// <summary>
    /// [PD] P - Paid; D - Deducted;
    /// </summary>
    [Column("classification_flag")]
    [MaxLength(1)]
    public char ClassificationFlag { get; set; }

    /// <summary>
    /// [Y/N]
    /// </summary>
    [Column("commuted_value_pension")]
    public bool CommutedValuePension { get; set; }

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

    [ForeignKey("AccountHeadId")]
    [InverseProperty("Classifications")]
    public virtual AccountHead AccountHead { get; set; } = null!;

    [InverseProperty("Classification")]
    public virtual ICollection<EppoAmount> EppoAmounts { get; set; } = new List<EppoAmount>();
}
