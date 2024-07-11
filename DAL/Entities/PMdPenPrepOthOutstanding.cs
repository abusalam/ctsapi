using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_OTH_OUTSTANDING", Schema = "cts_pension")]
public partial class PMdPenPrepOthOutstanding
{
    [Key]
    [Column("INT_PEN_OTH_OUTSTANDING_ID")]
    public int IntPenOthOutstandingId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("INT_PEN_PREP_CALC_HEADER_ID")]
    public int IntPenPrepCalcHeaderId { get; set; }

    [Column("ITERATION_NO")]
    public int IterationNo { get; set; }

    /// <summary>
    /// &apos;PA&apos;--- Pay Allowance type overdrawl out standing, &apos;OTH&apos; other outstanding
    /// </summary>
    [Column("OUTSTANDING_TYPE")]
    [StringLength(2)]
    public string OutstandingType { get; set; } = null!;

    [Column("COMP_DESCRIPTION")]
    [StringLength(200)]
    public string? CompDescription { get; set; }

    [Column("INT_OVERDRAWL_DETAILS_ID")]
    public int? IntOverdrawlDetailsId { get; set; }

    [Column("INT_COMPONENT_ID")]
    public int IntComponentId { get; set; }

    [Column("AMOUNT")]
    public int Amount { get; set; }

    [Column("FROM_DATE")]
    public DateOnly? FromDate { get; set; }

    [Column("RECOVERY_FLAG")]
    [StringLength(1)]
    public string? RecoveryFlag { get; set; }

    [Column("HOA_ID")]
    public int? HoaId { get; set; }

    [Column("DEMAND_NO")]
    [StringLength(2)]
    public string? DemandNo { get; set; }

    [Column("MAJOR_HEAD")]
    [StringLength(4)]
    public string? MajorHead { get; set; }

    [Column("SUBMAJOR_HEAD")]
    [StringLength(2)]
    public string? SubmajorHead { get; set; }

    [Column("MINOR_HEAD")]
    [StringLength(3)]
    public string? MinorHead { get; set; }

    [Column("PLAN_STATUS")]
    [StringLength(2)]
    public string? PlanStatus { get; set; }

    [Column("SCHEME_HEAD")]
    [StringLength(3)]
    public string? SchemeHead { get; set; }

    [Column("DETAIL_HEAD")]
    [StringLength(2)]
    public string DetailHead { get; set; } = null!;

    [Column("SUBDETAIL_HEAD")]
    [StringLength(2)]
    public string SubdetailHead { get; set; } = null!;

    [Column("CHARGED_VOTED")]
    [StringLength(1)]
    public string? ChargedVoted { get; set; }

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

    [Column("REMARKS")]
    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column("OVRDRAWAL_REASON")]
    [StringLength(500)]
    public string? OvrdrawalReason { get; set; }

    [Column("RECOVERED_AMOUNT")]
    public int? RecoveredAmount { get; set; }

    [Column("DIFFERENCE_AMOUNT")]
    public int? DifferenceAmount { get; set; }
}
