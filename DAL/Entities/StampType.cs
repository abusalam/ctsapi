using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_type", Schema = "cts_master")]
public partial class StampType
{
    [Key]
    [Column("denomination_id")]
    public long DenominationId { get; set; }

    [Column("denomination")]
    [Precision(10, 2)]
    public decimal Denomination { get; set; }

    [Required]
    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public long? UpdatedBy { get; set; }

    [InverseProperty("StampDenomination")]
    public virtual ICollection<StampCombination> StampCombinationStampDenominations { get; set; } = new List<StampCombination>();

    [InverseProperty("StampType")]
    public virtual ICollection<StampCombination> StampCombinationStampTypes { get; set; } = new List<StampCombination>();
}
