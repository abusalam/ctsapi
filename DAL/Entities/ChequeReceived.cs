using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace CTS_BE.DAL.Entities;

[Table("cheque_received", Schema = "cts")]
public partial class ChequeReceived
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("range")]
    public NpgsqlRange<int>[][]? Range { get; set; }

    [Column("quantity")]
    public short? Quantity { get; set; }

    [Column("received_user")]
    public short? ReceivedUser { get; set; }

    [Column("cheque_series_info", TypeName = "char")]
    public char? ChequeSeriesInfo { get; set; }

    [Column("invoice_id")]
    public short? InvoiceId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
}
