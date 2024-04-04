using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_indent", Schema = "cts")]
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
}
