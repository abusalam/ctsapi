using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("available")]
public partial class Available
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("financial_year_id")]
    public short? FinancialYearId { get; set; }

    [Column("count")]
    public int? Count { get; set; }

    [Column("utilized")]
    public int? Utilized { get; set; }

    [Column("?column?")]
    public int? Column { get; set; }
}
