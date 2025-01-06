using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("eppo_nominees", Schema = "cts_pension")]
public partial class EppoNominee
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    /// <summary>
    /// P - Pensioner; F - Family; D - Dependent;
    /// </summary>
    [Column("nominee_type")]
    [MaxLength(1)]
    public char NomineeType { get; set; }

    [Column("serial_no")]
    public int SerialNo { get; set; }

    [Column("nominee_name")]
    [StringLength(100)]
    public string NomineeName { get; set; } = null!;

    [Column("date_of_birth")]
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// [WEHSDOMRNAFKYCUITJBPVL] E - Employed; L - Widow Daughter; U - Unmarried Daughter; V - Divorced Daughter; N - Minor Son; R - Minor Daughter; P - Handicapped Son; G - Handicapped Daughter; J - Dependent Father; K - Dependent Mother; H - Husband; W - Wife;
    /// </summary>
    [Column("relation")]
    [MaxLength(1)]
    public char Relation { get; set; }

    [Column("nominee_share")]
    public int? NomineeShare { get; set; }

    /// <summary>
    /// A - Adult; M - Minor;
    /// </summary>
    [Column("nominee_adult_minor")]
    [MaxLength(1)]
    public char? NomineeAdultMinor { get; set; }

    [Column("eppo_receipt_id")]
    public long EppoReceiptId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [Column("active_flag")]
    public bool ActiveFlag { get; set; }

    [ForeignKey("EppoReceiptId")]
    [InverseProperty("EppoNominees")]
    public virtual EppoReceipt EppoReceipt { get; set; } = null!;
}
