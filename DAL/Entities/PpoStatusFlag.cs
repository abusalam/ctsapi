using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("ppo_status_flags", Schema = "cts_pension")]
[Index("PpoId", "TreasuryCode", Name = "ppo_status_flags_ppo_id_treasury_code_key", IsUnique = true)]
public partial class PpoStatusFlag
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

    [Column("ppo_approved_flag")]
    public bool? PpoApprovedFlag { get; set; }

    [Column("ppo_approved_wef")]
    public DateOnly? PpoApprovedWef { get; set; }

    [Column("health_scheme_flag")]
    public bool? HealthSchemeFlag { get; set; }

    [Column("health_scheme_wef")]
    public DateOnly? HealthSchemeWef { get; set; }

    [Column("first_pension_flag")]
    public bool? FirstPensionFlag { get; set; }

    [Column("first_pension_wef")]
    public DateOnly? FirstPensionWef { get; set; }

    [Column("double_pension_flag")]
    public bool? DoublePensionFlag { get; set; }

    [Column("double_pension_wef")]
    public DateOnly? DoublePensionWef { get; set; }

    [Column("employeed_flag")]
    public bool? EmployeedFlag { get; set; }

    [Column("employeed_wef")]
    public DateOnly? EmployeedWef { get; set; }

    [Column("reemployed_flag")]
    public bool? ReemployedFlag { get; set; }

    [Column("reemployed_wef")]
    public DateOnly? ReemployedWef { get; set; }

    [Column("adhoc_pension_flag")]
    public bool? AdhocPensionFlag { get; set; }

    [Column("adhoc_pension_wef")]
    public DateOnly? AdhocPensionWef { get; set; }

    [Column("interim_pension_flag")]
    public bool? InterimPensionFlag { get; set; }

    [Column("interim_pension_wef")]
    public DateOnly? InterimPensionWef { get; set; }

    [Column("shared_pension_flag")]
    public bool? SharedPensionFlag { get; set; }

    [Column("shared_pension_wef")]
    public DateOnly? SharedPensionWef { get; set; }

    [Column("status_active_flag")]
    public bool? StatusActiveFlag { get; set; }

    [Column("status_active_wef")]
    public DateOnly? StatusActiveWef { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [Column("active_flag")]
    public bool? ActiveFlag { get; set; }
}
