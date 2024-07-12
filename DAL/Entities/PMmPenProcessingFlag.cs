using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_PROCESSING_FLAG", Schema = "cts_pension")]
public partial class PMmPenProcessingFlag
{
    [Key]
    [Column("PROCESSING_FLAG")]
    public long ProcessingFlag { get; set; }

    [Column("DESCRIPTION")]
    [StringLength(100)]
    public string Description { get; set; } = null!;

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string? ActiveFlag { get; set; }

    [Column("USER_ID")]
    public int UserId { get; set; }

    [Column("CREATED_TIMESTAMP")]
    public DateOnly CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP")]
    public DateOnly ModifiedTimestamp { get; set; }
}
