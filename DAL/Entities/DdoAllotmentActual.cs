using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("ddo_allotment_actual", Schema = "cts")]
public partial class DdoAllotmentActual
{
    [Key]
    [Column("allotment_id")]
    public long AllotmentId { get; set; }

    [Column("active_hoa_id")]
    public long? ActiveHoaId { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("sao_ddo_code")]
    [StringLength(12)]
    public string SaoDdoCode { get; set; } = null!;

    [Column("actual_released_amount")]
    [Precision(10, 0)]
    public decimal? ActualReleasedAmount { get; set; }

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

    [Column("in_process_amount")]
    [Precision(10, 2)]
    public decimal? InProcessAmount { get; set; }

    [InverseProperty("Allotment")]
    public virtual ICollection<DdoAllotmentBookedBill> DdoAllotmentBookedBills { get; set; } = new List<DdoAllotmentBookedBill>();

    [ForeignKey("FinancialYear")]
    [InverseProperty("DdoAllotmentActuals")]
    public virtual FinancialYearMaster FinancialYearNavigation { get; set; } = null!;

    public virtual Ddo SaoDdoCodeNavigation { get; set; } = null!;

    public virtual Treasury TreasuryCodeNavigation { get; set; } = null!;
}
