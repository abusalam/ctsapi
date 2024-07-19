using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("mm_stamp_label", Schema = "cts_master")]
public partial class MmStampLabel
{
    [Column("label_id")]
    [Precision(3, 0)]
    public decimal? LabelId { get; set; }

    [Column("number_of_label")]
    [Precision(4, 0)]
    public decimal? NumberOfLabel { get; set; }

    [Column("active_flag")]
    [StringLength(1)]
    public string? ActiveFlag { get; set; }

    [Column("user_id")]
    [Precision(8, 0)]
    public decimal? UserId { get; set; }

    [Column("created_timestamp")]
    public DateOnly CreatedTimestamp { get; set; }

    [Column("modified_user_id")]
    [Precision(8, 0)]
    public decimal? ModifiedUserId { get; set; }

    [Column("modified_timestamp")]
    public DateOnly ModifiedTimestamp { get; set; }
}
