using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("challan", Schema = "cts")]
public partial class Challan
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("challan_no")]
    [MaxLength(1)]
    public char ChallanNo { get; set; }

    [Column("challan_date")]
    public DateOnly ChallanDate { get; set; }

    [Column("token_id")]
    public long TokenId { get; set; }

    [Column("active_hoa_id")]
    public long ActiveHoaId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("financial_year")]
    public short FinancialYear { get; set; }

    [Column("created_by")]
    public long CreatedBy { get; set; }

    [Column("created_at")]
    public TimeOnly CreatedAt { get; set; }

    [Column("payment_advice_id")]
    public long? PaymentAdviceId { get; set; }

    [Column("major_head")]
    [StringLength(4)]
    public string? MajorHead { get; set; }
}
