using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("treasury", Schema = "master")]
[Index("Code", Name = "treasury_code_key", IsUnique = true)]
public partial class Treasury
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("district_name")]
    [StringLength(30)]
    public string? DistrictName { get; set; }

    [Column("district_code")]
    public short? DistrictCode { get; set; }

    [Column("code")]
    [StringLength(3)]
    public string Code { get; set; } = null!;

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    public virtual ICollection<BillBtdetail> BillBtdetails { get; set; } = new List<BillBtdetail>();

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual ICollection<BillSubdetailInfo> BillSubdetailInfos { get; set; } = new List<BillSubdetailInfo>();

    [InverseProperty("Treasury")]
    public virtual ICollection<TreasuryHasBranch> TreasuryHasBranches { get; set; } = new List<TreasuryHasBranch>();
}
