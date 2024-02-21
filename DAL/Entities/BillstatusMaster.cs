using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("billstatus_master", Schema = "master")]
public partial class BillstatusMaster
{
    [Column("id")]
    public short? Id { get; set; }

    [Column("description", TypeName = "character varying")]
    public string? Description { get; set; }

    [Column("status", TypeName = "character varying")]
    public string? Status { get; set; }

    [Column("remarks", TypeName = "character varying")]
    public string? Remarks { get; set; }
}
