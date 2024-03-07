using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("bill_subdetail_info", Schema = "billing")]
public partial class BillSubdetailInfo
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("bill_id")]
    public long BillId { get; set; }

    [Column("active_hoa_id")]
    public long ActiveHoaId { get; set; }

    [Column("amount")]
    [Precision(10, 2)]
    public decimal Amount { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("created_by_userid")]
    public long? CreatedByUserid { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_by_userid")]
    public long? UpdatedByUserid { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ActiveHoaId")]
    [InverseProperty("BillSubdetailInfos")]
    public virtual ActiveHoaMst ActiveHoa { get; set; } = null!;

    [ForeignKey("BillId")]
    [InverseProperty("BillSubdetailInfos")]
    public virtual BillDetail Bill { get; set; } = null!;
}
