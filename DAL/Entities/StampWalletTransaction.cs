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

    [Column("combination_id")]
    public long CombinationId { get; set; }

    [Column("sheet")]
    public short Sheet { get; set; }

    [Column("label")]
    public short Label { get; set; }

    [Column("invoice_id")]
    public long InvoiceId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    public virtual Treasury ToTreasuryCodeNavigation { get; set; } = null!;
}
