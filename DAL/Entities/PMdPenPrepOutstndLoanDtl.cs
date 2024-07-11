using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_OUTSTND_LOAN_DTL", Schema = "cts_pension")]
public partial class PMdPenPrepOutstndLoanDtl
{
    [Key]
    [Column("INT_PEN_OUTSTND_LOAN_DTL_ID")]
    public int IntPenOutstndLoanDtlId { get; set; }

    [Column("INT_PEN_OUTSTANDING_LOAN_ID")]
    public int IntPenOutstandingLoanId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("INT_PEN_PREP_CALC_HEADER_ID")]
    public int IntPenPrepCalcHeaderId { get; set; }

    [Column("ITERATION_NO")]
    public int IterationNo { get; set; }

    [Column("INT_EMP_RECOVERY_ID")]
    public int IntEmpRecoveryId { get; set; }

    [Column("INT_COMP_ID")]
    public int IntCompId { get; set; }

    [Column("RECOVERY_AMOUNT")]
    public int RecoveryAmount { get; set; }

    [Column("WEF")]
    public DateOnly Wef { get; set; }

    [Column("EFFECTIVE_END_DATE")]
    public DateOnly EffectiveEndDate { get; set; }

    [Column("INT_EMP_LOAN_ADVANCE_ID")]
    public int IntEmpLoanAdvanceId { get; set; }

    [Column("PAID_FLAG")]
    [StringLength(1)]
    public string PaidFlag { get; set; } = null!;

    [Column("PAID_DATE")]
    public DateOnly? PaidDate { get; set; }

    [Column("PAID_AMOUNT")]
    public int? PaidAmount { get; set; }

    [Column("LEGACY_FLAG")]
    [StringLength(1)]
    public string? LegacyFlag { get; set; }

    [Column("INT_EMP_LOAN_ADV_FC_ID")]
    public int? IntEmpLoanAdvFcId { get; set; }

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
    public int? CreatedRoleId { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int? ModifiedRoleId { get; set; }
}
