using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_PREP_ATTACHEMENT", Schema = "cts_pension")]
[Index("AttchementDesc", "RequestType", Name = "UK_P_MM_PEN_PREP_ATTACH1", IsUnique = true)]
[Index("RequestType", "IntPenPrepAttachement", Name = "UK_P_MM_PEN_PREP_ATTACH2", IsUnique = true)]
public partial class PMmPenPrepAttachement
{
    [Key]
    [Column("INT_PEN_PREP_ATTACHEMENT")]
    public long IntPenPrepAttachement { get; set; }

    [Column("ATTCHEMENT_DESC")]
    [StringLength(300)]
    public string AttchementDesc { get; set; } = null!;

    [Column("OPTIONAL_MANDATORY_FLAG")]
    [StringLength(3)]
    public string OptionalMandatoryFlag { get; set; } = null!;

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
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }

    [Column("ROLE_ID")]
    public int? RoleId { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int? ModifiedRoleId { get; set; }

    [Column("REQUEST_TYPE")]
    [StringLength(15)]
    public string RequestType { get; set; } = null!;
}
