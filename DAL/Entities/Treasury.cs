using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("treasury", Schema = "master")]
[Index("Code", Name = "Uk_treasury_code", IsUnique = true)]
public partial class Treasury
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("district_name")]
    [StringLength(30)]
    public string? DistrictName { get; set; }

    [Column("district_code")]
    public short? DistrictCode { get; set; }

    [Column("code")]
    [StringLength(3)]
    public string? Code { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }
}
