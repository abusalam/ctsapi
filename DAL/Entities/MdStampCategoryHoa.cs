using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("md_stamp_category_hoa", Schema = "cts_master")]
public partial class MdStampCategoryHoa
{
    [Column("hoa_id")]
    [Precision(6, 0)]
    public decimal? HoaId { get; set; }

    [Column("stamp_category")]
    [StringLength(2)]
    public string? StampCategory { get; set; }

    [Column("int_stamp_category_id")]
    public decimal? IntStampCategoryId { get; set; }

    [Column("payment_hoa_id")]
    [Precision(6, 0)]
    public decimal? PaymentHoaId { get; set; }
}
