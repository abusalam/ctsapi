using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

/// <summary>
/// Store Pensioner Personal information
/// </summary>
[Table("P_MM_PEN_PREP_PENSIONER_DETL", Schema = "cts_pension")]
[Index("PenFileId", Name = "UK_P_MM_PEN_PREP_PEN_DETL1", IsUnique = true)]
public partial class PMmPenPrepPensionerDetl
{
    /// <summary>
    /// pensioner id which is unique
    /// </summary>
    [Key]
    [Column("INT_PENSIONER_ID")]
    public long IntPensionerId { get; set; }

    /// <summary>
    /// gpf-tpf number id referencing column from PF_MM_GEN_SUBSCR_DETL
    /// </summary>
    [Column("INT_GPF_NO_ID_OBS")]
    public int? IntGpfNoIdObs { get; set; }

    /// <summary>
    /// District id of pensioner
    /// </summary>
    [Column("INT_DISTRICT_ID_OBS")]
    public int? IntDistrictIdObs { get; set; }

    [Column("SUBSCR_FIRST_NAME")]
    [StringLength(100)]
    public string? SubscrFirstName { get; set; }

    [Column("SUBSCR_MIDDLE_NAME")]
    [StringLength(100)]
    public string? SubscrMiddleName { get; set; }

    [Column("SUBSCR_LAST_NAME")]
    [StringLength(100)]
    public string? SubscrLastName { get; set; }

    [Column("INT_REL_ID_OBS")]
    public int? IntRelIdObs { get; set; }

    [Column("DATE_OF_BIRTH")]
    public DateOnly DateOfBirth { get; set; }

    [Column("DATE_OF_EXIT")]
    public DateOnly? DateOfExit { get; set; }

    /// <summary>
    /// to be fetched from employee master table -- added by ritu
    /// </summary>
    [Column("RETIREMENT_DATE")]
    public DateOnly? RetirementDate { get; set; }

    [Column("EMP_GENDER_OBS")]
    [StringLength(1)]
    public string? EmpGenderObs { get; set; }

    /// <summary>
    /// referencing from PF_MM_GEN_MARITAL_STATUS table indication maritial states.
    /// </summary>
    [Column("INT_MARITAL_STATUS_ID")]
    public int? IntMaritalStatusId { get; set; }

    [Column("INT_RELIGION_ID")]
    public int? IntReligionId { get; set; }

    [Column("INT_NATIONALITY_ID")]
    public int? IntNationalityId { get; set; }

    [Column("MOBILE_NUMBER")]
    [StringLength(11)]
    public string? MobileNumber { get; set; }

    [Column("EMAIL_ID")]
    [StringLength(100)]
    public string? EmailId { get; set; }

    [Column("IDENTIFICATION_MARK")]
    [StringLength(400)]
    public string? IdentificationMark { get; set; }

    [Column("INT_IDENTITY_TYPE_ID_OBS")]
    public int? IntIdentityTypeIdObs { get; set; }

    [Column("IDENTITY_CARD_NUMBER_OBS")]
    [StringLength(100)]
    public string? IdentityCardNumberObs { get; set; }

    [Column("IFSC_CODE")]
    [StringLength(90)]
    public string? IfscCode { get; set; }

    /// <summary>
    /// bank account id..
    /// </summary>
    [Column("BANK_AC_NO")]
    [StringLength(30)]
    public string? BankAcNo { get; set; }

    [Column("HEIGHT_IN_CENTI")]
    public int? HeightInCenti { get; set; }

    [Column("INT_PAYABLE_TREA_ID_PEN_OBS")]
    public int? IntPayableTreaIdPenObs { get; set; }

    [Column("INT_DDO_ID")]
    public int? IntDdoId { get; set; }

    [Column("CVP_OPTED_FLAG")]
    [StringLength(1)]
    public string? CvpOptedFlag { get; set; }

    [Column("CVP_PERCENTAGE")]
    public int? CvpPercentage { get; set; }

    [Column("P_CITY_VILLAGE_OBS")]
    [StringLength(50)]
    public string? PCityVillageObs { get; set; }

    [Column("P_TOWN_OBS")]
    [StringLength(50)]
    public string? PTownObs { get; set; }

    [Column("P_POLICE_STATION_OBS")]
    [StringLength(50)]
    public string? PPoliceStationObs { get; set; }

