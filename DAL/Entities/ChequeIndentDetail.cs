using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_indent_details", Schema = "cts")]
public partial class ChequeIndentDetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("cheque_indent_id")]
    public long? ChequeIndentId { get; set; }

    /// <summary>
    /// 1= treasury 2= others
    /// </summary>
    [Column("cheque_type")]
    public short? ChequeType { get; set; }

    [Column("micr_code")]
    [StringLength(6)]
    public string? MicrCode { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }
}
