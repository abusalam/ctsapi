using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("ecs_neft_payment_status_details", Schema = "cts-payment")]
public partial class EcsNeftPaymentStatusDetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("ecs_neft_id")]
    public long EcsNeftId { get; set; }

    [Column("payment_status")]
    public short? PaymentStatus { get; set; }

    [Column("response")]
    public short? Response { get; set; }
}
