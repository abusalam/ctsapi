using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[PrimaryKey("Id", "BankCode")]
[Table("banks", Schema = "cts-bank-master")]
[Index("BankCode", Name = "banks_bank_code_key", IsUnique = true)]
public partial class Bank
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("bank_code")]
    public short BankCode { get; set; }

    [Column("bank_name")]
    [StringLength(100)]
    public string BankName { get; set; } = null!;

    [Column("is_active")]
    public bool IsActive { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
