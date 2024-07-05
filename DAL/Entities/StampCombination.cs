using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_combination", Schema = "cts_master")]
public partial class StampCombination
{
    [Key]
    [Column("stamp_combination_id")]
    public long StampCombinationId { get; set; }

    [Column("stamp_label_id")]
    public long StampLabelId { get; set; }

    [Column("stamp_category_id")]
    public long StampCategoryId { get; set; }

    [Column("stamp_denomination_id")]
    public long StampDenominationId { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("stamp_type_id")]
    public long StampTypeId { get; set; }

    [ForeignKey("StampCategoryId")]
    [InverseProperty("StampCombinations")]
    public virtual StampCategory StampCategory { get; set; } = null!;

    [InverseProperty("StampCombination")]
    public virtual ICollection<StampIndent> StampIndents { get; set; } = new List<StampIndent>();

    [ForeignKey("StampLabelId")]
    [InverseProperty("StampCombinations")]
    public virtual StampLabelMaster StampLabel { get; set; } = null!;

    [ForeignKey("StampTypeId")]
    [InverseProperty("StampCombinations")]
    public virtual StampType StampType { get; set; } = null!;

    [InverseProperty("Combination")]
    public virtual ICollection<StampWallet> StampWallets { get; set; } = new List<StampWallet>();
}
