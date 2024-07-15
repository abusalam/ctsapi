using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_details", Schema = "billing")]
[Index("BillId", Name = "fki_cheque_details_bill_id_fkey")]
public partial class ChequeDetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("payee_name", TypeName = "character varying")]
    public string? PayeeName { get; set; }

    [Column("cheque_number")]
    public int? ChequeNumber { get; set; }

    [Column("amount")]
    public double? Amount { get; set; }

    [Column("cheque_date")]
    public DateOnly? ChequeDate { get; set; }

    [Column("pay_mode")]
    public short? PayMode { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by_userid")]
    public long? CreatedByUserid { get; set; }

    [Column("updated_by_userid")]
    public long? UpdatedByUserid { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_active")]
    public short? IsActive { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("ChequeDetails")]
    public virtual BillDetail Bill { get; set; } = null!;
}
