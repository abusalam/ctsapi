using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_label_master", Schema = "cts_master")]
public partial class StampLabelMaster
{
    [Key]
    [Column("label_id")]
    public long LabelId { get; set; }

    [Column("no_label_per_sheet")]
    public short NoLabelPerSheet { get; set; }

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

    [InverseProperty("StampLabel")]
    public virtual ICollection<StampCombination> StampCombinations { get; set; } = new List<StampCombination>();
}
