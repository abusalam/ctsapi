using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_damage", Schema = "cts")]
public partial class ChequeDamage
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("damage_index", TypeName = "character varying")]
    public string? DamageIndex { get; set; }

    [Column("damage_type", TypeName = "character varying")]
    public string? DamageType { get; set; }

    [Column("user_id")]
    public short? UserId { get; set; }

    [Column("cheque_invoice_details_id")]
    public long? ChequeInvoiceDetailsId { get; set; }

    [Column("damage_time", TypeName = "timestamp without time zone")]
    public DateTime? DamageTime { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("series_no", TypeName = "character varying")]
    public string? SeriesNo { get; set; }

    [Column("damage_status", TypeName = "character varying")]
    public string? DamageStatus { get; set; }
}
