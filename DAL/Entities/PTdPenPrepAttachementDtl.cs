using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_TD_PEN_PREP_ATTACHEMENT_DTL", Schema = "cts_pension")]
[Index("DocumentId", Name = "UK_P_TD_PEN_PREP_ATTACH_DTL1", IsUnique = true)]
[Index("ContentId", Name = "UK_P_TD_PEN_PREP_ATTACH_DTL2", IsUnique = true)]
public partial class PTdPenPrepAttachementDtl
{
    [Key]
    [Column("INT_ATTACHEMENT_DTL_ID")]
    public long IntAttachementDtlId { get; set; }

    [Column("DOCUMENT_ID")]
    public int DocumentId { get; set; }

    [Column("CONTENT_ID")]
    [StringLength(38)]
    public string ContentId { get; set; } = null!;

    [Column("INT_EMPLOYEE_ID")]
    public int IntEmployeeId { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string ActiveFlag { get; set; } = null!;

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("USER_ID")]
    public int UserId { get; set; }

    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int? ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }

    [Column("ROLE_ID")]
    public int RoleId { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int? ModifiedRoleId { get; set; }

    [Column("REQUEST_ID")]
    public int? RequestId { get; set; }

    [Column("INT_PEN_PREP_ATTACHEMENT")]
    public int? IntPenPrepAttachement { get; set; }

    [Column("REQUEST_TYPE")]
    [StringLength(15)]
    public string? RequestType { get; set; }

    [Column("FILE_NAME")]
    [StringLength(100)]
    public string? FileName { get; set; }

    [Column("FILE_TYPE")]
    [StringLength(20)]
    public string? FileType { get; set; }
}
