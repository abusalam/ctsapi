using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("sao", Schema = "master")]
public partial class Sao
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("code")]
    [StringLength(8)]
    public string? Code { get; set; }

    [Column("name")]
    [StringLength(200)]
    public string? Name { get; set; }

    [Column("next_level_code")]
    [StringLength(8)]
    public string? NextLevelCode { get; set; }

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [Column("created_on", TypeName = "timestamp without time zone")]
    public DateTime? CreatedOn { get; set; }

    [Column("modified_by")]
    public int? ModifiedBy { get; set; }

    [Column("modified_on")]
    public TimeOnly? ModifiedOn { get; set; }
}
