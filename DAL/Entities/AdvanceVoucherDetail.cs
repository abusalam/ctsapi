using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("advance_voucher_details", Schema = "cts")]
public partial class AdvanceVoucherDetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("treasury_code", TypeName = "character varying")]
    public string? TreasuryCode { get; set; }

    [Column("ddo_code", TypeName = "character varying")]
    public string? DdoCode { get; set; }

    [Column("voucher_no", TypeName = "character varying")]
    public string? VoucherNo { get; set; }

    [Column("voucher_date")]
    public DateOnly? VoucherDate { get; set; }

    [Column("token_no", TypeName = "character varying")]
    public string? TokenNo { get; set; }

    [Column("tr_form_id")]
    public int TrFormId { get; set; }

    [Column("amount")]
    public double? Amount { get; set; }

    [Column("demand")]
    [StringLength(2)]
    public string? Demand { get; set; }

    [Column("major_head")]
    [StringLength(4)]
    public string? MajorHead { get; set; }

    [Column("sub_major_head")]
    [StringLength(2)]
    public string? SubMajorHead { get; set; }

    [Column("minor_head")]
    [StringLength(3)]
    public string? MinorHead { get; set; }

    [Column("plan_status")]
    [StringLength(2)]
    public string? PlanStatus { get; set; }

    [Column("scheme_head")]
    [StringLength(3)]
    public string? SchemeHead { get; set; }

    [Column("detail_head")]
    [StringLength(2)]
    public string? DetailHead { get; set; }

    [Column("sub_detail_head")]
    [StringLength(2)]
    public string? SubDetailHead { get; set; }

    [Column("voted_charged")]
    [StringLength(1)]
    public string? VotedCharged { get; set; }

    [Column("reference_no", TypeName = "character varying")]
    public string ReferenceNo { get; set; } = null!;

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_by")]
    public long? UpdatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("token_date", TypeName = "timestamp without time zone")]
    public DateTime? TokenDate { get; set; }

    [Column("adjustment_reference_no", TypeName = "character varying")]
    public string? AdjustmentReferenceNo { get; set; }

    [Column("is_active")]
    public short? IsActive { get; set; }
}
