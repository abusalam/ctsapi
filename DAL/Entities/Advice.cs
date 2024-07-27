using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("advice", Schema = "lf_pl")]
public partial class Advice
{
    [Key]
    [Column("advice_id")]
    public long AdviceId { get; set; }

    [Column("advice_date")]
    public DateOnly? AdviceDate { get; set; }

    [Column("reference_object", TypeName = "jsonb")]
    public string? ReferenceObject { get; set; }

    [Column("no_of_reference")]
    public short? NoOfReference { get; set; }

    [Column("gross_amount")]
    [Precision(10, 2)]
    public decimal? GrossAmount { get; set; }

    [Column("net_amount")]
    [Precision(10, 2)]
    public decimal? NetAmount { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("memo_no", TypeName = "character varying")]
    public string? MemoNo { get; set; }

    [Column("memo_date")]
    public DateOnly? MemoDate { get; set; }

    [Column("ref_type")]
    public short? RefType { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("op_code")]
    public short? OpCode { get; set; }

    [Column("op_id")]
    public int? OpId { get; set; }

    public virtual OperatorMaster? Op { get; set; }

    [InverseProperty("Advice")]
    public virtual ICollection<Reference> References { get; set; } = new List<Reference>();
}
