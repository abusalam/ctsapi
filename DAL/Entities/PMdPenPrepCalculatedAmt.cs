using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_CALCULATED_AMT", Schema = "cts_pension")]
public partial class PMdPenPrepCalculatedAmt
{
    [Key]
    [Column("INT_PREP_CALCULATED_AMT_ID")]
    public int IntPrepCalculatedAmtId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("INT_PEN_PREP_CALC_HEADER_ID")]
    public int IntPenPrepCalcHeaderId { get; set; }

    [Column("ITERATION_NO")]
    public int IterationNo { get; set; }

    [Column("BENF_TYPE_ID")]
    public int? BenfTypeId { get; set; }

    [Column("PEN_TYPE_ID")]
    public int PenTypeId { get; set; }

    [Column("INT_PEN_PREP_CALC_HDR_ID")]
    public int IntPenPrepCalcHdrId { get; set; }

    [Column("INT_PEN_RULE_DTLS_ID")]
    public int IntPenRuleDtlsId { get; set; }

    [Column("AMOUNT_SYSTEM")]
    public int? AmountSystem { get; set; }

    [Column("AMOUNT_APPLICABLE_FOR_CALC")]
    public int? AmountApplicableForCalc { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string ActiveFlag { get; set; } = null!;

    [Column("REQUEST_ID")]
    public int RequestId { get; set; }

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("CREATED_USER_ID")]
    public int CreatedUserId { get; set; }

    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }

    [Column("CREATED_ROLE_ID")]
    public int CreatedRoleId { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int ModifiedRoleId { get; set; }

    [Column("FORMULA_WITH_VALUE")]
    [StringLength(500)]
    public string? FormulaWithValue { get; set; }

    [Column("WORKFLOW_STATUS_FLAG")]
    [StringLength(2)]
    public string WorkflowStatusFlag { get; set; } = null!;
}
