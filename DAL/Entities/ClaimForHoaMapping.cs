using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("claim_for_hoa_mapping", Schema = "master")]
public partial class ClaimForHoaMapping
{
    [Column("form_code", TypeName = "character varying")]
    public string? FormCode { get; set; }

    [Column("claim_for", TypeName = "character varying")]
    public string? ClaimFor { get; set; }

    [Column("demand", TypeName = "character varying")]
    public string? Demand { get; set; }

    [Column("major_head", TypeName = "character varying")]
    public string? MajorHead { get; set; }

    [Column("sub_major_head", TypeName = "character varying")]
    public string? SubMajorHead { get; set; }

    [Column("minor_head", TypeName = "character varying")]
    public string? MinorHead { get; set; }

    [Column("sub_minor_head", TypeName = "character varying")]
    public string? SubMinorHead { get; set; }

    [Column("scheme_head", TypeName = "character varying")]
    public string? SchemeHead { get; set; }

    [Column("voted_charged", TypeName = "character varying")]
    public string? VotedCharged { get; set; }

    [Column("detail_head", TypeName = "character varying")]
    public string? DetailHead { get; set; }

    [Column("sub_detail_head", TypeName = "character varying")]
    public string? SubDetailHead { get; set; }
}
