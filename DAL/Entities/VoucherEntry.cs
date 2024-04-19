using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("voucher_entry", Schema = "cts")]
[Index("TreasuryCode", "Month", "FinancialYearId", "MajorHead", Name = "voucher_entry_treasury_code_month_financial_year_id_major_h_key", IsUnique = true)]
public partial class VoucherEntry
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("last_voucher_no")]
    public int LastVoucherNo { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("month")]
    [Precision(2, 0)]
    public decimal Month { get; set; }

    [Column("financial_year_id")]
    public short FinancialYearId { get; set; }

    [Column("major_head")]
    [StringLength(4)]
    public string MajorHead { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
