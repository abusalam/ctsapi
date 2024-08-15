using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("ppo_bill_bytransfers", Schema = "cts_pension")]
public partial class PpoBillBytransfer
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

    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("bytransfer_hoa_id")]
    public int BytransferHoaId { get; set; }

    [Column("bytransfer_wef")]
    public DateOnly BytransferWef { get; set; }

    [Column("bytransfer_amount")]
    public int BytransferAmount { get; set; }

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

    [ForeignKey("BillId")]
    [InverseProperty("PpoBillBytransfers")]
    public virtual PpoBill Bill { get; set; } = null!;
}
