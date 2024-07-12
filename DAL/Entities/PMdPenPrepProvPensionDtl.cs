using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_PROV_PENSION_DTL", Schema = "cts_pension")]
public partial class PMdPenPrepProvPensionDtl
{
    [Key]
    [Column("INT_PEN_PROV_PENSION_ID")]
    public long IntPenProvPensionId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("PENSION_START_DATE")]
    public DateOnly PensionStartDate { get; set; }

    [Column("PENSION_END_DATE")]
    public DateOnly PensionEndDate { get; set; }

    [Column("MEMO_NO")]
    [StringLength(100)]
    public string? MemoNo { get; set; }

    [Column("MEMO_DATE")]
    public DateOnly? MemoDate { get; set; }

    [Column("UO_NO")]
    [StringLength(100)]
    public string? UoNo { get; set; }

    [Column("UO_DATE")]
    public DateOnly? UoDate { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string ActiveFlag { get; set; } = null!;

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("PENSION_HOA_MAP_ID")]
    public int? PensionHoaMapId { get; set; }

    [Column("GRATUITY_HOA_MAP_ID")]
    public int? GratuityHoaMapId { get; set; }

    [Column("PENSION_HOA_DESC")]
    [StringLength(200)]
    public string? PensionHoaDesc { get; set; }

    [Column("GRATUITY_HOA_DESC")]
    [StringLength(200)]
    public string? GratuityHoaDesc { get; set; }

    [Column("REMARKS")]
    [StringLength(1000)]
    public string? Remarks { get; set; }

    [Column("REQUEST_ID")]
    public int? RequestId { get; set; }

    [Column("CREATED_USER_ID")]
    public int CreatedUserId { get; set; }

    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }

    /// <summary>
    /// as per CR 181305
    /// </summary>
    [Column("PENSION_RATE_PER_MONTH_USER")]
    public int? PensionRatePerMonthUser { get; set; }

    /// <summary>
    /// as per CR 181305
    /// </summary>
    [Column("TOTAL_PENSION_PERIOD_USER")]
    [StringLength(100)]
    public string? TotalPensionPeriodUser { get; set; }
}
