using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_PREP_CALC_HEADER", Schema = "cts_pension")]
[Index("IntPensionerId", "IterationNo", Name = "UK_P_MM_PEN_PREP_CALC_HEADER1", IsUnique = true)]
[Index("IntPenPrepCalcHeaderId", "IterationNo", Name = "UK_P_MM_PEN_PREP_CALC_HEADER2", IsUnique = true)]
public partial class PMmPenPrepCalcHeader
{
    [Key]
    [Column("INT_PEN_PREP_CALC_HEADER_ID")]
    public long IntPenPrepCalcHeaderId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("ITERATION_NO")]
    public int IterationNo { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string ActiveFlag { get; set; } = null!;

    [Column("REQUEST_ID")]
    public int RequestId { get; set; }

    [Column("DATE_OF_COMMENCE_PEN_SERVICE")]
    public DateOnly? DateOfCommencePenService { get; set; }

    [Column("DATE_OF_COMMENCE_PEN")]
    public DateOnly? DateOfCommencePen { get; set; }

    [Column("WORK_CHARGE_EMPLOYEE")]
    [StringLength(1)]
    public string? WorkChargeEmployee { get; set; }

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

    [Column("PENSION_PAY_ORDER_NO")]
    [StringLength(50)]
    public string? PensionPayOrderNo { get; set; }

    [Column("PENSION_PAY_ORDER_DATE")]
    public DateOnly? PensionPayOrderDate { get; set; }

    /// <summary>
    /// &apos;Y&apos;/&apos;N&apos; flag
    /// </summary>
    [Column("COMMUTATION_SANCTIONED_FLAG")]
    [StringLength(1)]
    public string? CommutationSanctionedFlag { get; set; }

    /// <summary>
    /// &apos;Y&apos;/&apos;N&apos; flag
    /// </summary>
    [Column("IMMEDIATE_RELIEF_FLAG")]
    [StringLength(1)]
    public string? ImmediateReliefFlag { get; set; }

    [Column("PENSION_HOA")]
    public int? PensionHoa { get; set; }

    [Column("GRATUITY_HOA")]
    public int? GratuityHoa { get; set; }

    /// <summary>
    /// &apos;Y&apos;/&apos;N&apos; flag
    /// </summary>
    [Column("SATISFIED_FLAG")]
    [StringLength(1)]
    public string? SatisfiedFlag { get; set; }

    [Column("REMARKS")]
    [StringLength(500)]
    public string? Remarks { get; set; }

    /// <summary>
    /// &apos;P&apos; for percentage &apos;A&apos; for Amount
    /// </summary>
    [Column("PENSION_AGREED_PER_AMT_FLAG")]
    [StringLength(1)]
    public string? PensionAgreedPerAmtFlag { get; set; }

    [Column("PENSION_AGREED_PER_AMT_VALUE")]
    public int? PensionAgreedPerAmtValue { get; set; }

    [Column("PENSION_AGREED_AMT")]
    public int? PensionAgreedAmt { get; set; }

    [Column("PENSION_AGREED_DIFF_AMT")]
    public int? PensionAgreedDiffAmt { get; set; }

    [Column("PENSION_TOTAL_AMOUNT")]
    public int? PensionTotalAmount { get; set; }

    [Column("GRATUITY_TOTAL_AMOUNT")]
    public int? GratuityTotalAmount { get; set; }

    /// <summary>
    /// &apos;P&apos; for percentage &apos;A&apos; for Amount
    /// </summary>
    [Column("GRATUITY_AGREED_PER_AMT_FLAG")]
    [StringLength(1)]
    public string? GratuityAgreedPerAmtFlag { get; set; }

    [Column("GRATUITY_AGREED_PER_AMT_VALUE")]
    public int? GratuityAgreedPerAmtValue { get; set; }

    [Column("GRATUITY_AGREED_AMT")]
    public int? GratuityAgreedAmt { get; set; }

    [Column("GRATUITY_AGREED_DIFF_AMT")]
    public int? GratuityAgreedDiffAmt { get; set; }

    [Column("REDUCED_PENSION")]
    public int? ReducedPension { get; set; }

    /// <summary>
    /// memo no  to be given before approving calculation
    /// </summary>
    [Column("MEMO_NO")]
    [StringLength(200)]
    public string? MemoNo { get; set; }

    /// <summary>
    /// memo date to be given before approving calculation
    /// </summary>
    [Column("MEMO_DATE")]
    public DateOnly? MemoDate { get; set; }

    [Column("LAST_PAY")]
    public int? LastPay { get; set; }

    [Column("LAST_BASIC")]
    public int? LastBasic { get; set; }

    [Column("LAST_GRADE_PAY")]
    public int? LastGradePay { get; set; }

    [Column("LAST_DA")]
    public int? LastDa { get; set; }

    [Column("LAST_PAY_BILL_GROUP_ID")]
    public int? LastPayBillGroupId { get; set; }

    [Column("LAST_PAY_VOUCHER_DATE")]
    public DateOnly? LastPayVoucherDate { get; set; }

    [Column("LAST_PAYB_ID")]
    public int? LastPaybId { get; set; }

    [Column("LAST_PAY_SCALE_ID")]
    public int? LastPayScaleId { get; set; }

    [Column("PAY_BAND_SCALE_DESC")]
    [StringLength(500)]
    public string? PayBandScaleDesc { get; set; }

    [Column("LAST_ROPA_ID")]
    public int? LastRopaId { get; set; }

    [Column("ENHANCED_FAMILY_PENSION_UPTO")]
    public DateOnly? EnhancedFamilyPensionUpto { get; set; }

    [Column("ENHANCED_FAMILY_PENSION_AMT")]
    public int? EnhancedFamilyPensionAmt { get; set; }

    [Column("FAMILY_PENSION_AMT")]
    public int? FamilyPensionAmt { get; set; }

    /// <summary>
    /// data will be storred for Supperannuation system_calc_Basic Pension_amt or User_updated_Basic Pension_amt
    /// </summary>
    [Column("PENSION_CALCULATED")]
    public int? PensionCalculated { get; set; }

    [Column("GRATUITY_CALCULATED")]
    public int? GratuityCalculated { get; set; }

    [Column("CVP_CALCULATED")]
    public int? CvpCalculated { get; set; }

    [Column("NET_EFFECTIVE_SRV_PERIOD")]
    public int? NetEffectiveSrvPeriod { get; set; }

    [Column("PENSION_HOA_DESC")]
    [StringLength(200)]
    public string? PensionHoaDesc { get; set; }

    [Column("GRATUITY_HOA_DESC")]
    [StringLength(200)]
    public string? GratuityHoaDesc { get; set; }

    [Column("LAST_HRA")]
    public int? LastHra { get; set; }

    [Column("LAST_MA")]
    public int? LastMa { get; set; }

    [Column("PENSION_HOA_MAP_ID")]
    public int? PensionHoaMapId { get; set; }

    [Column("GRATUITY_HOA_MAP_ID")]
    public int? GratuityHoaMapId { get; set; }

    [Column("DEATH_GRATUITY_AMOUNT")]
    public int? DeathGratuityAmount { get; set; }

    /// <summary>
    /// If Immidiate Relief Flag is Yes then Immidiate Relief Amount to be filled
    /// </summary>
    [Column("IMMEDIATE_RELIEF_AMOUNT")]
    public int? ImmediateReliefAmount { get; set; }

    [Column("COURT_CASE_MEMO_NO")]
    [StringLength(200)]
    public string? CourtCaseMemoNo { get; set; }

    [Column("COURT_CASE_MEMO_DATE")]
    public DateOnly? CourtCaseMemoDate { get; set; }

    [Column("NO_DEMAND_MEMO_NO")]
    [StringLength(200)]
    public string? NoDemandMemoNo { get; set; }

    [Column("NO_DEMAND_MEMO_DATE")]
    public DateOnly? NoDemandMemoDate { get; set; }

    [Column("EPF_ENHANCED_UPTO")]
    public DateOnly? EpfEnhancedUpto { get; set; }

    [Column("PROV_MONTHLY_PEN_AMT_SYS")]
    public int? ProvMonthlyPenAmtSys { get; set; }

    [Column("PROV_MONTHLY_PEN_AMT_USER")]
    public int? ProvMonthlyPenAmtUser { get; set; }

    [Column("PROV_DEATH_GRATUITY_AMT_SYS")]
    public int? ProvDeathGratuityAmtSys { get; set; }

    [Column("PROV_DEATH_GRATUITY_AMT_USER")]
    public int? ProvDeathGratuityAmtUser { get; set; }

    [Column("PROV_FAMILY_PEN_AMT_SYS")]
    public int? ProvFamilyPenAmtSys { get; set; }

    [Column("PROV_FAMILY_PEN_AMT_USER")]
    public int? ProvFamilyPenAmtUser { get; set; }

    [Column("PROV_GRATUITY_AMT_SYS")]
    public int? ProvGratuityAmtSys { get; set; }

    [Column("PROV_GRATUITY_AMT_USER")]
    public int? ProvGratuityAmtUser { get; set; }

    [Column("PROV_GRATUITY_FORMULA_N_VALUE")]
    [StringLength(500)]
    public string? ProvGratuityFormulaNValue { get; set; }

    /// <summary>
    /// Rule used for calculation of normal provisional pension
    /// </summary>
    [Column("INT_PEN_PROV_RULE_DTLS_ID")]
    public int? IntPenProvRuleDtlsId { get; set; }

    /// <summary>
    /// Rule used for calculation of normal provisional Gratuity
    /// </summary>
    [Column("INT_PEN_PROV_GRAT_RULE_DTLS_ID")]
    public int? IntPenProvGratRuleDtlsId { get; set; }

    [Column("PROV_FORMULA_WITH_VALUE")]
    [StringLength(500)]
    public string? ProvFormulaWithValue { get; set; }

    [Column("NET_QUALIFING_SERVICE_YEAR")]
    public int? NetQualifingServiceYear { get; set; }

    [Column("NET_QUALIFING_SERVICE_MONTH")]
    public int? NetQualifingServiceMonth { get; set; }

    [Column("NET_QUALIFING_SERVICE_DAYS")]
    public int? NetQualifingServiceDays { get; set; }

    /// <summary>
    /// &apos;Y&apos; = include &apos;N&apos; = Exclude
    /// </summary>
    [Column("PROV_PEN_INCLUDE_EXCLUDE_FLAG")]
    [StringLength(1)]
    public string? ProvPenIncludeExcludeFlag { get; set; }

    /// <summary>
    /// &apos;Y&apos; = include &apos;N&apos; = Exclude
    /// </summary>
    [Column("PROV_GRAT_INCLUDE_EXCLUDE_FLAG")]
    [StringLength(1)]
    public string? ProvGratIncludeExcludeFlag { get; set; }

    /// <summary>
    /// Percentage at which DA calculated
    /// </summary>
    [Column("LAST_DA_RATE")]
    public int? LastDaRate { get; set; }

    [Column("LAST_NPA")]
    public int? LastNpa { get; set; }
}
