using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("ddo", Schema = "master")]
[Index("Code", Name = "ddo_code_key", IsUnique = true)]
public partial class Ddo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("treasury_mst_id")]
    public short? TreasuryMstId { get; set; }

    [Column("code")]
    [StringLength(9)]
    public string Code { get; set; } = null!;

    [Column("designation")]
    [StringLength(100)]
    public string? Designation { get; set; }

    [Column("designation_mst_id")]
    public int? DesignationMstId { get; set; }

    [Column("address")]
    [StringLength(500)]
    public string? Address { get; set; }

    [Column("phone")]
    [StringLength(20)]
    public string? Phone { get; set; }

    public virtual ICollection<BeneficiaryMaster> BeneficiaryMasters { get; set; } = new List<BeneficiaryMaster>();

    public virtual ICollection<BillBtdetail> BillBtdetails { get; set; } = new List<BillBtdetail>();

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual ICollection<BillSubdetailInfo> BillSubdetailInfos { get; set; } = new List<BillSubdetailInfo>();
}
