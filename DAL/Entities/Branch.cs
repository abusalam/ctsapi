using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("branchs", Schema = "cts-bank-master")]
[Index("BankCode", Name = "fki_bank_code_fkey")]
public partial class Branch
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("city_code")]
    public short CityCode { get; set; }

    [Column("bank_code")]
    public short BankCode { get; set; }

    [Column("branch_code")]
    public short BranchCode { get; set; }

    [Column("micr_code")]
    [StringLength(9)]
    public string? MicrCode { get; set; }

    [Column("branch_name")]
    [StringLength(100)]
    public string BranchName { get; set; } = null!;

    [Column("address")]
    [StringLength(100)]
    public string? Address { get; set; }

    [Column("city_name")]
    [StringLength(100)]
    public string? CityName { get; set; }

    [Column("pincode")]
    [StringLength(6)]
    public string? Pincode { get; set; }

    [Column("state")]
    [StringLength(100)]
    public string? State { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    public virtual Bank BankCodeNavigation { get; set; } = null!;

    [InverseProperty("Branchs")]
    public virtual ICollection<TreasuryHasBranch> TreasuryHasBranches { get; set; } = new List<TreasuryHasBranch>();
}
