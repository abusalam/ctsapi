﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("transaction_lot_history ", Schema = "cts-payment")]
public partial class TransactionLotHistory
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("transaction_lot_id")]
    public long TransactionLotId { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("response", TypeName = "character varying")]
    public string? Response { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
