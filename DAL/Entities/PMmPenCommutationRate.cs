using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_COMMUTATION_RATE", Schema = "cts_pension")]
public partial class PMmPenCommutationRate
{
    [Key]
    [Column("INT_PEN_COMMUTATION_RATE_ID")]
    public long IntPenCommutationRateId { get; set; }

    [Column("AGE_ON_NEXT_BIRTHDAY")]
    public int? AgeOnNextBirthday { get; set; }

    [Column("COMMUTATION_RATE")]
    public int? CommutationRate { get; set; }
}
