using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("payment_advice", Schema = "cts")]
public partial class PaymentAdvice
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("token_id")]
    public long TokenId { get; set; }

    [Column("paymandate_date")]
    public DateOnly PaymandateDate { get; set; }
}
