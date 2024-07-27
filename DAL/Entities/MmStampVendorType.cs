using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("mm_stamp_vendor_type", Schema = "cts_master")]
public partial class MmStampVendorType
{
    [Column("int_vendortype_id")]
    [Precision(6, 0)]
    public decimal? IntVendortypeId { get; set; }

    [Column("vendor_type")]
    [StringLength(1)]
    public string? VendorType { get; set; }

    [Column("description")]
    [StringLength(20)]
    public string? Description { get; set; }

    [Column("active_flag")]
    [StringLength(1)]
    public string? ActiveFlag { get; set; }
}
