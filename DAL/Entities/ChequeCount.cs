using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_count", Schema = "cts")]
[Index("FinancialYearId", "TreasuryCode", "MicrCode", Name = "cheque_count_financial_year_id_treasury_code_micr_code_key", IsUnique = true)]
public partial class ChequeCount
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("financial_year_id")]
    public short FinancialYearId { get; set; }

    [Column("total_count")]
    public int TotalCount { get; set; }

    [Column("utilized")]
    public int? Utilized { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("micr_code")]
    [StringLength(9)]
    public string? MicrCode { get; set; }
}
