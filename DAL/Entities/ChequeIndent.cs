using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_indent", Schema = "cts")]
[Index("Status", Name = "fki_cheque_indent_status_fkey")]
public partial class ChequeIndent
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("indent_id")]
    public int? IndentId { get; set; }

    [Column("indent_date")]
    public DateOnly? IndentDate { get; set; }

    [Column("memo_no")]
    [StringLength(18)]
    public string? MemoNo { get; set; }

    [Column("memo_date")]
    public DateOnly? MemoDate { get; set; }

    [Column("remarks")]
    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 1 = new indent , 2 =  approve by TO , 3= Reject by TO
    /// </summary>
    [Column("status")]
    public int? Status { get; set; }

    [Column("approved_rejected_by")]
    public long? ApprovedRejectedBy { get; set; }

    [Column("approved_rejected_at", TypeName = "timestamp without time zone")]
    public DateTime? ApprovedRejectedAt { get; set; }

    [Column("total_approved_quantity")]
    public int? TotalApprovedQuantity { get; set; }

    [Column("total_requested_quantity")]
    public int? TotalRequestedQuantity { get; set; }

    [InverseProperty("ChequeIndent")]
    public virtual ICollection<ChequeIndentDetail> ChequeIndentDetails { get; set; } = new List<ChequeIndentDetail>();

    [ForeignKey("Status")]
    [InverseProperty("ChequeIndents")]
    public virtual Status? StatusNavigation { get; set; }
}
