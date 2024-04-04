using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_invoice", Schema = "cts")]
public partial class ChequeInvoice
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("invoice_id")]
    public int? InvoiceId { get; set; }

    [Column("invoice_date")]
    public DateOnly? InvoiceDate { get; set; }

    [Column("invoice_number")]
    public int? InvoiceNumber { get; set; }

    [Column("cheque_indent_id")]
    public long? ChequeIndentId { get; set; }

    [Column("micr_no")]
    [StringLength(9)]
    public string? MicrNo { get; set; }

    [Column("quantity")]
    public short? Quantity { get; set; }
}
