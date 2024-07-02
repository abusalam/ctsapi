using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("cheque_distribute", Schema = "cts")]
public partial class ChequeDistribute
{
    [Column("id")]
    public long Id { get; set; }

    [Column("micr_code")]
    [StringLength(9)]
    public string? MicrCode { get; set; }

    [Column("user_id")]
    public short? UserId { get; set; }

    [Column("start")]
    public short? Start { get; set; }

    [Column("end")]
    public short? End { get; set; }

    [Column("quantity")]
    public short? Quantity { get; set; }

    [Column("series_no")]
    [StringLength(11)]
    public string? SeriesNo { get; set; }

    [Column("treasurie_code")]
    [StringLength(3)]
    public string? TreasurieCode { get; set; }

    [Column("currrent_position")]
    public short? CurrrentPosition { get; set; }
}
