using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_RETIREMENT_BENF_TYPE", Schema = "cts_pension")]
public partial class PMmPenRetirementBenfType
{
    [Key]
    [Column("BENF_TYPE_ID")]
    public long BenfTypeId { get; set; }

    [Column("BENF_DESC")]
    [StringLength(300)]
    public string BenfDesc { get; set; } = null!;

    [Column("BENF_ABBR")]
    [StringLength(4)]
    public string BenfAbbr { get; set; } = null!;

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
    /// For selecting nominee, value should be &apos;Y&apos;-- added by ritu
    /// </summary>
    [Column("NOMINEE_FLAG")]
    [StringLength(1)]
    public string? NomineeFlag { get; set; }
}
