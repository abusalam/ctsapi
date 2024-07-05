using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("reference_status_master", Schema = "lf_pl_master")]
public partial class ReferenceStatusMaster
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("decs", TypeName = "character varying")]
    public string? Decs { get; set; }

    [InverseProperty("StatusNavigation")]
    public virtual ICollection<Reference> References { get; set; } = new List<Reference>();
}
