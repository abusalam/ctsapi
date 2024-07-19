using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("ddo_wallet_actual", Schema = "cts")]
[Index("SaoDdoCode", "ActiveHoaId", Name = "ddo_wallet_actual_sao_ddo_code_active_hoa_id_key", IsUnique = true)]
public partial class DdoWalletActual
{
    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("sao_ddo_code")]
    [StringLength(12)]
    public string SaoDdoCode { get; set; } = null!;

    [Column("actual_released_amount")]
    [Precision(10, 0)]
    public decimal? ActualReleasedAmount { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by_userid")]
    public long CreatedByUserid { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by_userid")]
    public long? UpdatedByUserid { get; set; }

    [Key]
    [Column("wallet_id")]
    public long WalletId { get; set; }

    [Column("financial_year")]
    public short FinancialYear { get; set; }

    [Column("active_hoa_id")]
    public long? ActiveHoaId { get; set; }

    [Column("in_process_amount")]
    [Precision(10, 2)]
    public decimal? InProcessAmount { get; set; }

    [ForeignKey("FinancialYear")]
    [InverseProperty("DdoWalletActuals")]
    public virtual FinancialYearMaster FinancialYearNavigation { get; set; } = null!;

    public virtual Ddo SaoDdoCodeNavigation { get; set; } = null!;

    public virtual Treasury TreasuryCodeNavigation { get; set; } = null!;

    [ForeignKey("WalletId")]
    [InverseProperty("DdoWalletActual")]
    public virtual DdoWallet Wallet { get; set; } = null!;
}
