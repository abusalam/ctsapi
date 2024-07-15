using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("ddo_allotment_booked_bill", Schema = "billing")]
[Index("ActiveHoaId", Name = "fki_active_hoa_id_bantan_master_hoa_id")]
[Index("AllotmentId", Name = "fki_c")]
public partial class DdoAllotmentBookedBill
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("allotment_id")]
    public long? AllotmentId { get; set; }

    [Column("amount")]
    public double Amount { get; set; }

    [Column("ddo_user_id")]
    public int? DdoUserId { get; set; }

    [Column("ddo_code")]
    [StringLength(9)]
    public string DdoCode { get; set; } = null!;

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

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

    [Column("active_hoa_id")]
    public long ActiveHoaId { get; set; }

    [ForeignKey("ActiveHoaId")]
    [InverseProperty("DdoAllotmentBookedBills")]
    public virtual ActiveHoaMst ActiveHoa { get; set; } = null!;

    [ForeignKey("AllotmentId")]
    [InverseProperty("DdoAllotmentBookedBills")]
    public virtual DdoAllotmentActual? Allotment { get; set; }

    [ForeignKey("AllotmentId")]
    [InverseProperty("DdoAllotmentBookedBills")]
    public virtual DdoAllotmentTransaction? AllotmentNavigation { get; set; }

    public virtual Ddo DdoCodeNavigation { get; set; } = null!;

    [ForeignKey("FinancialYear")]
    [InverseProperty("DdoAllotmentBookedBills")]
    public virtual FinancialYearMaster FinancialYearNavigation { get; set; } = null!;

    public virtual Treasury TreasuryCodeNavigation { get; set; } = null!;
}
