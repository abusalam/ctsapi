using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_PREP_PAY_RECO_TYPE", Schema = "cts_pension")]
public partial class PMmPenPrepPayRecoType
{
    /// <summary>
    /// Recovery type id..
    /// </summary>
    [Key]
    [Column("PAY_RECO_TYPE_ID")]
    public long PayRecoTypeId { get; set; }

    /// <summary>
    /// Recovery type description..
    /// </summary>
    [Column("PAY_RECO_DESC")]
    [StringLength(200)]
    public string PayRecoDesc { get; set; } = null!;

    /// <summary>
    /// Single word abbreviation for recovery type description..
    /// </summary>
    [Column("PAY_RECO_ABBR")]
    [StringLength(2)]
    public string PayRecoAbbr { get; set; } = null!;

    [Column("COMPONENT_TYPE")]
    [MaxLength(1)]
    public char ComponentType { get; set; }

    /// <summary>
    /// R for recovery , P --- payment
    /// </summary>
    [Column("PAY_RECO_FLAG")]
    [MaxLength(1)]
    public char PayRecoFlag { get; set; }

    [Column("RECOVERY_FROM")]
    [MaxLength(1)]
    public char RecoveryFrom { get; set; }

    [Column("TRES_BREAKUP_CLASS_ID")]
    public int TresBreakupClassId { get; set; }

    [Column("TRES_BREAKUP_CLASS_TYPE")]
    [MaxLength(1)]
    public char TresBreakupClassType { get; set; }

    [Column("PAYMENT_ORDER_FLAG")]
    [MaxLength(1)]
    public char PaymentOrderFlag { get; set; }

    [Column("HOA_ID")]
    [StringLength(6)]
    public string? HoaId { get; set; }

    [Column("ACTIVE_FLAG")]
    [MaxLength(1)]
    public char ActiveFlag { get; set; }

    [Column("USER_ID")]
    public int UserId { get; set; }

    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }
}
