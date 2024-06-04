using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("transaction_lot", Schema = "cts_payment")]
public partial class TransactionLot
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("lot_no")]
    [StringLength(10)]
    public string? LotNo { get; set; }

    [Column("drn_no")]
    [StringLength(20)]
    public string? DrnNo { get; set; }

    [Column("financial_year_id")]
    public short? FinancialYearId { get; set; }

    [Column("number_of_beneficiary")]
    public short? NumberOfBeneficiary { get; set; }

    [Column("number_of_success")]
    public int? NumberOfSuccess { get; set; }

    [Column("number_of_failed")]
    public int? NumberOfFailed { get; set; }

    [Column("total_amount")]
    public int? TotalAmount { get; set; }

    [Column("debit_amount")]
    public int? DebitAmount { get; set; }

    /// <summary>
    /// 1=lot generate
    /// </summary>
    [Column("status")]
    public short? Status { get; set; }

    [Column("voucher_no")]
    public int? VoucherNo { get; set; }

    [Column("voucher_date")]
    public DateOnly? VoucherDate { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
