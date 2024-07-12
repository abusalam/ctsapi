using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_HOA_MAP", Schema = "cts_pension")]
public partial class PMmPenHoaMap
{
    [Key]
    [Column("INT_HOA_MAP_ID")]
    public long IntHoaMapId { get; set; }

    [Column("FULL_HOA")]
    [StringLength(200)]
    public string? FullHoa { get; set; }

    [Column("HOA_NAME")]
    [StringLength(200)]
    public string? HoaName { get; set; }

    [Column("HOA_TYPE_ABBR")]
    [StringLength(50)]
    public string? HoaTypeAbbr { get; set; }

    [Column("ACTIVE_FLAG")]
    [MaxLength(1)]
    public char ActiveFlag { get; set; }

    [Column("CREATED_USER_ID")]
    public int CreatedUserId { get; set; }

    [Column("CREATED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? CreatedTimestamp { get; set; }

    [Column("MODIFIED_USER_ID")]
    public int ModifiedUserId { get; set; }

    [Column("MODIFIED_TIMESTAMP", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedTimestamp { get; set; }
}
