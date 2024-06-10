using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_entry", Schema = "cts")]
public partial class ChequeEntry
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year_id")]
    public short FinancialYearId { get; set; }

    [Column("series_no", TypeName = "character varying")]
    public string SeriesNo { get; set; } = null!;

    [Column("quantity")]
    public short Quantity { get; set; }

    [Column("start")]
    public short Start { get; set; }

    [Column("end")]
    public short End { get; set; }

    [Column("current_position")]
    public short CurrentPosition { get; set; }

    [Column("is_used")]
    public bool IsUsed { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("treasurie_code")]
    [StringLength(3)]
    public string? TreasurieCode { get; set; }

    [Column("micr_code")]
    [StringLength(9)]
    public string? MicrCode { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("available_quantity")]
    public short? AvailableQuantity { get; set; }

    [InverseProperty("ChequeEntry")]
    public virtual ICollection<ChequeInvoiceDetail> ChequeInvoiceDetails { get; set; } = new List<ChequeInvoiceDetail>();
}
