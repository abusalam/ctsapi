using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("bt_details", Schema = "master")]
[Index("Id", Name = "idx_name")]
public partial class BtDetail
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("bt_serial")]
    public int? BtSerial { get; set; }

    [Column("desc", TypeName = "character varying")]
    public string? Desc { get; set; }

    [Column("hoa", TypeName = "character varying")]
    public string? Hoa { get; set; }

    [Column("type", TypeName = "character varying")]
    public string? Type { get; set; }

    [Column("demand")]
    [StringLength(2)]
    public string? Demand { get; set; }

    [Column("major")]
    [StringLength(4)]
    public string? Major { get; set; }

    [Column("submajor")]
    [StringLength(2)]
    public string? Submajor { get; set; }

    [Column("minorhead")]
    [StringLength(3)]
    public string? Minorhead { get; set; }

    [Column("schemehead")]
    [StringLength(3)]
    public string? Schemehead { get; set; }

    [Column("detailhead")]
    [StringLength(2)]
    public string? Detailhead { get; set; }

    [Column("subdetail")]
    [StringLength(2)]
    public string? Subdetail { get; set; }

    [Column("planstatus")]
    [StringLength(2)]
    public string? Planstatus { get; set; }

    [Column("votedcharged")]
    [MaxLength(1)]
    public char? Votedcharged { get; set; }

    [Column("created_by")]
    [StringLength(12)]
    public string? CreatedBy { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
