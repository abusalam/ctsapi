using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("beneficiaries_master", Schema = "lf_pl_master")]
[Index("OperatorCode", Name = "fki_operator_fk")]
public partial class BeneficiariesMaster
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("ben_name", TypeName = "character varying")]
    public string? BenName { get; set; }

    [Column("acc_no")]
    [StringLength(18)]
    public string? AccNo { get; set; }

    [Column("ifsc")]
    [StringLength(11)]
    public string? Ifsc { get; set; }

    [Column("bank_name", TypeName = "character varying")]
    public string? BankName { get; set; }

    [Column("acc_type", TypeName = "character varying")]
    public string? AccType { get; set; }

    [Column("pan")]
    [StringLength(10)]
    public string? Pan { get; set; }

    [Column("aadhar")]
    [StringLength(12)]
    public string? Aadhar { get; set; }

    [Column("group")]
    [MaxLength(1)]
    public char? Group { get; set; }

    [Column("gpf_no")]
    [StringLength(15)]
    public string? GpfNo { get; set; }

    [Column("email", TypeName = "character varying")]
    public string? Email { get; set; }

    [Column("address", TypeName = "character varying")]
    public string? Address { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("created_by")]
    public short? CreatedBy { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_by")]
    public short? UpdatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("operator_code")]
    public short? OperatorCode { get; set; }

    [Column("mobile_no")]
    [StringLength(10)]
    public string? MobileNo { get; set; }

    [Column("is_verified")]
    public short? IsVerified { get; set; }

    [Column("gstin")]
    [StringLength(15)]
    public string? Gstin { get; set; }

    [Column("ben_type", TypeName = "character varying")]
    public string? BenType { get; set; }

    public virtual BeneficiaryType? BenTypeNavigation { get; set; }
}
