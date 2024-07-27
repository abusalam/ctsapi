using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_wallet", Schema = "master")]
public partial class StampWallet
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("sheet_clear_balance")]
    public short SheetClearBalance { get; set; }

    [Column("sheet_ledger_balance")]
    public short SheetLedgerBalance { get; set; }

    [Column("combination_id")]
    public long CombinationId { get; set; }

    [Column("label_clear_balance")]
    public short LabelClearBalance { get; set; }

    [Column("label_ledger_balance")]
    public short LabelLedgerBalance { get; set; }

    [ForeignKey("CombinationId")]
    [InverseProperty("StampWallets")]
    public virtual StampCombination Combination { get; set; } = null!;
}
