using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_invoice_details", Schema = "cts")]
[Index("ChequeEntryId", Name = "fki_cheque_invoice_details__cheque_entry_id__fkey")]
[Index("ChequeIndentDetailId", Name = "fki_cheque_invoice_details__cheque_indent_detail_id__fkey")]
[Index("ChequeInvoiceId", Name = "fki_cheque_invoice_details__cheque_invoice_id__fkey")]
public partial class ChequeInvoiceDetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("cheque_invoice_id")]
    public long ChequeInvoiceId { get; set; }

    [Column("cheque_indent_detail_id")]
    public long ChequeIndentDetailId { get; set; }

    [Column("start")]
    public short Start { get; set; }

    [Column("end")]
    public short End { get; set; }

    [Column("quantity")]
    public short Quantity { get; set; }

    [Column("cheque_entry_id")]
    public long ChequeEntryId { get; set; }

    [ForeignKey("ChequeEntryId")]
    [InverseProperty("ChequeInvoiceDetails")]
    public virtual ChequeEntry ChequeEntry { get; set; } = null!;

    [ForeignKey("ChequeIndentDetailId")]
    [InverseProperty("ChequeInvoiceDetails")]
    public virtual ChequeIndentDetail ChequeIndentDetail { get; set; } = null!;

    [ForeignKey("ChequeInvoiceId")]
    [InverseProperty("ChequeInvoiceDetails")]
    public virtual ChequeInvoice ChequeInvoice { get; set; } = null!;
}
