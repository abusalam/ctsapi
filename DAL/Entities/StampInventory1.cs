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

    [Column("day")]
    public short? Day { get; set; }

    [Column("month")]
    public short? Month { get; set; }

    [Column("year")]
    public int? Year { get; set; }

    [Column("closing_stock")]
    public decimal? ClosingStock { get; set; }
}
