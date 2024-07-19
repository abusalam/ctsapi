using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("bank_master", Schema = "epradan_master")]
public partial class BankMaster
{
    [Key]
    [Column("bank_id")]
    public int BankId { get; set; }

    [Column("bank_name", TypeName = "character varying")]
    public string BankName { get; set; } = null!;

    [InverseProperty("BankCodeNavigation")]
    public virtual ICollection<BeneficiaryMaster> BeneficiaryMasters { get; set; } = new List<BeneficiaryMaster>();
}
