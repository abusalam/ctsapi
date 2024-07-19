using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("t_tm_stamp_bill", Schema = "cts")]
public partial class TTmStampBill
{
    [Column("fin_year_from")]
    [Precision(4, 0)]
    public decimal? FinYearFrom { get; set; }

    [Column("int_treasury_code")]
    [StringLength(5)]
    public string? IntTreasuryCode { get; set; }

    [Column("bill_no")]
    [StringLength(30)]
    public string? BillNo { get; set; }

    [Column("bill_date")]
    public DateOnly BillDate { get; set; }

    [Column("sanction_no")]
    [StringLength(30)]
    public string? SanctionNo { get; set; }

    [Column("sanction_date")]
    public DateOnly? SanctionDate { get; set; }

    [Column("sanction_by")]
    [StringLength(30)]
    public string? SanctionBy { get; set; }

    [Column("bill_from_date")]
    public DateOnly? BillFromDate { get; set; }

    [Column("bill_to_date")]
    public DateOnly? BillToDate { get; set; }

    [Column("int_ddo_id")]
    [Precision(20, 0)]
    public decimal? IntDdoId { get; set; }

    [Column("gross_amt")]
    [Precision(20, 0)]
    public decimal? GrossAmt { get; set; }

    [Column("tax_amt")]
    [Precision(20, 0)]
    public decimal? TaxAmt { get; set; }

    [Column("net_amt")]
    [Precision(20, 0)]
    public decimal? NetAmt { get; set; }

    [Column("r_ref_no")]
    [StringLength(50)]
    public string? RRefNo { get; set; }

    [Column("stamp_category_id")]
    [Precision(4, 0)]
    public decimal? StampCategoryId { get; set; }

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

    [Column("token_no")]
    [StringLength(10)]
    public string? TokenNo { get; set; }
}
