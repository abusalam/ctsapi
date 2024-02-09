using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("district", Schema = "master")]
public partial class District
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("district_code")]
    public int? DistrictCode { get; set; }

    [Column("district_name")]
    [StringLength(30)]
    public string? DistrictName { get; set; }

    [Column("rch_district_code")]
    public int? RchDistrictCode { get; set; }

    [Column("is_revenue_district")]
    public int? IsRevenueDistrict { get; set; }

    [Column("state_code")]
    public int? StateCode { get; set; }

    [Column("district_status")]
    public int? DistrictStatus { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }
}
