using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("banks", Schema = "cts_pension")]
[Index("BankName", Name = "banks_bank_name_key", IsUnique = true)]
public partial class Bank
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("bank_name")]
    [StringLength(100)]
    public string BankName { get; set; } = null!;

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

    [InverseProperty("Bank")]
    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
