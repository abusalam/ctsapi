using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_RULE_DTLS", Schema = "cts_pension")]
public partial class PMmPenRuleDtl
{
    [Key]
    [Column("INT_PEN_RULE_DTLS_ID")]
    public int IntPenRuleDtlsId { get; set; }

    [Column("INT_PEN_RULE_NAME_ID")]
    public int IntPenRuleNameId { get; set; }

    [Column("PAY_RECO_TYPE_ID")]
    public int PayRecoTypeId { get; set; }

    [Column("RULE_NAME")]
    [StringLength(300)]
    public string? RuleName { get; set; }

    [Column("RULE_ABBR")]
    [StringLength(10)]
    public string? RuleAbbr { get; set; }

    [Column("RULES_REMARKS")]
    [StringLength(500)]
    public string? RulesRemarks { get; set; }

    [Column("MIN_SERVICE_YR")]
    public int? MinServiceYr { get; set; }

    [Column("MIN_SERVICE_YR_OPERATOR")]
    [StringLength(10)]
    public string? MinServiceYrOperator { get; set; }

    [Column("MAX_SERVICE_YR")]
    public int? MaxServiceYr { get; set; }

    [Column("MAX_SERVICE_YR_OPERATOT")]
    [StringLength(10)]
    public string? MaxServiceYrOperatot { get; set; }

    [Column("PERCENT_OF_AMT")]
    public int? PercentOfAmt { get; set; }

    [Column("MAX_SERVICE_YEAR")]
    public int? MaxServiceYear { get; set; }

    [Column("RETIREMENT_DATE_FROM")]
    public DateOnly? RetirementDateFrom { get; set; }

    [Column("RETIREMENT_DATE_TO")]
    public DateOnly? RetirementDateTo { get; set; }

    [Column("MIN_CALCULATED_AMT")]
    public int? MinCalculatedAmt { get; set; }

    [Column("MAX_CALCULATED_AMT")]
    public int? MaxCalculatedAmt { get; set; }

    [Column("RULES")]
    [StringLength(999)]
    public string? Rules { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string ActiveFlag { get; set; } = null!;

    /// <summary>
    /// pay_allowance_abbr
    /// </summary>
    [Column("ROPA_ABBR")]
    [StringLength(50)]
    public string? RopaAbbr { get; set; }
}
