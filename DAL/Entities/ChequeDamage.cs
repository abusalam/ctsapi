using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("cheque_damage", Schema = "cts")]
public partial class ChequeDamage
{
    [Column("id")]
    public short? Id { get; set; }

    [Column("user_id")]
    public short? UserId { get; set; }

    [Column("cheque_entry_id")]
    public long? ChequeEntryId { get; set; }

    [Column("damage_time")]
    public DateTime? DamageTime { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("damage_index", TypeName = "character varying")]
    public string? DamageIndex { get; set; }

    [Column("damage_type", TypeName = "character varying")]
    public string? DamageType { get; set; }
}
