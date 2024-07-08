using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("lfpl_ecs", Schema = "lf_pl")]
public partial class LfplEc
{
    [Column("id")]
    public long Id { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("op_code")]
    public short? OpCode { get; set; }

    [Column("scheme_code")]
    public int? SchemeCode { get; set; }

    [Column("reference_type", TypeName = "character varying")]
    public string? ReferenceType { get; set; }

    [Column("ref_no")]
    public long? RefNo { get; set; }

    [Column("ref_date")]
    public DateOnly? RefDate { get; set; }

    [Column("advice_id")]
    public long? AdviceId { get; set; }

    [Column("advice_date")]
    public DateOnly? AdviceDate { get; set; }

    [Column("beneficiary_id")]
    public long? BeneficiaryId { get; set; }

    [Column("beneficiary_name")]
    [StringLength(50)]
    public string? BeneficiaryName { get; set; }

    [Column("beneficiary_ifsc")]
    [StringLength(14)]
    public string? BeneficiaryIfsc { get; set; }

    [Column("beneficiary_accno")]
    [StringLength(14)]
    public string? BeneficiaryAccno { get; set; }

    [Column("gross_amt")]
    [Precision(10, 2)]
    public decimal? GrossAmt { get; set; }

    [Column("net_amt")]
    [Precision(10, 2)]
    public decimal? NetAmt { get; set; }

    [Column("type_of_withdrawal")]
    public short? TypeOfWithdrawal { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public long? UpdatedBy { get; set; }

    [Column("bt_count")]
    public short? BtCount { get; set; }

    [Column("scheme_desc", TypeName = "character varying")]
    public string? SchemeDesc { get; set; }

    [ForeignKey("BeneficiaryId")]
    public virtual BeneficiariesMaster? Beneficiary { get; set; }

    public virtual Reference? RefNoNavigation { get; set; }
}
