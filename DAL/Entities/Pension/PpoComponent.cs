using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

[Table("ppo_components", Schema = "cts_pension")]
[Index("PpoId", "TreasuryCode", Name = "ppo_components_ppo_id_treasury_code_key", IsUnique = true)]
public partial class PpoComponent
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

    [Column("component_type")]
    [MaxLength(1)]
    public char ComponentType { get; set; }

    [Column("component_name")]
    [StringLength(100)]
    public string ComponentName { get; set; } = null!;

    [Column("component_rate")]
    public int ComponentRate { get; set; }

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

    [InverseProperty("Component")]
    public virtual ICollection<PpoBillComponent> PpoBillComponents { get; set; } = new List<PpoBillComponent>();
}
