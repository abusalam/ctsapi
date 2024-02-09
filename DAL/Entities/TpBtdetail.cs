using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("tp_btdetail", Schema = "billing")]
[Index("BillId", Name = "fki_tp_btdetail_bill_id_fkey")]
[Index("ReferenceNo", Name = "fki_tp_btdetail_reference_no_fkey")]
[Index("TrMasterId", Name = "fki_tp_btdetail_tr_master_id_fkey")]
public partial class TpBtdetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("reference_no", TypeName = "character varying")]
    public string? ReferenceNo { get; set; }

    [Column("bt_serial")]
    public int? BtSerial { get; set; }

    [Column("type", TypeName = "character varying")]
    public string? Type { get; set; }

    [Column("ag_amount")]
    public double? AgAmount { get; set; }

    [Column("treasury_amount")]
    public double? TreasuryAmount { get; set; }

    [Column("total_amount")]
    public double? TotalAmount { get; set; }

    [Column("hoa", TypeName = "character varying")]
    public string? Hoa { get; set; }

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

    [Column("tr_master_id")]
    public int? TrMasterId { get; set; }

    [Column("ddo_code")]
    [StringLength(9)]
    public string? DdoCode { get; set; }

    [Column("bill_id")]
    public long? BillId { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("TpBtdetails")]
    public virtual TpBill? Bill { get; set; }

    [ForeignKey("TrMasterId")]
    [InverseProperty("TpBtdetails")]
    public virtual TrMaster? TrMaster { get; set; }
}
