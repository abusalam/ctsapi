using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_OUTSTANDING_LOAN", Schema = "cts_pension")]
public partial class PMdPenPrepOutstandingLoan
{
    [Key]
    [Column("INT_PEN_OUTSTANDING_LOAN_ID")]
    public long IntPenOutstandingLoanId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("INT_PEN_PREP_CALC_HEADER_ID")]
    public int IntPenPrepCalcHeaderId { get; set; }

    [Column("ITERATION_NO")]
    public int IterationNo { get; set; }

    [Column("INT_LOAN_ADVANCE_TYPE_ID")]
    public int IntLoanAdvanceTypeId { get; set; }

    [Column("INT_EMP_LOAN_ADVANCE_ID")]
    public int IntEmpLoanAdvanceId { get; set; }

    [Column("INT_COMPONENT_ID_PRIN")]
    public int IntComponentIdPrin { get; set; }

    [Column("INT_COMPONENT_ID_INT")]
    public int? IntComponentIdInt { get; set; }

    [Column("OUTSTANDING_AS_ON_DATE")]
    public DateOnly? OutstandingAsOnDate { get; set; }

    [Column("SANCTION_DATE")]
    public DateOnly? SanctionDate { get; set; }

    [Column("OUT_STANDING_PRIN_AMT")]
    public int? OutStandingPrinAmt { get; set; }

    [Column("OUT_STANDING_INT_AMT")]
    public int? OutStandingIntAmt { get; set; }

    [Column("REMARKS")]
    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column("MODIFIABLE_FLAG")]
    [StringLength(1)]
    public string ModifiableFlag { get; set; } = null!;

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
}
