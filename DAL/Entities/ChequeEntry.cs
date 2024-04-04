﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("cheque_entry", Schema = "cts")]
public partial class ChequeEntry
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("financial_year_id")]
    public short FinancialYearId { get; set; }

    [Column("series_no", TypeName = "character varying")]
    public string SeriesNo { get; set; } = null!;

    [Column("quantity")]
    public short Quantity { get; set; }

    [Column("start")]
    public short Start { get; set; }

    [Column("end")]
    public short End { get; set; }

    [Column("current_position")]
    public short CurrentPosition { get; set; }

    [Column("is_used")]
    public bool IsUsed { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
