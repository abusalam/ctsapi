using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("lfpl_schemes_wallet", Schema = "cts")]
public partial class LfplSchemesWallet
{
    [Column("id")]
    public long? Id { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("operator_code")]
    public short OperatorCode { get; set; }

    [Column("scheme_code")]
    public int SchemeCode { get; set; }

    [Column("tr_prov_amt")]
    public double? TrProvAmt { get; set; }

    [Column("tr_actual_amt")]
    public double? TrActualAmt { get; set; }

    [Column("tr_passed_amt")]
    public double? TrPassedAmt { get; set; }
}
