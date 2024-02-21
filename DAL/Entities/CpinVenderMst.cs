using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cpin_vender_mst", Schema = "master")]
public partial class CpinVenderMst
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("cpinMstId")]
    public long CpinMstId { get; set; }

    [Column("vendorName", TypeName = "character varying")]
    public string? VendorName { get; set; }

    [Column("vendorGstIn")]
    [StringLength(15)]
    public string? VendorGstIn { get; set; }

    [Column("invoiceNo", TypeName = "character varying")]
    public string? InvoiceNo { get; set; }

    [Column("invoiceDate", TypeName = "timestamp without time zone")]
    public DateTime? InvoiceDate { get; set; }

    [Column("invoiceValue")]
    public double? InvoiceValue { get; set; }

    [Column("amountPart1")]
    public double? AmountPart1 { get; set; }

    [Column("amountPart2")]
    public double? AmountPart2 { get; set; }

    [Column("total")]
    public double? Total { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by_userid")]
    public long? CreatedByUserid { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by_userid")]
    public long? UpdatedByUserid { get; set; }

    [Column("eps_id")]
    public long? EpsId { get; set; }

    [ForeignKey("CpinMstId")]
    [InverseProperty("CpinVenderMsts")]
    public virtual CpinMaster CpinMst { get; set; } = null!;
}
