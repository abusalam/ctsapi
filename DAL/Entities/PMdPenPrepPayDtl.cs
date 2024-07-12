using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_PAY_DTLS", Schema = "cts_pension")]
public partial class PMdPenPrepPayDtl
{
    [Key]
    [Column("INT_PEN_PREP_PAY_DTLS_ID")]
    public long IntPenPrepPayDtlsId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("INT_PEN_PREP_CALC_HEADER_ID")]
    public int IntPenPrepCalcHeaderId { get; set; }

    [Column("ITERATION_NO")]
    public int IterationNo { get; set; }

    [Column("INT_PAY_BAND_ID")]
    public int? IntPayBandId { get; set; }

    [Column("INT_PAY_SCALE_ID")]
    public int? IntPayScaleId { get; set; }

    [Column("INT_REV_PAY_ALLOWANCE_ID")]
    public int IntRevPayAllowanceId { get; set; }

    [Column("PAY_SCALE_BAND_NAME")]
    [StringLength(300)]
    public string PayScaleBandName { get; set; } = null!;

    [Column("ROPA_NAME")]
    [StringLength(300)]
    public string RopaName { get; set; } = null!;

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

    [Column("WORKFLOW_STATUS_FLAG")]
    [StringLength(2)]
    public string WorkflowStatusFlag { get; set; } = null!;

    [Column("INT_PAY_BILL_GROUP_ID")]
    public int? IntPayBillGroupId { get; set; }

    [Column("NEXT_INCRIMENT_DATE")]
    public DateOnly? NextIncrimentDate { get; set; }

    [Column("NEXT_INCRISE_BASIC_AMT")]
    public int? NextIncriseBasicAmt { get; set; }

    [Column("NEXT_INCRISE_SPECIAL_PAY")]
    public int? NextIncriseSpecialPay { get; set; }

    [Column("TOT_SAL_DRAWN")]
    public int? TotSalDrawn { get; set; }

    [Column("TOT_SAL_DRAWN_DATE")]
    public DateOnly? TotSalDrawnDate { get; set; }

    [Column("NEXT_INCRISE_NPA_AMT")]
    public int? NextIncriseNpaAmt { get; set; }

    [Column("NEXT_INCRISE_SPCL_PAY_MODIFIED")]
    public int? NextIncriseSpclPayModified { get; set; }

    [Column("IMMEDIATE_RELIEF_PAID_FLAG")]
    [StringLength(1)]
    public string? ImmediateReliefPaidFlag { get; set; }

    [Column("IMMEDIATE_RELIEF_PAID_AMT")]
    public int? ImmediateReliefPaidAmt { get; set; }

    [Column("IMMEDIATE_RELIEF_PAID_REMARKS")]
    [StringLength(500)]
    public string? ImmediateReliefPaidRemarks { get; set; }
}
