using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_received", Schema = "cts")]
public partial class ChequeReceived
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("quantity")]
    public short? Quantity { get; set; }

    [Column("received_user")]
    public short? ReceivedUser { get; set; }

    [Column("invoice_id")]
    public long? InvoiceId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("cheque_invoice_details_id")]
    public long? ChequeInvoiceDetailsId { get; set; }

    [Column("start")]
    public short? Start { get; set; }

    [Column("end")]
    public short? End { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("available_quantity")]
    public short? AvailableQuantity { get; set; }

    [Column("treasurie_code")]
    [StringLength(3)]
    public string? TreasurieCode { get; set; }

    [Column("micr_code")]
    [StringLength(10)]
    public string? MicrCode { get; set; }

    [Column("series_no")]
    [StringLength(11)]
    public string? SeriesNo { get; set; }

    [Column("current_position")]
    public short? CurrentPosition { get; set; }

    [Column("is_used")]
    public bool? IsUsed { get; set; }
}
