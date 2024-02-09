using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("token_has_objections", Schema = "cts")]
[Index("GobalObjectionId", Name = "fki_o")]
[Index("LocalObjection", Name = "fki_token_has_objection_local_objection_id_fkey")]
[Index("TokenId", Name = "fki_token_has_objection_token_id_fkey")]
public partial class TokenHasObjection
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("token_id")]
    public long TokenId { get; set; }

    [Column("gobal_objection_id")]
    public int? GobalObjectionId { get; set; }

    [Column("local_objection", TypeName = "character varying")]
    public string? LocalObjection { get; set; }

    [Column("objected_by")]
    public long? ObjectedBy { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("overruled_by")]
    public long? OverruledBy { get; set; }

    [Column("overruled_at", TypeName = "timestamp without time zone")]
    public DateTime? OverruledAt { get; set; }

    [Column("overruled_remarks", TypeName = "character varying")]
    public string? OverruledRemarks { get; set; }

    [Column("objection_remark", TypeName = "character varying")]
    public string? ObjectionRemark { get; set; }

    [ForeignKey("GobalObjectionId")]
    [InverseProperty("TokenHasObjections")]
    public virtual GobalObjection? GobalObjection { get; set; }

    [ForeignKey("TokenId")]
    [InverseProperty("TokenHasObjections")]
    public virtual Token Token { get; set; } = null!;
}