    [Column("P_DISTRICT_CODE_OBS")]
    [StringLength(2)]
    public string? PDistrictCodeObs { get; set; }

    [Column("P_STATE_OBS")]
    public int? PStateObs { get; set; }

    [Column("P_PIN_OBS")]
    [StringLength(50)]
    public string? PPinObs { get; set; }

    [Column("C_CITY_VILLAGE_OBS")]
    [StringLength(50)]
    public string? CCityVillageObs { get; set; }

    [Column("C_TOWN_OBS")]
    [StringLength(50)]
    public string? CTownObs { get; set; }

    [Column("C_POLICE_STATION_OBS")]
    [StringLength(50)]
    public string? CPoliceStationObs { get; set; }

    [Column("C_DISTRICT_CODE_OBS")]
    [StringLength(2)]
    public string? CDistrictCodeObs { get; set; }

    [Column("C_STATE_OBS")]
    public int? CStateObs { get; set; }

    [Column("C_PIN_OBS")]
    [StringLength(50)]
    public string? CPinObs { get; set; }

    /// <summary>
    /// unique for every pensioner
    /// </summary>
    [Column("PEN_FILE_ID")]
    public int? PenFileId { get; set; }

    /// <summary>
    /// Last post dat the pensioner hold referencing from P_MM_PEN_PREP_POST
    /// </summary>
    [Column("LAST_POST_ID_OBS")]
    public int? LastPostIdObs { get; set; }

    /// <summary>
    /// unique id of the institute..
    /// </summary>
    [Column("INSTITUTE_ID_OBS")]
    public int? InstituteIdObs { get; set; }

    [Column("SEND_TO_USER_ID_OBS")]
    public int? SendToUserIdObs { get; set; }

    [Column("ACTIVE_FLAG")]
    [MaxLength(1)]
    public char ActiveFlag { get; set; }

    [Column("PROCESSING_FLAG")]
    public int ProcessingFlag { get; set; }

    [Column("PREV_INT_PENSIONER_ID")]
    public int? PrevIntPensionerId { get; set; }

    [Column("IDENTIFICATION_MARK2")]
    [StringLength(400)]
    public string? IdentificationMark2 { get; set; }

    [Column("PAN_NO")]
    [StringLength(10)]
    public string? PanNo { get; set; }

    /// <summary>
    /// Y - Yes, N - No
    /// </summary>
    [Column("FINAL_GPF_APPLIED_FLAG")]
    [MaxLength(1)]
    public char FinalGpfAppliedFlag { get; set; }

    [Column("FINAL_GPF_APPLIED_DATE")]
    public DateOnly? FinalGpfAppliedDate { get; set; }

    [Column("REASON_FINAL_GPF_NOT_APPLY")]
    [StringLength(400)]
    public string? ReasonFinalGpfNotApply { get; set; }

    /// <summary>
    /// N for Normal, F for Family
    /// </summary>
    [Column("PREVIOUS_PENSION_TYPE")]
    [MaxLength(1)]
    public char? PreviousPensionType { get; set; }

    /// <summary>
    /// M - Military, C - Civil, O - Others
    /// </summary>
    [Column("PREVIOUS_PENSION_SOURCE")]
    [MaxLength(1)]
    public char? PreviousPensionSource { get; set; }

    [Column("PREVIOUS_PENSION_PPO_NO")]
    [StringLength(30)]
    public string? PreviousPensionPpoNo { get; set; }

    [Column("PREVIOUS_PENSION_AMT")]
    public int? PreviousPensionAmt { get; set; }

    [Column("PREVIOUS_PENSION_WEF")]
    public DateOnly? PreviousPensionWef { get; set; }

    [Column("FINAL_GPF_APPLIED_LETTER_NO")]
    [StringLength(30)]
    public string? FinalGpfAppliedLetterNo { get; set; }

    [Column("BSR_CODE")]
    [StringLength(30)]
    public string? BsrCode { get; set; }

    [Column("HRMS_EMP_ID_OBS")]
    [StringLength(30)]
    public string? HrmsEmpIdObs { get; set; }

