﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("ppo_bill_breakups", Schema = "cts_pension")]
[Index("TreasuryCode", "PpoId", "PpoBillId", "RevisionId", "FromDate", Name = "ppo_bill_breakups_treasury_code_ppo_id_ppo_bill_id_revision_key", IsUnique = true)]
public partial class PpoBillBreakup
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year")]
    public int FinancialYear { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("ppo_id")]
    public int PpoId { get; set; }

    /// <summary>
    /// BillId is to identify the bill on which the actual payment made
    /// </summary>
    [Column("ppo_bill_id")]
    public long PpoBillId { get; set; }

    /// <summary>
    /// RevisionId is to identify the component rate applied on the bill
    /// </summary>
    [Column("revision_id")]
    public long RevisionId { get; set; }

    [Column("from_date")]
    public DateOnly FromDate { get; set; }

    [Column("to_date")]
    public DateOnly ToDate { get; set; }

    [Column("breakup_amount")]
    public int BreakupAmount { get; set; }

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

    [ForeignKey("PpoBillId")]
    [InverseProperty("PpoBillBreakups")]
    public virtual PpoBill PpoBill { get; set; } = null!;

    [ForeignKey("RevisionId")]
    [InverseProperty("PpoBillBreakups")]
    public virtual PpoComponentRevision Revision { get; set; } = null!;
}
