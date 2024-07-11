using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_RULE_NAME", Schema = "cts_pension")]
[Index("RuleName", Name = "UK_P_MM_PEN_RULE_NAME1", IsUnique = true)]
[Index("RuleAbbr", Name = "UK_P_MM_PEN_RULE_NAME2", IsUnique = true)]
public partial class PMmPenRuleName
{
    [Key]
    [Column("INT_PEN_RULE_NAME_ID")]
    public int IntPenRuleNameId { get; set; }

    [Column("INT_SERVICE_ID")]
    public int IntServiceId { get; set; }

    [Column("RULE_NAME")]
    [StringLength(300)]
    public string RuleName { get; set; } = null!;

    [Column("RULE_ABBR")]
    [StringLength(10)]
    public string RuleAbbr { get; set; } = null!;

    [Column("BENF_TYPE_ID")]
    public int BenfTypeId { get; set; }

    [Column("PEN_TYPE_ID")]
    public int PenTypeId { get; set; }

    [Column("WEF")]
    public DateOnly Wef { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string ActiveFlag { get; set; } = null!;

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("CREATED_USER_ID")]
    public int CreatedUserId { get; set; }

    [Column("CREATED_TIME_STAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimeStamp { get; set; }

    [Column("CREATED_ROLE_ID")]
    public int CreatedRoleId { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int? ModifiedUserId { get; set; }

    [Column("MODIFIED_TIME_STAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimeStamp { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int? ModifiedRoleId { get; set; }
}
