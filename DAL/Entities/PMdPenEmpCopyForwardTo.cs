using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_EMP_COPY_FORWARD_TO", Schema = "cts_pension")]
public partial class PMdPenEmpCopyForwardTo
{
    [Key]
    [Column("INT_COPY_FORWARD_ID")]
    public int IntCopyForwardId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int? IntPensionerId { get; set; }

    [Column("SEQ_NO")]
    public int? SeqNo { get; set; }

    [Column("COPY_FORWARD_TO_TEXT")]
    [StringLength(500)]
    public string? CopyForwardToText { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string? ActiveFlag { get; set; }

    [Column("DML_STATUS_FLAG")]
    public int? DmlStatusFlag { get; set; }
}
