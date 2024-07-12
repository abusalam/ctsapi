using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_TD_PEN_PREP_FAMILY_DTL", Schema = "cts_pension")]
public partial class PTdPenPrepFamilyDtl
{
    /// <summary>
    /// unique pension id for the pensioner..
    /// </summary>
    [Column("PEN_FILE_ID_OBS")]
    public int? PenFileIdObs { get; set; }

    /// <summary>
    /// unique family id..
    /// </summary>
    [Key]
    [Column("FAMILY_ID")]
    public long FamilyId { get; set; }

    /// <summary>
    /// name of the family member
    /// </summary>
    [Column("REL_FIRST_NAME")]
    [StringLength(200)]
    public string RelFirstName { get; set; } = null!;

    /// <summary>
    /// relationshp of pensioner with this member..
    /// </summary>
    [Column("INT_REL_ID_OBS")]
    public int? IntRelIdObs { get; set; }

    /// <summary>
    /// gender of the pensioner..
    /// </summary>
    [Column("GENDER_OBS")]
    [MaxLength(1)]
    public char? GenderObs { get; set; }

    /// <summary>
    /// date of birth of pensioner..
    /// </summary>
    [Column("DOB")]
    public DateOnly? Dob { get; set; }

    /// <summary>
    /// mariatial status of the relationship of pensionar
    /// </summary>
    [Column("INT_MARITAL_STATUS_ID")]
    public int? IntMaritalStatusId { get; set; }

    /// <summary>
    /// EFP up to date..
    /// </summary>
    [Column("EFP_UPTO")]
    public DateOnly? EfpUpto { get; set; }

    /// <summary>
    /// Share percentage of the family member..
    /// </summary>
    [Column("SHARE_PERCENTAGE_OBS")]
    public int? SharePercentageObs { get; set; }

    /// <summary>
    /// Whether this member is handicapped..
    /// </summary>
    [Column("HANDICAPP_FLAG_OBS")]
    [MaxLength(1)]
    public char? HandicappFlagObs { get; set; }

    /// <summary>
    /// whether this member is minor..
    /// </summary>
    [Column("MINOR_FLAG_OBS")]
    [MaxLength(1)]
    public char? MinorFlagObs { get; set; }

    [Column("MINOR_FLAG_CALCULATED_ON_OBS")]
    public DateOnly? MinorFlagCalculatedOnObs { get; set; }

    /// <summary>
    /// bank account id..
    /// </summary>
    [Column("BANK_AC_NO_OBS")]
    [StringLength(30)]
    public string? BankAcNoObs { get; set; }

    [Column("IFSC_CODE_OBS")]
    [StringLength(11)]
    public string? IfscCodeObs { get; set; }

    /// <summary>
    /// &apos;P&apos; for Physically Handicapped and &apos;M&apos; for Mentally Handicapped
    /// </summary>
    [Column("HANDICAPP_TYPE_OBS")]
    [MaxLength(1)]
    public char? HandicappTypeObs { get; set; }

    [Column("REMARKS")]
    [StringLength(200)]
    public string? Remarks { get; set; }

    [Column("GUARDIAN_NAME_OBS")]
    [StringLength(200)]
    public string? GuardianNameObs { get; set; }

    [Column("MOBILE_NUMBER")]
    [StringLength(11)]
    public string? MobileNumber { get; set; }

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

    [Column("SALUTATION_FAMILY_OBS")]
    [StringLength(50)]
    public string? SalutationFamilyObs { get; set; }

    [Column("SALUTATION_FAMILY_GUARDIAN_OBS")]
    [StringLength(50)]
    public string? SalutationFamilyGuardianObs { get; set; }

    [Column("TI_APPLICABLE_FLAG_OBS")]
    [MaxLength(1)]
    public char? TiApplicableFlagObs { get; set; }

    /// <summary>
    /// pensioner id which is unique
    /// </summary>
    [Column("INT_PENSIONER_ID")]
    public int? IntPensionerId { get; set; }

    [Column("INT_EMP_RELATIONSHIP_ID")]
    public int? IntEmpRelationshipId { get; set; }

    /// <summary>
    /// RL type in other master
    /// </summary>
    [Column("REL_INT_OMI_RELATIONSHIP")]
    public int? RelIntOmiRelationship { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string? ActiveFlag { get; set; }

    /// <summary>
    /// PHC in Otehr Master
    /// </summary>
    [Column("REL_INT_OMI_PHYSIC_CHALLENGED")]
    public int? RelIntOmiPhysicChallenged { get; set; }

    [Column("REL_PHYSIC_CHALLENGED_PERC_VH")]
    public int? RelPhysicChallengedPercVh { get; set; }

    [Column("REL_PHYSIC_CHALLENGED_PERC_PH")]
    public int? RelPhysicChallengedPercPh { get; set; }

    [Column("REQUEST_ID")]
    public int? RequestId { get; set; }

    [Column("WBHS_CARD_NUMBER")]
    [StringLength(50)]
    public string? WbhsCardNumber { get; set; }

    [Column("HEALTH_INSURANCE_TYPE")]
    [StringLength(1)]
    public string? HealthInsuranceType { get; set; }

    [Column("RELATIONSHIP_EMP_NO")]
    [StringLength(50)]
    public string? RelationshipEmpNo { get; set; }

    [Column("INT_EMPLOYEE_ID")]
    public int? IntEmployeeId { get; set; }

    [Column("CREATED_ROLE_ID")]
    public int? CreatedRoleId { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int? ModifiedRoleId { get; set; }

    /// <summary>
    /// &apos;Y&apos; for alive &apos;N&apos; for dead
    /// </summary>
    [Column("DECEASED_FLAG")]
    [StringLength(1)]
    public string DeceasedFlag { get; set; } = null!;

    /// <summary>
    /// &apos;Y&apos; for family pension admisible else &apos;N&apos;
    /// </summary>
    [Column("FAMILY_PENSION_FLAG")]
    [StringLength(1)]
    public string FamilyPensionFlag { get; set; } = null!;

    /// <summary>
    /// Added for Requirement of Exit Management
    /// </summary>
    [Column("MARRIAGE_DATE")]
    public DateOnly? MarriageDate { get; set; }

    [Column("REL_EXACT_HEIGHT")]
    public int? RelExactHeight { get; set; }

    [Column("INT_REL_OMI_HEIGHT_UNIT")]
    public int? IntRelOmiHeightUnit { get; set; }

    [Column("REL_IDENTIFICATION_MARK")]
    [StringLength(200)]
    public string? RelIdentificationMark { get; set; }
}
