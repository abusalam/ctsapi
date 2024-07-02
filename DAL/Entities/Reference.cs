using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("reference", Schema = "lf_pl")]
public partial class Reference
{
    [Key]
    [Column("ref_id")]
    public long RefId { get; set; }

    [Column("ref_no")]
    public long? RefNo { get; set; }

    [Column("ref_date")]
    public DateOnly? RefDate { get; set; }

    [Column("advice_id")]
    public long? AdviceId { get; set; }

    [Column("advice_date")]
    public DateOnly? AdviceDate { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("op_code")]
    public short? OpCode { get; set; }

    [Column("schemes", TypeName = "jsonb")]
    public string? Schemes { get; set; }

    [Column("gross_amount")]
    public double? GrossAmount { get; set; }

    [Column("net_amount")]
    public double? NetAmount { get; set; }

    [Column("no_of_schemes")]
    public short? NoOfSchemes { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("ref_type")]
    public short? RefType { get; set; }

    [Column("total_bt_count")]
    public short? TotalBtCount { get; set; }
}
