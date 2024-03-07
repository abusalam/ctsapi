using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("bill_btdetail", Schema = "billing")]
public partial class BillBtdetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("bill_id")]
    public long? BillId { get; set; }

    [Column("bt_serial")]
    public int? BtSerial { get; set; }

    [Column("bt_type")]
    public short? BtType { get; set; }

    [Column("amount")]
    [Precision(10, 2)]
    public decimal? Amount { get; set; }

    [Column("ddo_code")]
    [StringLength(9)]
    public string? DdoCode { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_by")]
    public long? UpdatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("BillBtdetails")]
    public virtual BillDetail? Bill { get; set; }

    public virtual Treasury? TreasuryCodeNavigation { get; set; }
}
