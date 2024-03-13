using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("payment_advice_has_beneficiarys", Schema = "cts")]
public partial class PaymentAdviceHasBeneficiary
{
    [Column("id")]
    public long Id { get; set; }

    [Column("payment_advice_id")]
    public long PaymentAdviceId { get; set; }

    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("payee_name")]
    [StringLength(100)]
    public string? PayeeName { get; set; }

    [Column("beneficiary_id")]
    [StringLength(20)]
    public string? BeneficiaryId { get; set; }

    [Column("payee_type")]
    public short? PayeeType { get; set; }

    [Column("pan_no")]
    [StringLength(10)]
    public string? PanNo { get; set; }

    [Column("contact_number")]
    [StringLength(15)]
    public string? ContactNumber { get; set; }

    [Column("beneficiary_type")]
    public short? BeneficiaryType { get; set; }

    [Column("address")]
    [StringLength(200)]
    public string? Address { get; set; }

    [Column("email")]
    [StringLength(60)]
    public string? Email { get; set; }

    [Column("ifsc_code")]
    [StringLength(15)]
    public string? IfscCode { get; set; }

    [Column("account_type")]
    public short? AccountType { get; set; }

    [Column("bank_account_number")]
    [StringLength(20)]
    public string? BankAccountNumber { get; set; }

    [Column("bank_name")]
    [StringLength(50)]
    public string? BankName { get; set; }

    [Column("amount")]
    [Precision(10, 2)]
    public decimal? Amount { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
