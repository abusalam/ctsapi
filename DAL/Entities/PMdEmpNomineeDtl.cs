using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_EMP_NOMINEE_DTL", Schema = "cts_pension")]
public partial class PMdEmpNomineeDtl
{
    [Key]
    [Column("INT_PEN_NOM_ID")]
    public long IntPenNomId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("INT_NOM_ID")]
    public int IntNomId { get; set; }

    [Column("INT_EMPLOYEE_ID")]
    public int IntEmployeeId { get; set; }

    [Column("NOMINEE_NAME")]
    [StringLength(200)]
    public string NomineeName { get; set; } = null!;

    [Column("GENDER")]
    [MaxLength(1)]
    public char? Gender { get; set; }

    [Column("INT_MARITAL_STATUS_ID")]
    public int? IntMaritalStatusId { get; set; }

    [Column("DATE_OF_BIRTH")]
    public DateOnly DateOfBirth { get; set; }

    [Column("MINOR_FLAG")]
    [MaxLength(1)]
    public char MinorFlag { get; set; }

    [Column("GUARDIAN_NAME")]
    [StringLength(200)]
    public string? GuardianName { get; set; }

    [Column("IDENTIFICATION_MARK")]
    [StringLength(100)]
    public string? IdentificationMark { get; set; }

    [Column("SHARE_PERCENTAGE")]
    public int? SharePercentage { get; set; }

    [Column("BANK_AC_NO")]
    [StringLength(30)]
    public string? BankAcNo { get; set; }

    [Column("IFSC_CODE")]
    [StringLength(11)]
    public string? IfscCode { get; set; }

    [Column("PRIORITY_LEVEL")]
    public int PriorityLevel { get; set; }

    [Column("MINOR_FLAG_CALCULATED_ON", TypeName = "timestamp without time zone")]
    public DateTime? MinorFlagCalculatedOn { get; set; }

    [Column("BENF_TYPE_ID")]
    public int BenfTypeId { get; set; }

    [Column("NOMINEE_TYPE")]
    [MaxLength(1)]
    public char NomineeType { get; set; }

    [Column("MOBILE_NUMBER")]
    [StringLength(11)]
    public string? MobileNumber { get; set; }

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("USER_ID")]
    public int UserId { get; set; }

    /// <summary>
    /// nominee creation and application date -- added by ritu
    /// </summary>
    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }

    [Column("INT_EMP_RELATIONSHIP_ID")]
    public int? IntEmpRelationshipId { get; set; }

    [Column("NOM_INT_OMI_PHYSIC_CHALLENGED")]
    public int? NomIntOmiPhysicChallenged { get; set; }

    [Column("NOM_PHYSIC_CHALLENGED_PERC_VH")]
    public int? NomPhysicChallengedPercVh { get; set; }

    [Column("NOM_PHYSIC_CHALLENGED_PERC_PH")]
    public int? NomPhysicChallengedPercPh { get; set; }

    [Column("INT_RELATION_ADDR_ID")]
    public int? IntRelationAddrId { get; set; }

    [Column("HOUSE_NO_STREET_LANE")]
    [StringLength(100)]
    public string? HouseNoStreetLane { get; set; }

    [Column("CITY_TOWN_VILLAGE")]
    [StringLength(100)]
    public string? CityTownVillage { get; set; }

    [Column("POST_OFFICE")]
    [StringLength(100)]
    public string? PostOffice { get; set; }

    [Column("POLICE_STATION")]
    [StringLength(100)]
    public string? PoliceStation { get; set; }

    [Column("STATE_ID")]
    public int? StateId { get; set; }

    [Column("INT_DISTRICT_ID")]
    public int? IntDistrictId { get; set; }

    [Column("PIN")]
    [StringLength(10)]
    public string? Pin { get; set; }

    [Column("GUARDIAN_ADDR")]
    [StringLength(500)]
    public string? GuardianAddr { get; set; }

    /// <summary>
    /// Relation In - 0, out - 1 --added by ritu
    /// </summary>
    [Column("RELATION_IN_OUT")]
    [StringLength(1)]
    public string? RelationInOut { get; set; }

    [Column("ROLE_ID")]
    public int? RoleId { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int? ModifiedRoleId { get; set; }

    [Column("REQUEST_ID")]
    public int? RequestId { get; set; }

    /// <summary>
    /// original nominee id for whom this person is an alternate --added by ritu
    /// </summary>
    [Column("ORG_NOM_ID_FOR_ALTERNATE")]
    public int? OrgNomIdForAlternate { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string? ActiveFlag { get; set; }

    [Column("DECLARATION_FOR_NOMINEE")]
    [StringLength(300)]
    public string? DeclarationForNominee { get; set; }

    [Column("ESE_FLAG")]
    [StringLength(1)]
    public string? EseFlag { get; set; }

    /// <summary>
    /// 0 initiate, 1    approved, -1 rejected
    /// </summary>
    [Column("PROCESSING_FLAG")]
    public int? ProcessingFlag { get; set; }

    [Column("EMAIL_ID")]
    [StringLength(100)]
    public string? EmailId { get; set; }

    [Column("NOM_CONTINGENCY")]
    [StringLength(500)]
    public string? NomContingency { get; set; }

    [Column("REMARKS")]
    [StringLength(500)]
    public string? Remarks { get; set; }

    /// <summary>
    /// reason for relation - out --added by ritu
    /// </summary>
    [Column("REASON")]
    [StringLength(500)]
    public string? Reason { get; set; }

    /// <summary>
    /// Nominee created after death of employee Y/N
    /// </summary>
    [Column("CREATION_AFTER_DEATH_FLAG")]
    [StringLength(1)]
    public string? CreationAfterDeathFlag { get; set; }

    /// <summary>
    /// &apos;Y&apos; for alive &apos;N&apos; for dead
    /// </summary>
    [Column("NOM_DECEASED_FLAG")]
    [StringLength(1)]
    public string NomDeceasedFlag { get; set; } = null!;

    /// <summary>
    /// Nominee age on created date for report
    /// </summary>
    [Column("AGE_ON_CREATED_DATE")]
    [StringLength(100)]
    public string? AgeOnCreatedDate { get; set; }

    [Column("OUT_REL_DESC")]
    [StringLength(500)]
    public string? OutRelDesc { get; set; }

    /// <summary>
    /// N - nominee L - Legal Hier --added by pk pandit
    /// </summary>
    [Column("NOMINEE_LEGAL_HIRE_FLAG")]
    [StringLength(1)]
    public string NomineeLegalHireFlag { get; set; } = null!;
}
