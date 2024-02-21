using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("operator_code", Schema = "master")]
public partial class OperatorCode
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("treasury_codeno", TypeName = "character varying")]
    public string? TreasuryCodeno { get; set; }

    [Column("treasury_code", TypeName = "character varying")]
    public string? TreasuryCode { get; set; }

    [Column("operator_id")]
    public int? OperatorId { get; set; }

    [Column("operator_name", TypeName = "character varying")]
    public string? OperatorName { get; set; }

    [Column("operator_type", TypeName = "character varying")]
    public string? OperatorType { get; set; }

    [Column("demand")]
    [StringLength(2)]
    public string? Demand { get; set; }

    [Column("major_head")]
    [StringLength(4)]
    public string? MajorHead { get; set; }

    [Column("sub_major_head")]
    [StringLength(2)]
    public string? SubMajorHead { get; set; }

    [Column("minor_head")]
    [StringLength(3)]
    public string? MinorHead { get; set; }

    [Column("plan_status")]
    [StringLength(2)]
    public string? PlanStatus { get; set; }

    [Column("scheme_head")]
    [StringLength(3)]
    public string? SchemeHead { get; set; }

    [Column("detail_head")]
    [StringLength(2)]
    public string? DetailHead { get; set; }

    [Column("sub_detailed_head")]
    [StringLength(2)]
    public string? SubDetailedHead { get; set; }

    [Column("voted_charge")]
    [StringLength(1)]
    public string? VotedCharge { get; set; }
}
