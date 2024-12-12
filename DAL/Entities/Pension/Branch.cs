using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities.Pension;

/// <summary>
/// PensionModuleSchema v1
/// </summary>
[Table("branches", Schema = "cts_pension")]
public partial class Branch
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("bank_id")]
    public long BankId { get; set; }

    [Column("branch_name")]
    [StringLength(100)]
    public string BranchName { get; set; } = null!;

    [Column("branch_address")]
    [StringLength(500)]
    public string BranchAddress { get; set; } = null!;

    [Column("ifsc_code")]
    [StringLength(11)]
    public string IfscCode { get; set; } = null!;

    [Column("district_name")]
    [StringLength(500)]
    public string DistrictName { get; set; } = null!;

    [Column("city_name")]
    [StringLength(100)]
    public string CityName { get; set; } = null!;

    [Column("state_name")]
    [StringLength(100)]
    public string StateName { get; set; } = null!;

    [Column("phone_no")]
    [StringLength(100)]
    public string PhoneNo { get; set; } = null!;

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

    [ForeignKey("BankId")]
    [InverseProperty("Branches")]
    public virtual Bank Bank { get; set; } = null!;

    [InverseProperty("Branch")]
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [InverseProperty("Branch")]
    public virtual ICollection<Nominee> Nominees { get; set; } = new List<Nominee>();

    [InverseProperty("Branch")]
    public virtual ICollection<Pensioner> Pensioners { get; set; } = new List<Pensioner>();
}
