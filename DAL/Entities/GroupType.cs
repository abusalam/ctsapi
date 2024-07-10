using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("group_type", Schema = "epradan_master")]
public partial class GroupType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("group_desc", TypeName = "character varying")]
    public string GroupDesc { get; set; } = null!;

    [InverseProperty("Group")]
    public virtual ICollection<BeneficiaryMaster> BeneficiaryMasters { get; set; } = new List<BeneficiaryMaster>();
}
