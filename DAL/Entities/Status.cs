using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("status", Schema = "cts_master")]
public partial class Status
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name", TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [Column("slug", TypeName = "character varying")]
    public string Slug { get; set; } = null!;

    /// <summary>
    /// 1 = token flow ,2 = Cheque indent,3 Cheque invoice, 4 = Cheque Received
    /// </summary>
    [Column("type")]
    public short Type { get; set; }

    [InverseProperty("StatusNavigation")]
    public virtual ICollection<ChequeIndent> ChequeIndents { get; set; } = new List<ChequeIndent>();

    [InverseProperty("StatusNavigation")]
    public virtual ICollection<ChequeInvoice> ChequeInvoices { get; set; } = new List<ChequeInvoice>();

    [InverseProperty("Status")]
    public virtual ICollection<StampIndent> StampIndents { get; set; } = new List<StampIndent>();

    [InverseProperty("Status")]
    public virtual ICollection<TokenFlow> TokenFlows { get; set; } = new List<TokenFlow>();

    [InverseProperty("Status")]
    public virtual ICollection<VendorStampRequisition> VendorStampRequisitions { get; set; } = new List<VendorStampRequisition>();
}
