using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
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

    [Column("treasury_advice_id")]
    public long TreasuryAdviceId { get; set; }
}