    [Column("DECLRN_CHECK_FLAG")]
    [MaxLength(1)]
    public char DeclrnCheckFlag { get; set; }

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("USER_ID")]
    public int UserId { get; set; }

    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }

    [Column("PREV_INT_TREASURY_ID")]
    public int? PrevIntTreasuryId { get; set; }

    [Column("PREV_BANK_IFSC_CODE")]
    [StringLength(90)]
    public string? PrevBankIfscCode { get; set; }

    [Column("PREV_PIA")]
    [StringLength(100)]
    public string? PrevPia { get; set; }

    [Column("EDU_QUALIFICATION_OBS")]
    [StringLength(200)]
    public string? EduQualificationObs { get; set; }

    [Column("TRAINING_OBS")]
    [StringLength(400)]
    public string? TrainingObs { get; set; }

    /// <summary>
    /// Unique pension ID type.
    /// </summary>
    [Column("PEN_TYPE_ID")]
    public int? PenTypeId { get; set; }

    [Column("SALUTATION_NAME_OF_RELATIVE_OB")]
    [StringLength(50)]
    public string? SalutationNameOfRelativeOb { get; set; }

    [Column("FIRST_NAME_OF_RELATIVE_OBS")]
    [StringLength(50)]
    public string? FirstNameOfRelativeObs { get; set; }

    [Column("MIDDLE_NAME_OF_RELATIVE_OBS")]
    [StringLength(50)]
    public string? MiddleNameOfRelativeObs { get; set; }

    [Column("LAST_NAME_OF_RELATIVE_OBS")]
    [StringLength(50)]
    public string? LastNameOfRelativeObs { get; set; }

    [Column("INT_DESIGNATION_ID")]
    public int? IntDesignationId { get; set; }

    [Column("INT_POST_ID")]
    public int? IntPostId { get; set; }

    [Column("PENSION_FILE_NO")]
    [StringLength(100)]
    public string? PensionFileNo { get; set; }

    [Column("DESIGNATION_DESC")]
    [StringLength(500)]
    public string? DesignationDesc { get; set; }

    [Column("POST_DESC")]
    [StringLength(500)]
    public string? PostDesc { get; set; }

    [Column("INT_CADRE_ID")]
    public int? IntCadreId { get; set; }

    [Column("CADRE_DESC")]
    [StringLength(500)]
    public string? CadreDesc { get; set; }

    [Column("INT_SRV_ID")]
    public int? IntSrvId { get; set; }

    [Column("SERVICE_TYPE_DESC")]
    [StringLength(500)]
    public string? ServiceTypeDesc { get; set; }

    [Column("INT_EMPLOYMENT_TYPE")]
    public int? IntEmploymentType { get; set; }

    [Column("EMPLOYMENT_TYPE_DESC")]
    [StringLength(500)]
    public string? EmploymentTypeDesc { get; set; }

    [Column("TREASURY_CODE_PAYABLE")]
    [StringLength(4)]
    public string? TreasuryCodePayable { get; set; }

    [Column("INT_EMPLOYEE_ID")]
    public int? IntEmployeeId { get; set; }

    [Column("EMPLOYEE_NO")]
    [StringLength(50)]
    public string? EmployeeNo { get; set; }

    [Column("REQUEST_ID")]
    public int? RequestId { get; set; }

    [Column("TRES_CODE_PAYABLE_OUT_STATE")]
    [StringLength(500)]
    public string? TresCodePayableOutState { get; set; }

    [Column("WB_HEALTH_SCHEME_FLAG")]
    [StringLength(1)]
    public string? WbHealthSchemeFlag { get; set; }

    [Column("HEALTH_SCHEME_TO_BE_CONTINUED")]
    [StringLength(1)]
    public string? HealthSchemeToBeContinued { get; set; }

    [Column("MEMBER_OF_GPF")]
    [StringLength(1)]
    public string? MemberOfGpf { get; set; }

    [Column("GPF_SERIES")]
    [StringLength(30)]
    public string? GpfSeries { get; set; }

    [Column("GPF_SERIES1")]
    [StringLength(12)]
    public string? GpfSeries1 { get; set; }

    [Column("GPF_ACCOUNT_NO")]
    [StringLength(20)]
    public string? GpfAccountNo { get; set; }

    [Column("INT_HEAD_OF_OFFICE_ID")]
    public int? IntHeadOfOfficeId { get; set; }

    [Column("INT_APP_AUTHORITY_ID")]
    public int? IntAppAuthorityId { get; set; }

    [Column("FROM_ESS")]
    [StringLength(1)]
    public string? FromEss { get; set; }

    [Column("INT_PREP_DECLARATION_ID")]
    public int? IntPrepDeclarationId { get; set; }

    [Column("BANK_OBS")]
    [StringLength(300)]
    public string? BankObs { get; set; }

    [Column("BRANCH_NAME_OBS")]
    [StringLength(2000)]
    public string? BranchNameObs { get; set; }

    /// <summary>
    /// no use of this
    /// </summary>
    [Column("BENF_TYPE_ID")]
    public int? BenfTypeId { get; set; }

    [Column("EMP_INT_OMI_GENDER")]
    public int? EmpIntOmiGender { get; set; }

    [Column("MARITAL_STATUS_DESC")]
    [StringLength(300)]
    public string? MaritalStatusDesc { get; set; }

    [Column("RELIGION_DESC")]
    [StringLength(300)]
    public string? ReligionDesc { get; set; }

    [Column("GENDER_DESC")]
    [StringLength(30)]
    public string? GenderDesc { get; set; }

    [Column("INT_PAYABLE_TREA_ID_GRAT_OBS")]
    public int? IntPayableTreaIdGratObs { get; set; }

    [Column("SALUTATION_NAME_OF_EMPL_OBS")]
    [StringLength(50)]
    public string? SalutationNameOfEmplObs { get; set; }

    /// <summary>
    /// B: Bank, I-Treasury, O: outside WB(State ) 
    /// </summary>
    [Column("PAYABLE_TREASURY_FLAG")]
    [StringLength(1)]
    public string? PayableTreasuryFlag { get; set; }

    /// <summary>
    /// For PAYABLE_TREASURY_FLAG &apos;I&apos; Traeasury name &apos;O&apos; State name
    /// </summary>
    [Column("PAYABLE_OUT_TREASURY_NAME")]
    [StringLength(50)]
    public string? PayableOutTreasuryName { get; set; }

    [Column("ROLE_ID")]
    public int? RoleId { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int? ModifiedRoleId { get; set; }

    [Column("APPLICATION_DATE")]
    public DateOnly? ApplicationDate { get; set; }

    /// <summary>
    /// 0 for forenoon and 1 for afternoon
    /// </summary>
    [Column("RETIREMENT_FORE_AFT_NOON")]
    public int? RetirementForeAftNoon { get; set; }

    [Column("NATIONALITY")]
    [StringLength(50)]
    public string? Nationality { get; set; }

    [Column("INT_DEPT_ID")]
    public int? IntDeptId { get; set; }

    [Column("DEPT_DESCRIPTION")]
    [StringLength(100)]
    public string? DeptDescription { get; set; }

    [Column("INT_SANCTIONING_AUTHORITY_ID")]
    public int? IntSanctioningAuthorityId { get; set; }

    /// <summary>
    /// AA --------- Appointing Auth, HOO----------- Head Of Office
    /// </summary>
    [Column("SANCTIONING_AUTHORITY_TYPE")]
    [StringLength(10)]
    public string? SanctioningAuthorityType { get; set; }

    [Column("APPOINTMENT_ADD_HOC_FLAG")]
    [StringLength(1)]
    public string? AppointmentAddHocFlag { get; set; }

    [Column("PREVIOUS_CVP_APPLICATION_FLAG")]
    [StringLength(1)]
    public string? PreviousCvpApplicationFlag { get; set; }

    [Column("PREVIOUS_CVP_APPLI_REMARKS")]
    [StringLength(500)]
    public string? PreviousCvpAppliRemarks { get; set; }

    [Column("PREV_APPEARANCE_MEDICAL_FLAG")]
    [StringLength(1)]
    public string? PrevAppearanceMedicalFlag { get; set; }

    [Column("PREV_APPEAR_MEDICAL_REMARKS")]
    [StringLength(500)]
    public string? PrevAppearMedicalRemarks { get; set; }

    [Column("PPO_NUMBER")]
    [StringLength(30)]
    public string? PpoNumber { get; set; }

    [Column("INT_POST_CODE")]
    public int? IntPostCode { get; set; }

    [Column("POST_CODE")]
    [StringLength(50)]
    public string? PostCode { get; set; }

    [Column("FORWARDING_AUTHORITY_OBS")]
    [StringLength(6)]
    public string? ForwardingAuthorityObs { get; set; }

    [Column("INT_SERVICE_BOOK_AVAIL")]
    public int? IntServiceBookAvail { get; set; }

    /// <summary>
    /// Y/N
    /// </summary>
    [Column("COURT_CASE_PENDING_STATUS")]
    [StringLength(1)]
    public string? CourtCasePendingStatus { get; set; }

    /// <summary>
    /// For Departmental Proceedings &amp; &apos;Judicial&apos; Proceedings.
    /// </summary>
    [Column("COURT_CASE_REMARKS")]
    [StringLength(500)]
    public string? CourtCaseRemarks { get; set; }

    [Column("APPROVER_USER_ID")]
    public int? ApproverUserId { get; set; }

    [Column("APPROVER_ROLE_ID")]
    public int? ApproverRoleId { get; set; }

    [Column("APPROVE_TIME_STAMP", TypeName = "timestamp without time zone")]
    public DateTime? ApproveTimeStamp { get; set; }

    [Column("DATE_OF_JOINING")]
    public DateOnly? DateOfJoining { get; set; }

    [Column("PEN_SANCTION_INT_TREASURY_ID")]
    public int? PenSanctionIntTreasuryId { get; set; }

    [Column("INT_OMI_PF_NPS_TYPE")]
    public int? IntOmiPfNpsType { get; set; }

    [Column("PSA_CODE")]
    [StringLength(40)]
    public string? PsaCode { get; set; }

    [Column("RE_EMPLOYED_AFT_RETIR_REMARKS")]
    [StringLength(500)]
    public string? ReEmployedAftRetirRemarks { get; set; }

    [Column("FAMILY_PENSIONER_EMPLOYED_FLAG")]
    [StringLength(1)]
    public string? FamilyPensionerEmployedFlag { get; set; }

    [Column("FAMILY_PEN_EMPLOYED_REMARKS")]
    [StringLength(500)]
    public string? FamilyPenEmployedRemarks { get; set; }

    [Column("FAMILY_PEN_RCPT_PEN_REMARKS")]
    [StringLength(500)]
    public string? FamilyPenRcptPenRemarks { get; set; }

    /// <summary>
    /// 19
    /// </summary>
    [Column("CPF_SHARE_REMARKS")]
    [StringLength(1000)]
    public string? CpfShareRemarks { get; set; }

    [Column("PROVISIONAL_PENSION_REMARKS")]
    [StringLength(500)]
    public string? ProvisionalPensionRemarks { get; set; }

    [Column("PROVISIONAL_GRATUITY_REMARKS")]
    [StringLength(500)]
    public string? ProvisionalGratuityRemarks { get; set; }

    [Column("APPOINTMENT_ADD_HOC_REMARKS")]
    [StringLength(500)]
    public string? AppointmentAddHocRemarks { get; set; }

    /// <summary>
    /// 22/f
    /// </summary>
    [Column("OTHER_PENSION_RECEIPT_FLAG")]
    [StringLength(1)]
    public string? OtherPensionReceiptFlag { get; set; }

    [Column("OTHER_PENSION_RCPT_NAME")]
    [StringLength(500)]
    public string? OtherPensionRcptName { get; set; }

    [Column("OTHER_PENSION_RCPT_PARTICULAR")]
    [StringLength(500)]
    public string? OtherPensionRcptParticular { get; set; }

    [Column("OTHER_PENSION_RCPT_SOURCE")]
    [StringLength(500)]
    public string? OtherPensionRcptSource { get; set; }

    [Column("VIGILANCE_CASE_PENDING_REMARKS")]
    [StringLength(500)]
    public string? VigilanceCasePendingRemarks { get; set; }

    [Column("DEPT_CRIMINAL_PROC_REMARKS")]
    [StringLength(500)]
    public string? DeptCriminalProcRemarks { get; set; }

    /// <summary>
    /// Y/N
    /// </summary>
    [Column("RE_EMPLOYED_AFTER_RETIRE_FLAG")]
    [StringLength(1)]
    public string? ReEmployedAfterRetireFlag { get; set; }

    [Column("FAMILY_PEN_RCPT_OTH_PEN_FLAG")]
    [StringLength(1)]
    public string? FamilyPenRcptOthPenFlag { get; set; }

    /// <summary>
    /// 35/a
    /// </summary>
    [Column("PROVISIONAL_PEN_RECEIVED_FLAG")]
    [StringLength(1)]
    public string? ProvisionalPenReceivedFlag { get; set; }

    /// <summary>
    /// 35/b
    /// </summary>
    [Column("PROVISIONAL_GRATUITY_RCVD_FLAG")]
    [StringLength(1)]
    public string? ProvisionalGratuityRcvdFlag { get; set; }

    [Column("VIGILANCE_CASE_PENDING_FLAG")]
    [StringLength(1)]
    public string? VigilanceCasePendingFlag { get; set; }

    [Column("DEPT_CRIMINAL_PROC_PENDING_FLG")]
    [StringLength(1)]
    public string? DeptCriminalProcPendingFlg { get; set; }

    [Column("TRES_NAME_PAYABLE_OUT_STATE")]
    [StringLength(500)]
    public string? TresNamePayableOutState { get; set; }

    [Column("TRES_CODE_PAYABLE_OUT_STATE_ID")]
    public int? TresCodePayableOutStateId { get; set; }

    [Column("PSA_TREASURY_MODIFIABLE_FLAG")]
    [StringLength(1)]
    public string PsaTreasuryModifiableFlag { get; set; } = null!;

    /// <summary>
    /// treasury attached with DDO of Service Book HOO
    /// </summary>
    [Column("INT_PSA_TREASURY_CODE")]
    [StringLength(5)]
    public string? IntPsaTreasuryCode { get; set; }

    /// <summary>
    /// SERVICE BOOK HOO
    /// </summary>
    [Column("INT_HEAD_OF_OFFICE_ID_SB")]
    public int? IntHeadOfOfficeIdSb { get; set; }

    /// <summary>
    /// SERVICE BOOK HOO tagged with DDO or Not
    /// </summary>
    [Column("PSA_DDO_AVL_FLAG")]
    [StringLength(1)]
    public string? PsaDdoAvlFlag { get; set; }

    /// <summary>
    /// If family pensioner received any other pension or family pension then that PPO Number to be provided
    /// </summary>
    [Column("OTHER_PPO_NO")]
    [StringLength(30)]
    public string? OtherPpoNo { get; set; }

    /// <summary>
    /// If family pensioner received any other pension or family pension then that PPO Details to be provided
    /// </summary>
    [Column("OTHER_PPO_DETAILS")]
    [StringLength(500)]
    public string? OtherPpoDetails { get; set; }

    /// <summary>
    /// If court case pending is Yes Then W.P(Writ Petition) No to be provided
    /// </summary>
    [Column("WRIT_PETITION_NO")]
    [StringLength(30)]
    public string? WritPetitionNo { get; set; }

    /// <summary>
    /// If court case pending is Yes Then W.P(Writ Petition) Details to be provided
    /// </summary>
    [Column("WRIT_PETITION_DETAILS")]
    [StringLength(500)]
    public string? WritPetitionDetails { get; set; }

    /// <summary>
    /// For Detail Of Processing Status
    /// </summary>
    [Column("PROCESSING_STAGE_DTL")]
    [StringLength(4)]
    public string? ProcessingStageDtl { get; set; }

    /// <summary>
    /// Case Type: &apos;Judicial Proceedings&apos;, �Departmental Proceedings� Taken from OtherMaster Master Type&apos; CCT&apos;
    /// </summary>
    [Column("INT_OMI_COURT_CASE_TYPE")]
    public int? IntOmiCourtCaseType { get; set; }

    /// <summary>
    /// For Judicial Proceedings Only
    /// </summary>
    [Column("COURT_CASE_TYPE")]
    [StringLength(100)]
    public string? CourtCaseType { get; set; }

    /// <summary>
    /// For Judicial Proceedings Only
    /// </summary>
    [Column("COURT_CASE_NO")]
    [StringLength(100)]
    public string? CourtCaseNo { get; set; }

    /// <summary>
    /// For Judicial Proceedings Only
    /// </summary>
    [Column("COURT_CASE_YEAR")]
    public int? CourtCaseYear { get; set; }

    /// <summary>
    /// For Judicial Proceedings Only
    /// </summary>
    [Column("NAME_OF_COURT")]
    [StringLength(200)]
    public string? NameOfCourt { get; set; }

    /// <summary>
    /// For Departmental Proceedings &amp; &apos;Judicial&apos; Proceedings.
    /// </summary>
    [Column("INT_OMI_CASE_STATUS")]
    public int? IntOmiCaseStatus { get; set; }

    /// <summary>
    /// For Judicial Proceedings Only
    /// </summary>
    [Column("INT_OMI_CASE_RELATION_TO")]
    public int? IntOmiCaseRelationTo { get; set; }

    /// <summary>
    /// For Judicial Proceedings Only
    /// </summary>
    [Column("CASE_REL_OTHER_REASON")]
    [StringLength(1000)]
    public string? CaseRelOtherReason { get; set; }

    /// <summary>
    /// For Departmental Proceedings Only
    /// </summary>
    [Column("NAME_OF_THE_AUTHORITY")]
    [StringLength(200)]
    public string? NameOfTheAuthority { get; set; }

    /// <summary>
    /// Remarks For General Purpose
    /// </summary>
    [Column("REMARKS")]
    [StringLength(500)]
    public string? Remarks { get; set; }

    /// <summary>
    /// &apos;F&apos; for Final &apos;P&apos; for Provisional
    /// </summary>
    [Column("PENSION_TYPE_FLAG")]
    [StringLength(1)]
    public string? PensionTypeFlag { get; set; }

    /// <summary>
    /// Types are commomn Superannuation &amp; &apos;Death&apos; &apos;SP&apos; &amp; &apos;DP&apos;
    /// </summary>
    [Column("PENSION_CALC_TYPE")]
    [StringLength(2)]
    public string? PensionCalcType { get; set; }

    /// <summary>
    /// Tagged DDO of Service Book HOO
    /// </summary>
    [Column("INT_PSA_DDO_ID")]
    public int? IntPsaDdoId { get; set; }

    /// <summary>
    /// If Provisional Pension is system calculated then &apos;Y&apos; Else &apos;N&apos;
    /// </summary>
    [Column("PROV_PEN_SYSTEM_RCVD_FLAG")]
    [StringLength(1)]
    public string? ProvPenSystemRcvdFlag { get; set; }

    /// <summary>
    /// If Provisional Gratuity is system calculated then &apos;Y&apos; Else &apos;N&apos;
    /// </summary>
    [Column("PROV_GRAT_SYSTEM_RCVD_FLAG")]
    [StringLength(1)]
    public string? ProvGratSystemRcvdFlag { get; set; }

    /// <summary>
    /// as per CR 181305 Point 1
    /// </summary>
    [Column("RATE_OF_PROV_PENSION")]
    public int? RateOfProvPension { get; set; }

    /// <summary>
    /// as per CR 181305 Point 1
    /// </summary>
    [Column("PROV_PENSION_PERIOD")]
    [StringLength(100)]
    public string? ProvPensionPeriod { get; set; }

    /// <summary>
    /// as per CR 181305 Point 1
    /// </summary>
    [Column("RATE_OF_PROV_GRATUITY")]
    public int? RateOfProvGratuity { get; set; }

    /// <summary>
    /// as per CR 181305 Point 2
    /// </summary>
    [Column("GRATUITY_PAYMENT_ORDER_NO")]
    [StringLength(100)]
    public string? GratuityPaymentOrderNo { get; set; }

    /// <summary>
    /// as per CR 181305 Point 2
    /// </summary>
    [Column("COMMUTED_VALUE_PAY_ORDER_NO")]
    [StringLength(100)]
    public string? CommutedValuePayOrderNo { get; set; }

    /// <summary>
    /// as per CR 208818 Point 1
    /// </summary>
    [Column("APPROVAL_AUTH_DESIGNATION")]
    [StringLength(300)]
    public string? ApprovalAuthDesignation { get; set; }

    /// <summary>
    /// as per CR 275470
    /// </summary>
    [Column("INT_SPOUSE_RELIGION_ID")]
    public int? IntSpouseReligionId { get; set; }

    [Column("INT_PSA_DESIGNATION_ID")]
    public int? IntPsaDesignationId { get; set; }

    [Column("PSA_ADDRESS")]
    [StringLength(1000)]
    public string? PsaAddress { get; set; }

    /// <summary>
    /// as per CR 480156
    /// </summary>
    [Column("TREASURY_BANK")]
    [StringLength(100)]
    public string? TreasuryBank { get; set; }

    /// <summary>
    /// as per CR 480156
    /// </summary>
    [Column("TREASURY_NAME")]
    [StringLength(100)]
    public string? TreasuryName { get; set; }
}
