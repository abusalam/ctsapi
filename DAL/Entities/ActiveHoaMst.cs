using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("active_hoa_mst", Schema = "bantan_master")]
public partial class ActiveHoaMst
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("dept_code")]
    [StringLength(2)]
    public string DeptCode { get; set; } = null!;

    [Column("demand_no")]
    [StringLength(2)]
    public string DemandNo { get; set; } = null!;

    [Column("major_head")]
    [StringLength(4)]
    public string MajorHead { get; set; } = null!;

    [Column("submajor_head")]
    [StringLength(2)]
    public string SubmajorHead { get; set; } = null!;

    [Column("minor_head")]
    [StringLength(3)]
    public string MinorHead { get; set; } = null!;

    [Column("plan_status")]
    [StringLength(2)]
    public string PlanStatus { get; set; } = null!;

    [Column("scheme_head")]
    [StringLength(3)]
    public string SchemeHead { get; set; } = null!;

    [Column("detail_head")]
    [StringLength(2)]
    public string DetailHead { get; set; } = null!;

    [Column("subdetail_head")]
    [StringLength(2)]
    public string SubdetailHead { get; set; } = null!;

    [Column("voted_charged")]
    [MaxLength(1)]
    public char VotedCharged { get; set; }

    [Column("description", TypeName = "character varying")]
    public string? Description { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }

    [Column("activated_by")]
    public short? ActivatedBy { get; set; }

    [Column("financial_year")]
    [StringLength(50)]
    public string? FinancialYear { get; set; }

    [InverseProperty("ActiveHoa")]
    public virtual ICollection<BillSubdetailInfo> BillSubdetailInfos { get; set; } = new List<BillSubdetailInfo>();

    [InverseProperty("ActiveHoa")]
    public virtual ICollection<DdoAllotmentBookedBill> DdoAllotmentBookedBills { get; set; } = new List<DdoAllotmentBookedBill>();
}
