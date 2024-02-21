using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("bill_current_status_master", Schema = "master")]
public partial class BillCurrentStatusMaster
{
    [Key]
    [Column("status_id")]
    public short StatusId { get; set; }

    [Column("status_code", TypeName = "character varying")]
    public string StatusCode { get; set; } = null!;
}
