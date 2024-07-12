using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_EMP_WISE_FORWARD_LIST", Schema = "cts_pension")]
public partial class PMdPenEmpWiseForwardList
{
    [Key]
    [Column("INT_PEN_FORWARDING_ID")]
    public long IntPenForwardingId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int? IntPensionerId { get; set; }

    [Column("SL_NO")]
    public int? SlNo { get; set; }

    [Column("FORWARDING_TEXT")]
    [StringLength(500)]
    public string? ForwardingText { get; set; }

    /// <summary>
    /// &apos;Y&apos; / &apos;N&apos; Flag
    /// </summary>
    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string? ActiveFlag { get; set; }

    [Column("DML_STATUS_FLAG")]
    public int? DmlStatusFlag { get; set; }
}
