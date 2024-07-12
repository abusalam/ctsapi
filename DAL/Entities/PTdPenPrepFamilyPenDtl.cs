using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_TD_PEN_PREP_FAMILY_PEN_DTL", Schema = "cts_pension")]
public partial class PTdPenPrepFamilyPenDtl
{
    [Key]
    [Column("INT_PREP_FAMILY_PEN_DTL")]
    public long IntPrepFamilyPenDtl { get; set; }

    [Column("INT_PENSIONER_ID")]
    public int IntPensionerId { get; set; }

    [Column("FAMILY_ID")]
    public int FamilyId { get; set; }

    [Column("REMARKS")]
    [StringLength(300)]
    public string? Remarks { get; set; }

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

    [Column("REQUEST_ID")]
    public int RequestId { get; set; }

    [Column("CREATED_ROLE_ID")]
    public int? CreatedRoleId { get; set; }

    [Column("MODIFIED_ROLE_ID")]
    public int? ModifiedRoleId { get; set; }

    [Column("ACTIVE_FLAG")]
    [StringLength(1)]
    public string? ActiveFlag { get; set; }

    [Column("INT_NOM_ID")]
    public int? IntNomId { get; set; }
}
