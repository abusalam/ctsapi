using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("bill_details", Schema = "billing")]
[Index("BillNo", "DdoCode", "FinancialYear", Name = "Uk_bill_no_ddocode_fin_yr", IsUnique = true)]
[Index("ReferenceNo", "VersionNo", Name = "Uk_reference_no", IsUnique = true)]
[Index("Demand", Name = "fki_dept_demand_code_fkey")]
public partial class BillDetail
{
    [Key]
    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("bill_no")]
    [StringLength(15)]
    public string? BillNo { get; set; }

    [Column("bill_date")]
    public DateOnly BillDate { get; set; }

    [Column("bill_mode")]
    public short BillMode { get; set; }

    [Column("reference_no")]
    [StringLength(20)]
    public string? ReferenceNo { get; set; }

    [Column("version_no")]
    public short VersionNo { get; set; }

    [Column("tr_master_id")]
    public short TrMasterId { get; set; }

    [Column("payment_mode")]
    public short PaymentMode { get; set; }

    [Column("financial_year")]
    public short FinancialYear { get; set; }

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
    [MaxLength(1)]
    public char? VotedCharged { get; set; }

    [Column("gross_amount")]
    [Precision(10, 2)]
    public decimal GrossAmount { get; set; }

    [Column("net_amount")]
    [Precision(10, 2)]
    public decimal NetAmount { get; set; }

    [Column("bt_amount")]
    [Precision(10, 2)]
    public decimal? BtAmount { get; set; }

    [Column("sanction_no")]
    public long? SanctionNo { get; set; }

    [Column("sanction_amt")]
    [Precision(10, 2)]
    public decimal? SanctionAmt { get; set; }

    [Column("sanction_date")]
    public DateOnly? SanctionDate { get; set; }

    [Column("sanction_by")]
    [StringLength(100)]
    public string? SanctionBy { get; set; }

    [Column("remarks")]
    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column("ddo_code")]
    [StringLength(9)]
    public string? DdoCode { get; set; }

    [Column("is_extended_part_filled")]
    public bool IsExtendedPartFilled { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("is_gem")]
    public bool IsGem { get; set; }

    [Column("status")]
    public short Status { get; set; }

    [Column("created_by_userid")]
    public long? CreatedByUserid { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_by_userid")]
    public long? UpdatedByUserid { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("form_version")]
    public short FormVersion { get; set; }

    [Column("form_revision_no")]
    public short FormRevisionNo { get; set; }

    [InverseProperty("Bill")]
    public virtual ICollection<BillBtdetail> BillBtdetails { get; set; } = new List<BillBtdetail>();

    [InverseProperty("Bill")]
    public virtual ICollection<BillSubdetailInfo> BillSubdetailInfos { get; set; } = new List<BillSubdetailInfo>();

    public virtual Ddo? DdoCodeNavigation { get; set; }

    public virtual Department? DemandNavigation { get; set; }

    public virtual DetailHead? DetailHeadNavigation { get; set; }

    [ForeignKey("FinancialYear")]
    [InverseProperty("BillDetails")]
    public virtual FinancialYearMaster FinancialYearNavigation { get; set; } = null!;

    public virtual MajorHead? MajorHeadNavigation { get; set; }

    [InverseProperty("Bill")]
    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();

    [ForeignKey("TrMasterId")]
    [InverseProperty("BillDetails")]
    public virtual TrMaster TrMaster { get; set; } = null!;

    public virtual Treasury TreasuryCodeNavigation { get; set; } = null!;
}
