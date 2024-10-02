using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("bills", Schema = "cts_pension")]
public partial class Bill
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year")]
    public int FinancialYear { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("hoa_id")]
    [StringLength(50)]
    public string HoaId { get; set; } = null!;

    [Column("branch_id")]
    public long BranchId { get; set; }

    [Column("bill_no")]
    public int BillNo { get; set; }

    [Column("bill_date")]
    public DateOnly BillDate { get; set; }

    [Column("treasury_voucher_no")]
    [StringLength(100)]
    public string? TreasuryVoucherNo { get; set; }

    [Column("treasury_voucher_date")]
    public DateOnly? TreasuryVoucherDate { get; set; }

    [Column("from_date")]
    public DateOnly FromDate { get; set; }

    [Column("to_date")]
    public DateOnly ToDate { get; set; }

    [Column("gross_amount")]
    public int GrossAmount { get; set; }

    [Column("bytransfer_amount")]
    public int BytransferAmount { get; set; }

    [Column("net_amount")]
    public int NetAmount { get; set; }

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

    [ForeignKey("BranchId")]
    [InverseProperty("Bills")]
    public virtual Branch Branch { get; set; } = null!;

    [InverseProperty("Bill")]
    public virtual ICollection<PpoBill> PpoBills { get; set; } = new List<PpoBill>();
}
