using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_invoice", Schema = "cts")]
public partial class StampInvoice
{
    [Key]
    [Column("stamp_invoice_id")]
    public long StampInvoiceId { get; set; }

    [Column("stamp_indent_id")]
    public long StampIndentId { get; set; }

    [Column("sheet")]
    public short Sheet { get; set; }

    [Column("label")]
    public short Label { get; set; }

    [Column("invoice_number")]
    [StringLength(20)]
    public string InvoiceNumber { get; set; } = null!;

    [Column("invoice_date", TypeName = "timestamp without time zone")]
    public DateTime InvoiceDate { get; set; }

    [Column("amount")]
    [Precision(10, 2)]
    public decimal Amount { get; set; }

    [Column("created_by")]
    public long CreatedBy { get; set; }

    [Column("quantity")]
    public short Quantity { get; set; }

    [ForeignKey("StampIndentId")]
    [InverseProperty("StampInvoices")]
    public virtual StampIndent StampIndent { get; set; } = null!;
}
