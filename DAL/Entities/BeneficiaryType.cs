using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("beneficiary_type", Schema = "lf_pl_master")]
[Index("Desc", Name = "unique_ben_type", IsUnique = true)]
public partial class BeneficiaryType
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("desc", TypeName = "character varying")]
    public string Desc { get; set; } = null!;

    public virtual ICollection<BeneficiariesMaster> BeneficiariesMasters { get; set; } = new List<BeneficiariesMaster>();
}
