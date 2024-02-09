using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("ddo", Schema = "master")]
public partial class Ddo
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("treasury_code")]
    [StringLength(3)]
    public string? TreasuryCode { get; set; }

    [Column("treasury_mst_id")]
    public short? TreasuryMstId { get; set; }

    [Column("code")]
    [StringLength(9)]
    public string? Code { get; set; }

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
}
