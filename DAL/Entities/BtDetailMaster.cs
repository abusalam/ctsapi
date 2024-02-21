using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("bt_detail_master", Schema = "master")]
public partial class BtDetailMaster
{
    [Column("form_code")]
    public int? FormCode { get; set; }

    [Key]
    [Column("code")]
    public int Code { get; set; }

    [Column(TypeName = "character varying")]
    public string? Description { get; set; }

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

    [Column("sub_detail_head")]
    [StringLength(2)]
    public string? SubDetailHead { get; set; }

    [Column("voted_charged")]
    [StringLength(1)]
    public string? VotedCharged { get; set; }

    [Column("BT_type")]
    [StringLength(40)]
    public string? BtType { get; set; }
}
