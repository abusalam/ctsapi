using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cpin_master", Schema = "master")]
public partial class CpinMaster
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("cpin_id")]
    [StringLength(14)]
    public string CpinId { get; set; } = null!;

    [Column("cpin_amount")]
    public decimal CpinAmount { get; set; }

    [Column("cpin_date", TypeName = "timestamp without time zone")]
    public DateTime? CpinDate { get; set; }

    [Column("cpin_type")]
    public int? CpinType { get; set; }

    [Column("cpin_sub_type")]
    public int? CpinSubType { get; set; }

    [Column("active_flag")]
    public int? ActiveFlag { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by_userid")]
    public long? CreatedByUserid { get; set; }

    [Column("updated_by_userid")]
    public long? UpdatedByUserid { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("ddo_gstin")]
    [StringLength(15)]
    public string? DdoGstin { get; set; }

    [Column("vendor_data", TypeName = "jsonb")]
    public string? VendorData { get; set; }

    [Column("epsId")]
    public long? EpsId { get; set; }

    [InverseProperty("CpinMst")]
    public virtual ICollection<CpinVenderMst> CpinVenderMsts { get; set; } = new List<CpinVenderMst>();
}
