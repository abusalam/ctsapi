using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

/// <summary>
/// Details required for generating forwarding letter report
/// </summary>
[Table("P_MM_EXIT_FORWARDING_LIST", Schema = "cts_pension")]
public partial class PMmExitForwardingList
{
    /// <summary>
    /// primary key of this table
    /// </summary>
    [Key]
    [Column("INT_FRWDING_LIST_ID")]
    public int IntFrwdingListId { get; set; }

    /// <summary>
    /// foreign key from pension master table
    /// </summary>
    [Column("INT_PENSIONER_ID")]
    public int? IntPensionerId { get; set; }

    /// <summary>
    /// name of the reports whose name needs to be displayed in the forwardign letter report
    /// </summary>
    [Column("REPORT_NAME")]
    [StringLength(200)]
    public string ReportName { get; set; } = null!;

    [Column("REPORT_ABBR")]
    [StringLength(5)]
    public string? ReportAbbr { get; set; }

    /// <summary>
    /// value - Y for active and N for inactive
    /// </summary>
    [Column("ACTIVE_FLAG")]
    [StringLength(5)]
    public string ActiveFlag { get; set; } = null!;

    /// <summary>
    /// user id of the person who will be generating the report
    /// </summary>
    [Column("IN_USER_ID")]
    public int InUserId { get; set; }

    /// <summary>
    /// foreign key from pension type table
    /// </summary>
    [Column("PEN_TYPE_ID")]
    public int? PenTypeId { get; set; }

    /// <summary>
    /// abbr for pension type
    /// </summary>
    [Column("PEN_TYPE_ABBR")]
    [StringLength(3)]
    public string? PenTypeAbbr { get; set; }

    /// <summary>
    /// value - Y (when mandatory for all benefit types), O (when mandatory based on a certain benefit type), N - (when not mandatory)
    /// </summary>
    [Column("MANDATORY_FLAG")]
    [StringLength(5)]
    public string? MandatoryFlag { get; set; }

    /// <summary>
    /// time when this record was craeted.
    /// </summary>
    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    /// <summary>
    /// used for showing the list in particular order
    /// </summary>
    [Column("ODR_SL_NO")]
    public int? OdrSlNo { get; set; }
}
