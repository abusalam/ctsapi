using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

[Table("ppo_bills", Schema = "cts_pension")]
[Index("PpoId", "TreasuryCode", Name = "ppo_bills_ppo_id_treasury_code_key", IsUnique = true)]
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

    [Column("paid_from")]
    public DateOnly PaidFrom { get; set; }

    [Column("paid_upto")]
    public DateOnly PaidUpto { get; set; }

    [Column("bill_type")]
    [MaxLength(1)]
    public char BillType { get; set; }

    [Column("bill_no")]
    [StringLength(100)]
    public string BillNo { get; set; } = null!;

    [Column("bill_date")]
    public DateOnly BillDate { get; set; }

    [Column("bill_amount")]
    public int BillAmount { get; set; }

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

    [InverseProperty("Bill")]
    public virtual ICollection<PpoBillBytransfer> PpoBillBytransfers { get; set; } = new List<PpoBillBytransfer>();

    [InverseProperty("Bill")]
    public virtual ICollection<PpoBillComponent> PpoBillComponents { get; set; } = new List<PpoBillComponent>();
}
