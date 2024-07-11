using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MD_PEN_PREP_ATTACHMENT", Schema = "cts_pension")]
[Index("DocumentId", Name = "UK_P_MD_PEN_PREP_ATTACHEMENT1", IsUnique = true)]
[Index("ContentId", Name = "UK_P_MD_PEN_PREP_ATTACHEMENT2", IsUnique = true)]
public partial class PMdPenPrepAttachment
{
    [Key]
    [Column("INT_PEN_PREP_ATTACHEMENT")]
    public int IntPenPrepAttachement { get; set; }

    /// <summary>
    /// For Service book : &apos;S&apos; for Exit Management &apos;E&apos;
    /// </summary>
    [Column("SERVICE_BOOK_EXIT_MNGMNT_FLAG")]
    [StringLength(1)]
    public string ServiceBookExitMngmntFlag { get; set; } = null!;

    [Column("DOCUMENT_ID")]
    public int DocumentId { get; set; }

    [Column("CONTENT_ID")]
    [StringLength(38)]
    public string ContentId { get; set; } = null!;

    [Column("INT_EMPLOYEE_ID")]
    public int IntEmployeeId { get; set; }

    [Column("REQUEST_ID")]
    public int? RequestId { get; set; }

    [Column("REQUEST_TYPE")]
    [StringLength(15)]
    public string? RequestType { get; set; }

    [Column("FILE_NAME")]
    [StringLength(100)]
    public string? FileName { get; set; }

    [Column("FILE_TYPE")]
    [StringLength(20)]
    public string? FileType { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string ActiveFlag { get; set; } = null!;

    [Column("DML_STATUS_FLAG")]
    public int DmlStatusFlag { get; set; }

    [Column("CREATED_USER_ID")]
    public int CreatedUserId { get; set; }

    [Column("CREATED_ROLE_ID")]
    public int CreatedRoleId { get; set; }

    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int? ModifiedUserId { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int? ModifiedRoleId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }

    /// <summary>
    /// document type from other master master type: &apos;AFT&apos;
    /// </summary>
    [Column("INT_OMI_UPD_DOC_TYPE")]
    public int? IntOmiUpdDocType { get; set; }
}
