using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("bank_accounts", Schema = "cts_pension")]
[Index("PpoId", "TreasuryCode", Name = "bank_accounts_ppo_id_treasury_code_key", IsUnique = true)]
public partial class BankAccount
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year")]
    public int FinancialYear { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("ppo_id")]
    public int PpoId { get; set; }

    [Column("account_holder_name")]
    [StringLength(100)]
    public string AccountHolderName { get; set; } = null!;

    [Column("bank_ac_no")]
    [StringLength(30)]
    public string? BankAcNo { get; set; }

    [Column("ifsc_code")]
    [StringLength(11)]
    public string? IfscCode { get; set; }

    [Column("bank_name")]
    [StringLength(100)]
    public string? BankName { get; set; }

    [Column("branch_name")]
    [StringLength(100)]
    public string? BranchName { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [Column("active_flag")]
    public bool? ActiveFlag { get; set; }
}
