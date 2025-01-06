using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("eppo_amounts", Schema = "cts_pension")]
public partial class EppoAmount
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    /// <summary>
    /// CLS - Classification; EFP - Enhanced Family Pension; BSC - Basic Pension; NFP - Normal Family Pension; BYT - By Transfer;
    /// </summary>
    [Column("amount_type")]
    [StringLength(3)]
    public string AmountType { get; set; } = null!;

    [Column("classification_id")]
    public long? ClassificationId { get; set; }

    [Column("from_date")]
    public DateOnly? FromDate { get; set; }

    [Column("to_date")]
    public DateOnly? ToDate { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [Column("consolidated")]
    public bool Consolidated { get; set; }

    [Column("category_id")]
    public long? CategoryId { get; set; }

    [Column("eppo_receipt_id")]
    public long EppoReceiptId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [Column("active_flag")]
    public bool ActiveFlag { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("EppoAmounts")]
    public virtual Category? Category { get; set; }

    [ForeignKey("ClassificationId")]
    [InverseProperty("EppoAmounts")]
    public virtual Classification? Classification { get; set; }

    [ForeignKey("EppoReceiptId")]
    [InverseProperty("EppoAmounts")]
    public virtual EppoReceipt EppoReceipt { get; set; } = null!;
}
