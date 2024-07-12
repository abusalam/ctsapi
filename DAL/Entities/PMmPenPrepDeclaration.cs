using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_PREP_DECLARATION", Schema = "cts_pension")]
public partial class PMmPenPrepDeclaration
{
    [Key]
    [Column("INT_PREP_DECLARATION_ID")]
    public long IntPrepDeclarationId { get; set; }

    /// <summary>
    /// P for pension, C for CVP application etc
    /// </summary>
    [Column("RETIREMENT_TYPE")]
    [StringLength(2)]
    public string RetirementType { get; set; } = null!;

    [Column("DECLARATIOIN_TEXT")]
    [StringLength(100)]
    public string DeclaratioinText { get; set; } = null!;

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string ActiveFlag { get; set; } = null!;

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("CREATED_BY_USER")]
    public int CreatedByUser { get; set; }

    [Column("MODIFIED_BY_USER")]
    public int? ModifiedByUser { get; set; }

    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }

    /// <summary>
    /// Unique pension ID type.
    /// </summary>
    [Column("PEN_TYPE_ID")]
    public int PenTypeId { get; set; }
}
