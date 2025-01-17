using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("bytransfer_heads", Schema = "cts_pension")]
public partial class BytransferHead
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    /// <summary>
    /// P - Payment; R - Recovery;
    /// </summary>
    [Column("bytransfer_type")]
    [MaxLength(1)]
    public char BytransferType { get; set; }

    [Column("account_head_id")]
    public long AccountHeadId { get; set; }

    [Column("bytransfer_description")]
    [StringLength(500)]
    public string BytransferDescription { get; set; } = null!;

    [Column("ag_bytransfer")]
    public bool AgBytransfer { get; set; }

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

    [ForeignKey("AccountHeadId")]
    [InverseProperty("BytransferHeads")]
    public virtual AccountHead AccountHead { get; set; } = null!;

    [InverseProperty("BytransferHead")]
    public virtual ICollection<BillBytransfer> BillBytransfers { get; set; } = new List<BillBytransfer>();

    [InverseProperty("BytransferHead")]
    public virtual ICollection<PpoBytransfer> PpoBytransfers { get; set; } = new List<PpoBytransfer>();
}
