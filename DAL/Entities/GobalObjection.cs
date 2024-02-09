using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("gobal_objection", Schema = "cts_master")]
public partial class GobalObjection
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("description", TypeName = "character varying")]
    public string Description { get; set; } = null!;

    [Column("status")]
    public short Status { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public long CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public long? UpdatedBy { get; set; }

    [InverseProperty("GobalObjection")]
    public virtual ICollection<TokenHasObjection> TokenHasObjections { get; set; } = new List<TokenHasObjection>();
}
