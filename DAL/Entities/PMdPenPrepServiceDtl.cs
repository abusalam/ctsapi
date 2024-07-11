using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_SERVICE_DTLS", Schema = "cts_pension")]
public partial class PMdPenPrepServiceDtl
{
    [Key]
    [Column("INT_PEN_SERVICE_DTLS_ID")]
    public int IntPenServiceDtlsId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("INT_SERVICE_TYPE_ID")]
    public int? IntServiceTypeId { get; set; }

    [Column("INT_PEN_PREP_CALC_HEADER_ID")]
    public int IntPenPrepCalcHeaderId { get; set; }

    [Column("SOURCE_ORG_ADD_SERVICE_NAME")]
    [StringLength(200)]
    public string SourceOrgAddServiceName { get; set; } = null!;

    [Column("FROM_DATE")]
    public DateOnly FromDate { get; set; }

    [Column("TO_DATE")]
    public DateOnly ToDate { get; set; }

    [Column("YEAR_IN_NUM")]
    public int YearInNum { get; set; }

    [Column("MONTH_IN_NUM")]
    public int MonthInNum { get; set; }

    [Column("DAYS_IN_NUM")]
    public int DaysInNum { get; set; }

    [Column("REMARKS")]
    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column("MODIFIABLE_FLAG")]
    [StringLength(1)]
    public string ModifiableFlag { get; set; } = null!;

    /// <summary>
    /// SHRMS-  Service HRMS,                 SNHRMS- Service Non HRMS,
    /// SADD-   Additional Service,
    /// SDEPH-  herms deputation,             SDEPNH- deputation non hrms,
    /// SNQHL-  non qualifying hrms leave,    SNQNHL-non qualifying non hrms leave,
    /// SNQHS-  non qualifying hrms suspence, SNQNHS-non qualifying non hrms suspence,
    /// SNQO-   non qualifying others,        WSADM --- Weightage  
    /// </summary>
    [Column("PERIOD_TYPE")]
    [StringLength(6)]
    public string PeriodType { get; set; } = null!;

    [Column("QUAL_NON_QUAL_WEIGHTAGE_TYPE")]
    [StringLength(2)]
    public string QualNonQualWeightageType { get; set; } = null!;

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

    [Column("ITERATION_NO")]
    public int IterationNo { get; set; }

    /// <summary>
    /// Not required from fron end
    /// </summary>
    [Column("INT_EMP_WORKING_DTLS_ID")]
    public int? IntEmpWorkingDtlsId { get; set; }

    /// <summary>
    /// Not required from fron end
    /// </summary>
    [Column("INT_LEAVE_TYPE_ID")]
    public int? IntLeaveTypeId { get; set; }

    /// <summary>
    /// Not required from fron end
    /// </summary>
    [Column("BUSINESS_PK_TYPE")]
    [StringLength(5)]
    public string? BusinessPkType { get; set; }

    /// <summary>
    /// Not required from fron end
    /// </summary>
    [Column("INT_LEAVE_ID")]
    public int? IntLeaveId { get; set; }

    /// <summary>
    /// will come from hr_mm_gen_other_master p WHERE p.master_abbr = &apos;OST&apos;
    /// </summary>
    [Column("INT_OTHER_SERVICE_TYPE_ID")]
    public int? IntOtherServiceTypeId { get; set; }

    /// <summary>
    /// Not required from fron end For Deputation
    /// </summary>
    [Column("INT_EMP_WORKING_DTLS_ID_DEPU")]
    public int? IntEmpWorkingDtlsIdDepu { get; set; }

    [Column("WORKFLOW_STATUS_FLAG")]
    [StringLength(2)]
    public string WorkflowStatusFlag { get; set; } = null!;

    /// <summary>
    /// 22c Any Contribution Received Y/N flag
    /// </summary>
    [Column("CONTRIBUTION_RECEIVED_FLAG")]
    [StringLength(1)]
    public string? ContributionReceivedFlag { get; set; }

    /// <summary>
    /// 22d
    /// </summary>
    [Column("PENSION_RECEIVED")]
    public int? PensionReceived { get; set; }

    /// <summary>
    /// 22d
    /// </summary>
    [Column("GRATUITY_RECEIVED")]
    public int? GratuityReceived { get; set; }

    /// <summary>
    /// 22f
    /// </summary>
    [Column("CONTRIBUTION_RECEIVED_SOURCE")]
    [StringLength(500)]
    public string? ContributionReceivedSource { get; set; }

    /// <summary>
    /// Government under which the service
    /// </summary>
    [Column("GOVT_NAME")]
    [StringLength(500)]
    public string? GovtName { get; set; }

    /// <summary>
    /// 22e Y/N flag
    /// </summary>
    [Column("FAMILY_PENSION_FLAG")]
    [StringLength(1)]
    public string? FamilyPensionFlag { get; set; }

    /// <summary>
    /// Added for Block &apos;B&apos; &amp; &apos;C&apos; see CR No 181316 for Details
    /// </summary>
    [Column("GO_NO")]
    [StringLength(200)]
    public string? GoNo { get; set; }

    /// <summary>
    /// Added for Block &apos;B&apos; &amp; &apos;C&apos; see CR No 181316 for Details
    /// </summary>
    [Column("GO_DATE")]
    public DateOnly? GoDate { get; set; }

    /// <summary>
    /// Added for Block &apos;B&apos; &amp; &apos;C&apos; see CR No 181316 for Details
    /// </summary>
    [Column("AMT_OF_CONTRIBUTION")]
    public int? AmtOfContribution { get; set; }
}
