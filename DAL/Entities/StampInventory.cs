using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_inventory", Schema = "master")]
public partial class StampInventory
{
    [Key]
    [Column("stamp_inventory_id")]
    public long StampInventoryId { get; set; }

    [Column("combination_id")]
    public long CombinationId { get; set; }

    [Column("month")]
    public short Month { get; set; }

    [Column("year")]
    public short Year { get; set; }

    [Column("clear_sheet_balance")]
    public short ClearSheetBalance { get; set; }

    [Column("clear_label_balance")]
    public short ClearLabelBalance { get; set; }
}
