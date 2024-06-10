using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("transaction_lot_has_beneficiaries", Schema = "cts_payment")]
[Index("TransactionLotId", Name = "fki_transaction_lot_fky")]
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
    [Precision(10, 2)]
    public decimal? Amount { get; set; }

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

    [ForeignKey("TransactionLotId")]
    [InverseProperty("TransactionLotHasBeneficiaries")]
    public virtual TransactionLot? TransactionLot { get; set; }
}
