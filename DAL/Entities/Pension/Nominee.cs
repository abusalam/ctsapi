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

    /// <summary>
    /// F - Father; M - Mother; H - Husband; W - Wife; S - Son; D - Daughter; B - Brother; T - Sister; E - Self; I - Brother(Minor); A - Sister(Unmarried); C - Sister(Widowed); O - Other;
    /// </summary>
    [Column("relation")]
    [MaxLength(1)]
    public char Relation { get; set; }

    [Column("date_of_birth")]
    public DateOnly DateOfBirth { get; set; }

    [Column("date_of_death")]
    public DateOnly? DateOfDeath { get; set; }

    /// <summary>
    /// 1 - Family; 5 - LTA; 6 - Death Gratuity;
    /// </summary>
    [Column("nominee_type")]
    [MaxLength(1)]
    public char? NomineeType { get; set; }

    /// <summary>
    /// A - Adult; M - Minor;
    /// </summary>
    [Column("nominee_adult_minor")]
    [MaxLength(1)]
    public char? NomineeAdultMinor { get; set; }

    /// <summary>
    /// 1 - First; 2 - Second; 3 - Third; 4 - Fourth; 5 - Fifth;
    /// </summary>
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
