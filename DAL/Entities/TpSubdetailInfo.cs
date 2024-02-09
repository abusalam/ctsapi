using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("TP_subdetailInfo", Schema = "billing")]
[Index("ActiveHoaId", "TreasuryCode", Name = "TP_subdetailInfo_active_hoa_id_treasury_code_key", IsUnique = true)]
[Index("BillId", Name = "fki_tp_subdetailInfo_bill_id_fkey")]
[Index("ActiveHoaId", "TreasuryCode", Name = "hoa_id_treasury_code_idx")]
public partial class TpSubdetailInfo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("demand")]
    [StringLength(2)]
    public string? Demand { get; set; }

    [Column("major_head")]
    [StringLength(4)]
    public string? MajorHead { get; set; }

    [Column("sub_major_head")]
    [StringLength(2)]
    public string? SubMajorHead { get; set; }

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

    [Column("sub_detail_head")]
    [StringLength(2)]
    public string? SubDetailHead { get; set; }

    [Column("voted_charged")]
    [StringLength(1)]
    public string? VotedCharged { get; set; }

    [Column("amount")]
    public double? Amount { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("created_by_userid")]
    public long? CreatedByUserid { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_by_userid")]
    public long? UpdatedByUserid { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("reference_no", TypeName = "character varying")]
    public string? ReferenceNo { get; set; }

    [Column("financial_year", TypeName = "character varying")]
    public string? FinancialYear { get; set; }

    [Column("ddo_code")]
    [StringLength(9)]
    public string? DdoCode { get; set; }

    [Column("active_hoa_id")]
    public long? ActiveHoaId { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("TpSubdetailInfos")]
    public virtual TpBill Bill { get; set; } = null!;

    public virtual DdoWallet? DdoWallet { get; set; }
}
