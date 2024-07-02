using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("reference_details", Schema = "lf_pl")]
public partial class ReferenceDetail
{
    [Column("ref_id")]
    public long RefId { get; set; }

    [Column("ref_no")]
    public long? RefNo { get; set; }

    [Column("ref_date")]
    public DateOnly? RefDate { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("op_code")]
    public short? OpCode { get; set; }

    [Column("scheme_code")]
    public int? SchemeCode { get; set; }

    [Column("gross_amt")]
    public double? GrossAmt { get; set; }

    [Column("net_amt")]
    public double? NetAmt { get; set; }

    [Column("no_of_beneficiaries")]
    public short? NoOfBeneficiaries { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("advice_id")]
    public long? AdviceId { get; set; }

    [Column("advice_date")]
    public DateOnly? AdviceDate { get; set; }

    [Column("scheme_desc", TypeName = "character varying")]
    public string? SchemeDesc { get; set; }

    [Column("bt_count")]
    public short? BtCount { get; set; }

    [Column("total_bt_count")]
    public short? TotalBtCount { get; set; }

    [Column("ref_type")]
    public short? RefType { get; set; }

    [Column("no_of_payee_op")]
    public short? NoOfPayeeOp { get; set; }
}
