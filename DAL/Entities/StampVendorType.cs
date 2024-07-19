using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("stamp_vendor_type", Schema = "cts_master")]
public partial class StampVendorType
{
    [Key]
    [Column("vendor_type_id")]
    public long VendorTypeId { get; set; }

    [Column("vendor_type", TypeName = "character varying")]
    public string VendorType { get; set; } = null!;

    [Column("description", TypeName = "character varying")]
    public string Description { get; set; } = null!;

    [Required]
    [Column("is_active")]
    public bool? IsActive { get; set; }

    [InverseProperty("VendorTypeNavigation")]
    public virtual ICollection<DiscountDetail> DiscountDetails { get; set; } = new List<DiscountDetail>();

    [InverseProperty("VendorTypeNavigation")]
    public virtual ICollection<StampVendor> StampVendors { get; set; } = new List<StampVendor>();
}
