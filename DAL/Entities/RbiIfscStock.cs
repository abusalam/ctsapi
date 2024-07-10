using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("RBI_IFSC_STOCK", Schema = "epradan_master")]
public partial class RbiIfscStock
{
    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Column("BankID")]
    public int? BankId { get; set; }

    [Column(TypeName = "character varying")]
    public string? BankName { get; set; }

    [Key]
    [Column("IFSC", TypeName = "character varying")]
    public string Ifsc { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string? Office { get; set; }

    [Column(TypeName = "character varying")]
    public string? Address { get; set; }

    [Column(TypeName = "character varying")]
    public string? District { get; set; }

    [Column(TypeName = "character varying")]
    public string? City { get; set; }

    [Column(TypeName = "character varying")]
    public string? State { get; set; }

    [Column(TypeName = "character varying")]
    public string? Phone { get; set; }

    [InverseProperty("IfscCodeNavigation")]
    public virtual ICollection<BeneficiaryMaster> BeneficiaryMasters { get; set; } = new List<BeneficiaryMaster>();
}
