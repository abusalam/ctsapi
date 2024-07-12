using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_CALCULATION_HDR", Schema = "cts_pension")]
public partial class PMdPenPrepCalculationHdr
{
    [Key]
    [Column("INT_PEN_PREP_CALC_HDR_ID")]
    public long IntPenPrepCalcHdrId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("INT_PEN_PREP_CALC_HEADER_ID")]
    public int IntPenPrepCalcHeaderId { get; set; }

    [Column("ITERATION_NO")]
    public int IterationNo { get; set; }

    [Column("REQUEST_ID")]
    public int RequestId { get; set; }

    [Column("INT_PEN_RULE_NAME_ID")]
    public int IntPenRuleNameId { get; set; }

    [Column("NET_QUALIFYING_SERVICE_YR")]
    public int? NetQualifyingServiceYr { get; set; }

    [Column("NET_QUALIFYING_SERVICE_MON")]
    public int? NetQualifyingServiceMon { get; set; }

    [Column("NET_QUALIFYING_SERVICE_DAYS")]
    public int? NetQualifyingServiceDays { get; set; }

    [Column("LAST_PAY")]
    public int? LastPay { get; set; }

    [Column("LAST_BASIC")]
    public int? LastBasic { get; set; }

    [Column("LAST_GRADE_PAY")]
    public int? LastGradePay { get; set; }

    [Column("LAST_DA")]
    public int? LastDa { get; set; }

    [Column("DATE_OF_BIRTH")]
    public DateOnly DateOfBirth { get; set; }

    [Column("DATE_OF_RETIREMENT")]
    public DateOnly DateOfRetirement { get; set; }

    [Column("AGE_AT_NEXT_BIRTHDAY")]
    public int AgeAtNextBirthday { get; set; }

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

    [Column("WORKFLOW_STATUS_FLAG")]
    [StringLength(2)]
    public string WorkflowStatusFlag { get; set; } = null!;

    [Column("LAST_MA")]
    public int? LastMa { get; set; }
}
