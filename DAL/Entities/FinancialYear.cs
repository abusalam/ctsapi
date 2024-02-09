using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("financial_year", Schema = "master")]
public partial class FinancialYear
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("description", TypeName = "character varying")]
    public string? Description { get; set; }

    [Column("isActive")]
    [MaxLength(1)]
    public char? IsActive { get; set; }
}
