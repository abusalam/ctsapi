using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("t_tm_stamp_stock_summary", Schema = "cts")]
public partial class TTmStampStockSummary
{
    [Column("fin_year_from")]
    [Precision(4, 0)]
    public decimal? FinYearFrom { get; set; }

    [Column("int_treasury_code")]
    [StringLength(5)]
    public string? IntTreasuryCode { get; set; }

    [Column("transaction_date")]
    public DateOnly TransactionDate { get; set; }

    [Column("stamp_category_id")]
    [Precision(4, 0)]
    public decimal? StampCategoryId { get; set; }

    [Column("opening_stock")]
    [Precision(9, 0)]
    public decimal? OpeningStock { get; set; }

    [Column("stock_in")]
    [Precision(9, 0)]
    public decimal? StockIn { get; set; }

    [Column("stock_out")]
    [Precision(9, 0)]
    public decimal? StockOut { get; set; }

    [Column("trans_id")]
    [Precision(18, 0)]
    public decimal? TransId { get; set; }

    [Column("dml_status_flag")]
    [Precision(1, 0)]
    public decimal? DmlStatusFlag { get; set; }

    [Column("user_id")]
    [Precision(8, 0)]
    public decimal? UserId { get; set; }

    [Column("created_timestamp")]
    public DateOnly CreatedTimestamp { get; set; }

    [Column("modified_user_id")]
    [Precision(8, 0)]
    public decimal? ModifiedUserId { get; set; }

    [Column("modified_timestamp")]
    public DateOnly ModifiedTimestamp { get; set; }

    [Column("denomination")]
    [Precision(10, 2)]
    public decimal? Denomination { get; set; }

    [Column("label_id")]
    [Precision(3, 0)]
    public decimal? LabelId { get; set; }
}
