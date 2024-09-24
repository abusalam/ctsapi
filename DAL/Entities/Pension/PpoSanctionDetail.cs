using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("ppo_sanction_details", Schema = "cts_pension")]
public partial class PpoSanctionDetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string TreasuryCode { get; set; } = null!;

    [Column("pensioner_id")]
    public long PensionerId { get; set; }

    [Column("ppo_id")]
    public int PpoId { get; set; }

    [Column("employee_name")]
    [StringLength(500)]
    public string? EmployeeName { get; set; }

    [Column("sanction_authority")]
    [StringLength(500)]
    public string SanctionAuthority { get; set; } = null!;

    [Column("sanction_no")]
    [StringLength(500)]
    public string SanctionNo { get; set; } = null!;

    [Column("sanction_date")]
    public DateOnly SanctionDate { get; set; }

    [Column("employee_dob")]
    public DateOnly? EmployeeDob { get; set; }

    [Column("employee_gender")]
    [MaxLength(1)]
    public char? EmployeeGender { get; set; }

    [Column("employee_date_of_appointment")]
    public DateOnly? EmployeeDateOfAppointment { get; set; }

    [Column("employee_office")]
    [StringLength(500)]
    public string? EmployeeOffice { get; set; }

    [Column("employee_designation")]
    [StringLength(500)]
    public string? EmployeeDesignation { get; set; }

    [Column("employee_last_pay")]
    public int? EmployeeLastPay { get; set; }

    [Column("average_emolument")]
    public int? AverageEmolument { get; set; }

    [Column("employee_hrms_id")]
    [StringLength(500)]
    public string? EmployeeHrmsId { get; set; }

    [Column("issuing_authority")]
    [StringLength(500)]
    public string? IssuingAuthority { get; set; }

    [Column("issuing_letter_no")]
    [StringLength(500)]
    public string? IssuingLetterNo { get; set; }

    [Column("issuing_letter_date")]
    public DateOnly? IssuingLetterDate { get; set; }

    [Column("qualifying_service_gross_years")]
    public int? QualifyingServiceGrossYears { get; set; }

    [Column("qualifying_service_gross_months")]
    public int? QualifyingServiceGrossMonths { get; set; }

    [Column("qualifying_service_gross_days")]
    public int? QualifyingServiceGrossDays { get; set; }

    [Column("qualifying_service_net_years")]
    public int? QualifyingServiceNetYears { get; set; }

    [Column("qualifying_service_net_months")]
    public int? QualifyingServiceNetMonths { get; set; }

    [Column("qualifying_service_net_days")]
    public int? QualifyingServiceNetDays { get; set; }

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

    [ForeignKey("PensionerId")]
    [InverseProperty("PpoSanctionDetails")]
    public virtual Pensioner Pensioner { get; set; } = null!;
}
