using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("reference_type", Schema = "lf_pl")]
public partial class ReferenceType
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("description", TypeName = "character varying")]
    public string Description { get; set; } = null!;

    [Column("single_multiple", TypeName = "character varying")]
    public string? SingleMultiple { get; set; }

    [InverseProperty("RefTypeNavigation")]
    public virtual ICollection<Reference> References { get; set; } = new List<Reference>();
}
