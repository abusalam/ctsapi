using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("financial_year_master", Schema = "master")]
public partial class FinancialYearMaster
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("financial_year")]
    [StringLength(9)]
    public string FinancialYear { get; set; } = null!;

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("created_by_userid")]
    public long? CreatedByUserid { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_by_userid")]
    public long? UpdatedByUserid { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("FinancialYearNavigation")]
    public virtual ICollection<BillBtdetail> BillBtdetails { get; set; } = new List<BillBtdetail>();

    [InverseProperty("FinancialYearNavigation")]
    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    [InverseProperty("FinancialYearNavigation")]
    public virtual ICollection<BillSubdetailInfo> BillSubdetailInfos { get; set; } = new List<BillSubdetailInfo>();

    [InverseProperty("FinancialYearNavigation")]
    public virtual ICollection<DdoAllotmentActual> DdoAllotmentActuals { get; set; } = new List<DdoAllotmentActual>();

    [InverseProperty("FinancialYearNavigation")]
    public virtual ICollection<DdoAllotmentBookedBill> DdoAllotmentBookedBills { get; set; } = new List<DdoAllotmentBookedBill>();

    [InverseProperty("FinancialYearNavigation")]
    public virtual ICollection<DdoAllotmentProvisional> DdoAllotmentProvisionals { get; set; } = new List<DdoAllotmentProvisional>();

    [InverseProperty("FinancialYearNavigation")]
    public virtual ICollection<DdoWalletActual> DdoWalletActuals { get; set; } = new List<DdoWalletActual>();

    [InverseProperty("FinancialYearNavigation")]
    public virtual ICollection<DdoWalletProvisional> DdoWalletProvisionals { get; set; } = new List<DdoWalletProvisional>();
}
