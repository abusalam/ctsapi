using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_RULE_SUB_DTLS", Schema = "cts_pension")]
public partial class PMmPenRuleSubDtl
{
    [Key]
    [Column("INT_PEN_RULE_SUB_DTLS_ID")]
    public long IntPenRuleSubDtlsId { get; set; }

    [Column("INT_PEN_RULE_DTLS_ID")]
    public int IntPenRuleDtlsId { get; set; }

    [Column("MIN_BASIC_PAY")]
    public int? MinBasicPay { get; set; }

    [Column("MAX_BASIC_PAY")]
    public int? MaxBasicPay { get; set; }

    [Column("PERCENT_BASIC_PAY")]
    public int? PercentBasicPay { get; set; }

    [Column("MIN_CALCULATED_AMT")]
    public int? MinCalculatedAmt { get; set; }

    [Column("MAX_CALCULATED_AMT")]
    public int? MaxCalculatedAmt { get; set; }

    [Column("RULES")]
    [StringLength(999)]
    public string? Rules { get; set; }
}
