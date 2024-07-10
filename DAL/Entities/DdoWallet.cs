using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("ddo_wallet", Schema = "bantan")]
[Index("ActiveHoaId", "TreasuryCode", Name = "ddo_wallet_active_hoa_id_treasury_code_unkey", IsUnique = true)]
[Index("SaoDdoCode", "DemandNo", "MajorHead", "SubmajorHead", "MinorHead", "PlanStatus", "SchemeHead", "DetailHead", "SubdetailHead", "VotedCharged", Name = "ddo_wallet_un", IsUnique = true)]
public partial class DdoWallet
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("sao_ddo_code")]
    [StringLength(12)]
    public string SaoDdoCode { get; set; } = null!;

    [Column("dept_code")]
    [StringLength(2)]
    public string? DeptCode { get; set; }

    [Column("demand_no")]
    [StringLength(2)]
    public string? DemandNo { get; set; }

    [Column("major_head")]
    [StringLength(4)]
    public string? MajorHead { get; set; }

    [Column("submajor_head")]
    [StringLength(2)]
    public string? SubmajorHead { get; set; }

    [Column("minor_head")]
    [StringLength(3)]
    public string? MinorHead { get; set; }

    [Column("plan_status")]
    [StringLength(2)]
    public string? PlanStatus { get; set; }

    [Column("scheme_head")]
    [StringLength(3)]
    public string? SchemeHead { get; set; }

    [Column("detail_head")]
    [StringLength(2)]
    public string? DetailHead { get; set; }

    [Column("subdetail_head")]
    [StringLength(2)]
    public string? SubdetailHead { get; set; }

    [Column("voted_charged")]
    [MaxLength(1)]
    public char? VotedCharged { get; set; }

    [Column("budget_alloted_amount")]
    [Precision(10, 0)]
    public decimal? BudgetAllotedAmount { get; set; }

    [Column("reappropriated_amount")]
    [Precision(10, 0)]
    public decimal? ReappropriatedAmount { get; set; }

    [Column("augment_amount")]
    [Precision(10, 0)]
    public decimal? AugmentAmount { get; set; }

    [Column("surrender_amount")]
    [Precision(10, 0)]
    public decimal? SurrenderAmount { get; set; }

    [Column("revised_amount")]
    [Precision(10, 0)]
    public decimal? RevisedAmount { get; set; }

    [Column("ceiling_amount")]
    [Precision(10, 0)]
    public decimal? CeilingAmount { get; set; }

    [Column("provisional_released_amount")]
    [Precision(10, 0)]
    public decimal? ProvisionalReleasedAmount { get; set; }

    [Column("actual_released_amount")]
    [Precision(10, 0)]
    public decimal? ActualReleasedAmount { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime UpdatedAt { get; set; }

    [Column("updated_by")]
    public int UpdatedBy { get; set; }

    [Column("active_hoa_id")]
    public long? ActiveHoaId { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [InverseProperty("Wallet")]
    public virtual DdoWalletActual? DdoWalletActual { get; set; }
}
