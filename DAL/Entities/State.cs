using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("state", Schema = "master")]
public partial class State
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("state_code")]
    [StringLength(3)]
    public string? StateCode { get; set; }

    [Column("state")]
    public string? State1 { get; set; }

    [Column("active_flag")]
    public bool? ActiveFlag { get; set; }

    [Column("created_by")]
    [StringLength(12)]
    public string? CreatedBy { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
