using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
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

    [Column("pensioner_id")]
    public long PensionerId { get; set; }

    [Column("ppo_id")]
    public int PpoId { get; set; }

    [Column("account_holder_name")]
    [StringLength(100)]
    public string AccountHolderName { get; set; } = null!;

    [Column("pay_mode")]
    [MaxLength(1)]
    public char PayMode { get; set; }

    [Column("bank_ac_no")]
    [StringLength(30)]
    public string? BankAcNo { get; set; }

    [Column("ifsc_code")]
    [StringLength(11)]
    public string? IfscCode { get; set; }

    [Column("bank_code")]
    public long? BankCode { get; set; }

    [Column("branch_code")]
    public long? BranchCode { get; set; }

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

    [ForeignKey("PensionerId")]
    [InverseProperty("BankAccounts")]
    public virtual Pensioner Pensioner { get; set; } = null!;

    [InverseProperty("BankAccount")]
    public virtual ICollection<PpoBill> PpoBills { get; set; } = new List<PpoBill>();
}
