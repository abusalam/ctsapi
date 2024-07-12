using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_TYPE", Schema = "cts_pension")]
[Index("PenTypeAbbr", "PensionTypeFlag", Name = "UK_P_MD_PEN_PREP_TYPE1", IsUnique = true)]
public partial class PMdPenPrepType
{
    /// <summary>
    /// Reference column from P_MM_PEN_PREP_CATEGORY..
    /// </summary>
    [Column("CATEGORY_ID")]
    public long CategoryId { get; set; }

    /// <summary>
    /// Unique pension ID type.
    /// </summary>
    [Key]
    [Column("PEN_TYPE_ID")]
    public int PenTypeId { get; set; }

    [Column("TYPE_DESC")]
    [StringLength(400)]
    public string TypeDesc { get; set; } = null!;

    /// <summary>
    /// &apos;F&apos; = Family
    /// </summary>
    [Column("TYPE_ABBR")]
    [MaxLength(1)]
    public char TypeAbbr { get; set; }

    [Column("ACTIVE_FLAG")]
    [MaxLength(1)]
    public char ActiveFlag { get; set; }

    [Column("USER_ID")]
    public int UserId { get; set; }

    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }

    /// <summary>
    /// added kalyan
    /// </summary>
    [Column("PEN_TYPE_ABBR")]
    [StringLength(3)]
    public string? PenTypeAbbr { get; set; }

    /// <summary>
    /// &apos;P&apos; for Provisional &amp; &apos;F&apos; for Final Pension
    /// </summary>
    [Column("PENSION_TYPE_FLAG")]
    [StringLength(1)]
    public string? PensionTypeFlag { get; set; }

    /// <summary>
    /// Types are commomn Superannuation &amp; &apos;Death&apos; &apos;SP&apos; &amp; &apos;DP&apos;
    /// </summary>
    [Column("PENSION_CALC_TYPE")]
    [StringLength(2)]
    public string? PensionCalcType { get; set; }
}
