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
    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("operator_code")]
    public short OperatorCode { get; set; }

    [Column("scheme_code")]
    public int SchemeCode { get; set; }

    [Column("tr_prov_bal")]
    public double? TrProvBal { get; set; }

    [Column("tr_actual_bal")]
    public double? TrActualBal { get; set; }

    [Column("tr_passed_amt")]
    public double? TrPassedAmt { get; set; }
}
