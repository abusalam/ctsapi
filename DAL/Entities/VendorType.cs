using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("vendor_type", Schema = "cts_master")]
public partial class VendorType
{
    [Key]
    [Column("vendor_type_id")]
    public long VendorTypeId { get; set; }

    [Column("vendor_type")]
    public short VendorType1 { get; set; }

    [Column("description", TypeName = "character varying")]
    public string Description { get; set; } = null!;

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [InverseProperty("VendorType")]
    public virtual ICollection<DiscountDetail> DiscountDetails { get; set; } = new List<DiscountDetail>();

    [InverseProperty("VendorType")]
    public virtual ICollection<StampVendor> StampVendors { get; set; } = new List<StampVendor>();
}
