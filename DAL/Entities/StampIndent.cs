using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_indent", Schema = "cts")]
[Index("MemoNumber", Name = "stamp_indent_memo_number_memo_number1_key", IsUnique = true)]
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

    [Column("status_id")]
    public int StatusId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public long CreatedBy { get; set; }

    [Column("raised_to_treasury_code")]
    [StringLength(3)]
    public string? RaisedToTreasuryCode { get; set; }

    [Column("raised_by_treasury_code")]
    [StringLength(3)]
    public string? RaisedByTreasuryCode { get; set; }

    [ForeignKey("StampCombinationId")]
    [InverseProperty("StampIndents")]
    public virtual StampCombination StampCombination { get; set; } = null!;

    [InverseProperty("StampIndent")]
    public virtual ICollection<StampInvoice> StampInvoices { get; set; } = new List<StampInvoice>();

    [ForeignKey("StatusId")]
    [InverseProperty("StampIndents")]
    public virtual Status Status { get; set; } = null!;
}
