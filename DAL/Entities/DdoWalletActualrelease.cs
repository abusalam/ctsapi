using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("ddo_wallet_actualrelease", Schema = "cts")]
[Index("ActiveHoaId", "TreasuryCode", Name = "ddo_wallet_actualrelease_active_hoa_id_treasury_code_key", IsUnique = true)]
[Index("SaoDdoCode", "DemandNo", "MajorHead", "SubmajorHead", "MinorHead", "PlanStatus", "SchemeHead", "DetailHead", "SubdetailHead", "VotedCharged", Name = "ddo_wallet_un", IsUnique = true)]
public partial class DdoWalletActualrelease
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
}
