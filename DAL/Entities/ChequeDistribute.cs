using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("cheque_distribute", Schema = "cts")]
public partial class ChequeDistribute
{
    [Column("id")]
    public long? Id { get; set; }

    [Column("range")]
    public NpgsqlRange<int>[]? Range { get; set; }

    [Column("quantity")]
    public short? Quantity { get; set; }

    [Column("distribute_to", TypeName = "char")]
    public char? DistributeTo { get; set; }

    [Column("cheque_series_info", TypeName = "char")]
    public char? ChequeSeriesInfo { get; set; }

    [Column("user_id")]
    public short? UserId { get; set; }

    [Column("received_id")]
    public short? ReceivedId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
}
