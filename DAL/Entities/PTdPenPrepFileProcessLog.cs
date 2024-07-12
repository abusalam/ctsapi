using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_TD_PEN_PREP_FILE_PROCESS_LOG", Schema = "cts_pension")]
[Index("PenFileProcessLogId", "PenFileProcessSeq", Name = "UK_P_TD_PEN_PREP_FILE_PRO_LOG1", IsUnique = true)]
public partial class PTdPenPrepFileProcessLog
{
    [Key]
    [Column("PEN_FILE_PROCESS_LOG_ID")]
    public long PenFileProcessLogId { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int? IntPensionerId { get; set; }

    [Column("PEN_FILE_PROCESS_SEQ")]
    public int PenFileProcessSeq { get; set; }

    [Column("PROCESSING_FLAG")]
    public int ProcessingFlag { get; set; }

    [Column("PROCESS_BY")]
    public int ProcessBy { get; set; }

    [Column("PROCESS_DATE", TypeName = "timestamp without time zone")]
    public DateTime? ProcessDate { get; set; }

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("PROCESSED_BY_USER_TYPE")]
    [StringLength(2)]
    public string? ProcessedByUserType { get; set; }

    [Column("SUB_SYSTEM_ID")]
    public int SubSystemId { get; set; }

    [Column("PROCESSED_BY_USER_ROLE_ID")]
    public int? ProcessedByUserRoleId { get; set; }

    [Column("REMARKS")]
    [StringLength(300)]
    public string? Remarks { get; set; }

    [Column("SEND_TO_USER_ID")]
    public int? SendToUserId { get; set; }

    [Column("SEND_TO_USER_TYPE")]
    [StringLength(2)]
    public string? SendToUserType { get; set; }

    [Column("SEND_TO_USER_ROLE_ID")]
    public int? SendToUserRoleId { get; set; }

    [Column("PROCESSED_BY_AUTH_ID")]
    public int? ProcessedByAuthId { get; set; }

    [Column("SEND_TO_AUTH_ID")]
    public int? SendToAuthId { get; set; }
}
