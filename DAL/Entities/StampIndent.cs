using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_indent", Schema = "cts")]
public partial class StampIndent
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("stamp_combination_id")]
    public long StampCombinationId { get; set; }

    [Column("memo_number")]
    [StringLength(20)]
    public string MemoNumber { get; set; } = null!;

    [Column("memo_date")]
    public DateTime MemoDate { get; set; }

    [Column("remarks")]
    [StringLength(30)]
    public string? Remarks { get; set; }

    [Column("sheet")]
    public short Sheet { get; set; }

    [Column("label")]
    public short Label { get; set; }

    [Column("quantity")]
    public short Quantity { get; set; }

    [Column("amount")]
    [Precision(10, 2)]
    public decimal Amount { get; set; }

    [Column("status")]
    public short Status { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public long CreatedBy { get; set; }

    [Column("raised_to_treasury_code")]
    [StringLength(3)]
    public string? RaisedToTreasuryCode { get; set; }

    [ForeignKey("StampCombinationId")]
    [InverseProperty("StampIndents")]
    public virtual StampCombination StampCombination { get; set; } = null!;

    [InverseProperty("StampIndent")]
    public virtual ICollection<StampInvoice> StampInvoices { get; set; } = new List<StampInvoice>();
}
