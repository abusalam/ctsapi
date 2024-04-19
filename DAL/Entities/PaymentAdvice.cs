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

    /// <summary>
    /// 1=sortlist 2=paymandate
    /// </summary>
    [Column("status")]
    public short Status { get; set; }

    [Column("financial_year_id")]
    public short FinancialYearId { get; set; }

    [Column("payment_status")]
    public short? PaymentStatus { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public long CreatedBy { get; set; }
}
