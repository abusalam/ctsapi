using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_EMOLUMENT_DTLS", Schema = "cts_pension")]
public partial class PMdPenPrepEmolumentDtl
{
    [Key]
    [Column("INT_PEN_PREP_EML_DTLS_ID")]
    public int IntPenPrepEmlDtlsId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("INT_PEN_PREP_CALC_HEADER_ID")]
    public int IntPenPrepCalcHeaderId { get; set; }

    [Column("ITERATION_NO")]
    public int IterationNo { get; set; }

    [Column("INT_COMPONENT_ID")]
    public int? IntComponentId { get; set; }

    [Column("COMPONENT_NAME")]
    [StringLength(300)]
    public string? ComponentName { get; set; }

    [Column("INT_COMPONENT_ID_DEPUTATION")]
    public int? IntComponentIdDeputation { get; set; }

    [Column("WEF_DATE")]
    public DateOnly? WefDate { get; set; }

    [Column("AMOUNT_SYSTEM")]
    public int? AmountSystem { get; set; }

    [Column("AMOUNT_APPLICABLE_FOR_CALC")]
    public int? AmountApplicableForCalc { get; set; }

    [Column("HRMS_DEPUTATION_FLAG")]
    [StringLength(2)]
    public string? HrmsDeputationFlag { get; set; }

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

    [Column("WORKFLOW_STATUS_FLAG")]
    [StringLength(2)]
    public string WorkflowStatusFlag { get; set; } = null!;

    [Column("CALC_INCLUDE_FLAG")]
    [StringLength(1)]
    public string CalcIncludeFlag { get; set; } = null!;
}
