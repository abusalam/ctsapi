using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_TD_PEN_PREP_FAMILY_ADDRESS", Schema = "cts_pension")]
public partial class PTdPenPrepFamilyAddress
{
    [Key]
    [Column("INT_FAMILY_ADDR_ID")]
    public int IntFamilyAddrId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int? IntPensionerId { get; set; }

    [Column("FAMILY_ID")]
    public int FamilyId { get; set; }

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

    [Column("WEF", TypeName = "timestamp without time zone")]
    public DateTime? Wef { get; set; }

    [Column("ADDR_TYPE")]
    [StringLength(5)]
    public string? AddrType { get; set; }

    [Column("SAME_AS_PERMANENT_ADDR")]
    [StringLength(5)]
    public string? SameAsPermanentAddr { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string ActiveFlag { get; set; } = null!;

    [Column("CREATED_USER_ID")]
    public int CreatedUserId { get; set; }

    [Column("CREATED_TIMESTAMP")]
    public DateOnly CreatedTimestamp { get; set; }

    [Column("CREATED_ROLE_ID")]
    public int CreatedRoleId { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP")]
    public DateOnly ModifiedTimestamp { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int ModifiedRoleId { get; set; }

    [Column("EFFECTIVE_END_DATE")]
    public DateOnly? EffectiveEndDate { get; set; }

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("REQUEST_ID")]
    public int? RequestId { get; set; }

    [Column("INT_RELATION_ADDR_ID")]
    public int? IntRelationAddrId { get; set; }
}
