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
    public short? InvoiceId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("cheque_entry_id")]
    public long? ChequeEntryId { get; set; }

    [Column("start")]
    public short? Start { get; set; }

    [Column("end")]
    public short? End { get; set; }
}
