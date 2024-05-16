using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("ecs_neft_previous_record", Schema = "billing")]
public partial class EcsNeftPreviousRecord
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("ecs_neft_details_id")]
    public long? EcsNeftDetailsId { get; set; }

    [Column("previous_bill_id")]
    public long? PreviousBillId { get; set; }

    [Column("previous_lot_id")]
    public long? PreviousLotId { get; set; }

    [Column("financial_year_id")]
    public short? FinancialYearId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
