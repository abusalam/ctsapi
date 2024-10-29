using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("nominees", Schema = "cts_pension")]
public partial class Nominee
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year")]
    public int FinancialYear { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("family_pension")]
    public bool? FamilyPension { get; set; }

    [Column("refused")]
    public bool? Refused { get; set; }

    [Column("nominee_active")]
    public bool? NomineeActive { get; set; }

    [Column("serial_no")]
    public int SerialNo { get; set; }

    [Column("pensioner_id")]
    public long PensionerId { get; set; }

    [Column("ppo_id")]
    public int PpoId { get; set; }

    [Column("nominee_name")]
    [StringLength(100)]
    public string NomineeName { get; set; } = null!;

    [Column("relation")]
    [MaxLength(1)]
    public char Relation { get; set; }

    [Column("date_of_birth")]
    public DateOnly DateOfBirth { get; set; }

    [Column("date_of_death")]
    public DateOnly? DateOfDeath { get; set; }

    [Column("nominee_type")]
    [MaxLength(1)]
    public char? NomineeType { get; set; }

    [Column("nominee_priority")]
    public int? NomineePriority { get; set; }

    [Column("nominee_share")]
    public int? NomineeShare { get; set; }

    [Column("bank_ac_no")]
    [StringLength(30)]
    public string? BankAcNo { get; set; }

    [Column("branch_id")]
    public long? BranchId { get; set; }

    [Column("identification_mark")]
    [StringLength(100)]
    public string? IdentificationMark { get; set; }

    [Column("handicapped")]
    public bool? Handicapped { get; set; }

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

    [ForeignKey("BranchId")]
    [InverseProperty("Nominees")]
    public virtual Branch? Branch { get; set; }

    [ForeignKey("PensionerId")]
    [InverseProperty("Nominees")]
    public virtual Pensioner Pensioner { get; set; } = null!;
}
