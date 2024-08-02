using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("primary_categories", Schema = "cts_pension")]
[Index("PrimaryCategoryName", Name = "primary_categories_primary_category_name_key", IsUnique = true)]
public partial class PrimaryCategory
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    /// <summary>
    /// Head of Account: 2071 - 01 - 109 - 00 - 001 - V - 04 - 00
    /// </summary>
    [Column("hoa_id")]
    [StringLength(50)]
    public string HoaId { get; set; } = null!;

    [Column("primary_category_name")]
    [StringLength(100)]
    public string PrimaryCategoryName { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [Column("active_flag")]
    public bool ActiveFlag { get; set; }

    [InverseProperty("PrimaryCategory")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
