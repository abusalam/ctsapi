using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("token", Schema = "cts")]
[Index("BillId", Name = "fki_bill_id_fkey")]
[Index("BillId", Name = "fki_token_bill_details_fkey")]
[Index("TokenFlowId", Name = "fki_token_token_flow_id_fkey")]
[Index("BillId", Name = "fki_tp_bill_bill_id_fky")]
[Index("ReferenceNo", "ReferenceVersion", Name = "token_reference_no_reference_version_key", IsUnique = true)]
public partial class Token
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("token_number")]
    public long TokenNumber { get; set; }

    [Column("token_date")]
    public DateOnly TokenDate { get; set; }

    [Column("financial_year", TypeName = "character varying")]
    public string FinancialYear { get; set; } = null!;

    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("reference_no")]
    [StringLength(13)]
    public string ReferenceNo { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public long CreatedBy { get; set; }

    [Column("ddo_code")]
    [StringLength(9)]
    public string DdoCode { get; set; } = null!;

    [Column("token_flow_id")]
    public long? TokenFlowId { get; set; }

    [Column("remarks", TypeName = "character varying")]
    public string? Remarks { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("reference_version")]
    public short? ReferenceVersion { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("Tokens")]
    public virtual BillDetail Bill { get; set; } = null!;

    [ForeignKey("TokenFlowId")]
    [InverseProperty("Tokens")]
    public virtual TokenFlow? TokenFlow { get; set; }

    [InverseProperty("Toke")]
    public virtual ICollection<TokenFlow> TokenFlows { get; set; } = new List<TokenFlow>();

    [InverseProperty("Token")]
    public virtual ICollection<TokenHasObjection> TokenHasObjections { get; set; } = new List<TokenHasObjection>();
}
