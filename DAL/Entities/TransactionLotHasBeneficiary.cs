using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("transaction_lot_has_beneficiaries", Schema = "cts_payment")]
public partial class TransactionLotHasBeneficiary
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("transaction_lot_id")]
    public long? TransactionLotId { get; set; }

    [Column("financial_year_id")]
    public short? FinancialYearId { get; set; }

    [Column("amount")]
    public int? Amount { get; set; }

    [Column("beneficiary_name", TypeName = "character varying")]
    public string? BeneficiaryName { get; set; }

    [Column("account_number")]
    [StringLength(20)]
    public string? AccountNumber { get; set; }

    [Column("ifsc_code")]
    [StringLength(11)]
    public string? IfscCode { get; set; }

    [Column("mobile_no")]
    [StringLength(10)]
    public string? MobileNo { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("response", TypeName = "character varying")]
    public string? Response { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
