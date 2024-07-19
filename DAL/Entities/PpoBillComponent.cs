using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("ppo_bill_components", Schema = "cts_pension")]
[Index("PpoId", "TreasuryCode", Name = "ppo_bill_components_ppo_id_treasury_code_key", IsUnique = true)]
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

    [Column("component_id")]
    public long ComponentId { get; set; }

    [Column("component_wef")]
    public DateOnly ComponentWef { get; set; }

    [Column("component_amount")]
    public int ComponentAmount { get; set; }

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

    [ForeignKey("BillId")]
    [InverseProperty("PpoBillComponents")]
    public virtual PpoBill Bill { get; set; } = null!;

    [ForeignKey("ComponentId")]
    [InverseProperty("PpoBillComponents")]
    public virtual PpoComponent Component { get; set; } = null!;
}
