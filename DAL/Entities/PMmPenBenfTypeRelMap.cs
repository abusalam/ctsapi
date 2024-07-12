using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("P_MM_PEN_BENF_TYPE_REL_MAP", Schema = "cts_pension")]
public partial class PMmPenBenfTypeRelMap
{
    [Key]
    [Column("INT_BENF_TYPE_REL_MAP_ID")]
    public long IntBenfTypeRelMapId { get; set; }

    /// <summary>
    /// 5=Family Pension,4=Death Gratuity
    /// </summary>
    [Column("BENF_TYPE_ID")]
    public int BenfTypeId { get; set; }

    /// <summary>
    /// master_type  = &apos;RL&apos;
    /// </summary>
    [Column("INT_OTHER_MASTER_ID")]
    public int IntOtherMasterId { get; set; }
}
