using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("bill_subdetail_info", Schema = "billing")]
[Index("TreasuryCode", "ActiveHoaId", Name = "bill_subdetail_info_treasury_code_active_hoa_id_key", IsUnique = true)]
[Index("ActiveHoaId", "TreasuryCode", Name = "fki_ddo_wallet_active_hoa_id_treasury_code_fkey")]
public partial class BillSubdetailInfo
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("active_hoa_id")]
    public long ActiveHoaId { get; set; }

    [Column("amount")]
    [Precision(10, 2)]
    public decimal Amount { get; set; }

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

    [Column("financial_year")]
    public short FinancialYear { get; set; }

    [Column("ddo_code")]
    [StringLength(9)]
    public string DdoCode { get; set; } = null!;

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [ForeignKey("ActiveHoaId")]
    [InverseProperty("BillSubdetailInfos")]
    public virtual ActiveHoaMst ActiveHoa { get; set; } = null!;

    [ForeignKey("BillId")]
    [InverseProperty("BillSubdetailInfos")]
    public virtual BillDetail Bill { get; set; } = null!;

    public virtual Ddo DdoCodeNavigation { get; set; } = null!;

    public virtual DdoWallet DdoWallet { get; set; } = null!;

    [ForeignKey("FinancialYear")]
    [InverseProperty("BillSubdetailInfos")]
    public virtual FinancialYearMaster FinancialYearNavigation { get; set; } = null!;

    public virtual Treasury TreasuryCodeNavigation { get; set; } = null!;
}
