using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("categories", Schema = "cts_pension")]
[Index("PrimaryCategoryId", "SubCategoryId", Name = "categories_primary_category_id_sub_category_id_key", IsUnique = true)]
public partial class Category
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("primary_category_id")]
    public long PrimaryCategoryId { get; set; }

    [Column("sub_category_id")]
    public long SubCategoryId { get; set; }

    /// <summary>
    /// primary_category_name - sub_category_name
    /// </summary>
    [Column("category_name")]
    [StringLength(100)]
    public string CategoryName { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [Column("active_flag")]
    public bool ActiveFlag { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<ComponentRate> ComponentRates { get; set; } = new List<ComponentRate>();

    [InverseProperty("Category")]
    public virtual ICollection<Pensioner> Pensioners { get; set; } = new List<Pensioner>();

    [ForeignKey("PrimaryCategoryId")]
    [InverseProperty("Categories")]
    public virtual PrimaryCategory PrimaryCategory { get; set; } = null!;

    [ForeignKey("SubCategoryId")]
    [InverseProperty("Categories")]
    public virtual SubCategory SubCategory { get; set; } = null!;
}
