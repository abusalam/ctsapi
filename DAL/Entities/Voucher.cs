using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("voucher", Schema = "cts")]
public partial class Voucher
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("voucher_no")]
    public int VoucherNo { get; set; }

    [Column("voucher_date")]
    public DateOnly VoucherDate { get; set; }

    [Column("token_id")]
    public long TokenId { get; set; }

    [Column("major_head")]
    [StringLength(4)]
    public string MajorHead { get; set; } = null!;

    [Column("amount")]
    [Precision(10, 2)]
    public decimal Amount { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public long CreatedBy { get; set; }

    [Column("payment_advice_id")]
    public long? PaymentAdviceId { get; set; }

    [Column("financial_year_id")]
    public short FinancialYearId { get; set; }
}
