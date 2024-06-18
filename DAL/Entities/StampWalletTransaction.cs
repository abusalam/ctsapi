using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_wallet_transaction", Schema = "master")]
public partial class StampWalletTransaction
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("from_treasury_code")]
    [StringLength(3)]
    public string? FromTreasuryCode { get; set; }

    [Column("to_treasury_code")]
    [StringLength(3)]
    public string ToTreasuryCode { get; set; } = null!;

    [Column("stamp_number")]
    public short StampNumber { get; set; }

    public virtual Treasury ToTreasuryCodeNavigation { get; set; } = null!;
}
