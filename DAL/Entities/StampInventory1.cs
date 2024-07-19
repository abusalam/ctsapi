using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_inventory", Schema = "cts")]
public partial class StampInventory1
{
    [Key]
    [Column("stamp_inventory_id")]
    public long StampInventoryId { get; set; }

    [Column("stamp_category")]
    [StringLength(2)]
    public string? StampCategory { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("denomination")]
    [Precision(10, 2)]
    public decimal? Denomination { get; set; }

    [Column("sheet")]
    public short? Sheet { get; set; }

    [Column("label")]
    public short? Label { get; set; }

    [Column("timestamp", TypeName = "timestamp without time zone")]
    public DateTime? Timestamp { get; set; }

    [Column("is_debit")]
    public bool IsDebit { get; set; }
}
