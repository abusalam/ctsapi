using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PENSIONER_ADDRESS", Schema = "cts_pension")]
public partial class PMdPensionerAddress
{
    [Key]
    [Column("INT_PENSIONER_ADDR_ID")]
    public int IntPensionerAddrId { get; set; }

    [Column("INT_EMPLOYEE_ID")]
    public int IntEmployeeId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

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

    [Column("WEF")]
    public DateOnly? Wef { get; set; }

    /// <summary>
    /// PM for Permanent, PR for present CM for communication address
    /// </summary>
    [Column("ADDR_TYPE")]
    [StringLength(5)]
    public string AddrType { get; set; } = null!;

    /// <summary>
    /// N or Y
    /// </summary>
    [Column("SAME_AS_PERMANENT_ADDR")]
    [StringLength(5)]
    public string? SameAsPermanentAddr { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string ActiveFlag { get; set; } = null!;

    [Column("CREATED_USER_ID")]
    public int CreatedUserId { get; set; }

    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }

    [Column("EFFECTIVE_END_DATE")]
    public DateOnly? EffectiveEndDate { get; set; }

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("REQUEST_ID")]
    public int? RequestId { get; set; }

    [Column("COUNTRY_ID")]
    public int? CountryId { get; set; }

    [Column("ROLE_ID")]
    public int? RoleId { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int? ModifiedRoleId { get; set; }
}
