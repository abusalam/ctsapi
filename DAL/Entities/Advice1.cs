using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("advice", Schema = "cts")]
public partial class Advice1
{
    [Column("lfpl_advice_id")]
    public long? LfplAdviceId { get; set; }

    [Column("lfpl_advice_date")]
    public DateOnly? LfplAdviceDate { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("op_code")]
    public short? OpCode { get; set; }

    [Column("op_id")]
    public int? OpId { get; set; }

    [Key]
    [Column("treasury_advice_id")]
    public long TreasuryAdviceId { get; set; }

    [Column("treasury_advice_date")]
    public DateOnly? TreasuryAdviceDate { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("remarks", TypeName = "character varying")]
    public string? Remarks { get; set; }

    [Column("memo_no", TypeName = "character varying")]
    public string? MemoNo { get; set; }

    [Column("memo_date")]
    public DateOnly? MemoDate { get; set; }

    public virtual OperatorMaster? Op { get; set; }

    [ForeignKey("Status")]
    [InverseProperty("Advice1s")]
    public virtual Status? StatusNavigation { get; set; }
}
