using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("TP_Bill", Schema = "billing")]
[Index("BillNo", "DdoCode", "FinancialYear", Name = "Uk_bill_no_ddocode_fin_yr", IsUnique = true)]
[Index("ReferenceNo", Name = "Uk_reference_no", IsUnique = true)]
[Index("Demand", Name = "fki_tp_bill_demand_fkey")]
[Index("TrMasterId", Name = "fki_tp_bill_tr_master_id_fkey")]
public partial class TpBill
{
    [Key]
    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("payment_mode")]
    public short? PaymentMode { get; set; }

    [Column("tr_master_id")]
    public int? TrMasterId { get; set; }

    [Column("bill_no")]
    [StringLength(15)]
    public string? BillNo { get; set; }

    [Column("bill_date")]
    public DateOnly? BillDate { get; set; }

    [Column("bill_mode")]
    public short? BillMode { get; set; }

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

    [Column("voted_charged")]
    [StringLength(1)]
    public string? VotedCharged { get; set; }

    [Column("gross_amount")]
    public double? GrossAmount { get; set; }

    [Column("net_amount")]
    public double? NetAmount { get; set; }

    [Column("bt_amount")]
    public double? BtAmount { get; set; }

    [Column("dept_code")]
    [StringLength(2)]
    public string? DeptCode { get; set; }

    [Column("wb_sanction_no", TypeName = "character varying")]
    public string? WbSanctionNo { get; set; }

    [Column("wb_sanction_amt")]
    public double? WbSanctionAmt { get; set; }

    [Column("wb_sanction_date")]
    public DateOnly? WbSanctionDate { get; set; }

    [Column("wb_sanction_by", TypeName = "character varying")]
    public string? WbSanctionBy { get; set; }

    [Column("gem_non_gem")]
    public bool? GemNonGem { get; set; }

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

    [Column("pending_with_user", TypeName = "character varying")]
    public string? PendingWithUser { get; set; }

    [Column("remarks", TypeName = "character varying")]
    public string? Remarks { get; set; }

    [Column("reference_no", TypeName = "character varying")]
    public string? ReferenceNo { get; set; }

    [Column("ddo_code")]
    [StringLength(9)]
    public string? DdoCode { get; set; }

    [Column("ddo_user_id")]
    public int? DdoUserId { get; set; }

    [Column("designation")]
    [StringLength(100)]
    public string? Designation { get; set; }

    [Column("financial_year", TypeName = "character varying")]
    public string? FinancialYear { get; set; }

    [Column("sanction_id_object", TypeName = "jsonb")]
    public string? SanctionIdObject { get; set; }

    [Column("isextendedpartchecked")]
    public short Isextendedpartchecked { get; set; }

    [Column("allotment_id_object", TypeName = "character varying")]
    public string? AllotmentIdObject { get; set; }

    [Column("is_deleted")]
    public short? IsDeleted { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("active_hoa_id")]
    public long? ActiveHoaId { get; set; }

    public virtual Department? DemandNavigation { get; set; }

    [InverseProperty("Bill")]
    public virtual ICollection<TpBtdetail> TpBtdetails { get; set; } = new List<TpBtdetail>();

    [InverseProperty("Bill")]
    public virtual ICollection<TpSubdetailInfo> TpSubdetailInfos { get; set; } = new List<TpSubdetailInfo>();

    [ForeignKey("TrMasterId")]
    [InverseProperty("TpBills")]
    public virtual TrMaster? TrMaster { get; set; }
}
