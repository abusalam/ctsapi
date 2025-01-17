using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

[Table("ppo_bytransfers", Schema = "cts_pension")]
public partial class PpoBytransfer
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year")]
    public int FinancialYear { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("pensioner_id")]
    public long PensionerId { get; set; }

    [Column("ppo_id")]
    public int PpoId { get; set; }

    [Column("from_date")]
    public DateOnly FromDate { get; set; }

    [Column("to_date")]
    public DateOnly ToDate { get; set; }

    [Column("bytransfer_head_id")]
    public long BytransferHeadId { get; set; }

    [Column("bytransfer_amount")]
    public int BytransferAmount { get; set; }

    [Column("remarks")]
    [StringLength(500)]
    public string? Remarks { get; set; }

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

    [ForeignKey("BytransferHeadId")]
    [InverseProperty("PpoBytransfers")]
    public virtual BytransferHead BytransferHead { get; set; } = null!;

    [ForeignKey("PensionerId")]
    [InverseProperty("PpoBytransfers")]
    public virtual Pensioner Pensioner { get; set; } = null!;
}
