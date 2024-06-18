using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_wallet", Schema = "master")]
[Index("TreasuryCode", Name = "stamp_wallet_treasury_code_treasury_code1_key", IsUnique = true)]
public partial class StampWallet
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("clear_balance")]
    public short ClearBalance { get; set; }

    [Column("ledger_balance")]
    public short LedgerBalance { get; set; }
}
