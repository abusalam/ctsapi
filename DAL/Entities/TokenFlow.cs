using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("token_flow", Schema = "cts")]
[Index("ReferenceNo", Name = "fki_token_flow_reference_no_fkey")]
[Index("StatusId", Name = "fki_token_flow_status_id_fkey")]
[Index("TokeId", Name = "fki_token_flow_token_id_fkey")]
public partial class TokenFlow
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("toke_id")]
    public long TokeId { get; set; }

    [Column("reference_no")]
    [StringLength(20)]
    public string? ReferenceNo { get; set; }

    [Column("status_id")]
    public int StatusId { get; set; }

    [Column("action_taken_by")]
    public long ActionTakenBy { get; set; }

    [Column("action_taken_at", TypeName = "timestamp without time zone")]
    public DateTime ActionTakenAt { get; set; }

    /// <summary>
    /// 1 = Front Office Clerk
    /// 2 = Accountant
    /// 3 = Treasury Officer
    /// </summary>
    [Column("token_owner_type")]
    public short? TokenOwnerType { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("TokenFlows")]
    public virtual Status Status { get; set; } = null!;

    [ForeignKey("TokeId")]
    [InverseProperty("TokenFlows")]
    public virtual Token Toke { get; set; } = null!;

    [InverseProperty("TokenFlow")]
    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
