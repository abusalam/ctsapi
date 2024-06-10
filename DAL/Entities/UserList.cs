using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("user_list", Schema = "cts")]
public partial class UserList
{
    [Column("id")]
    public long Id { get; set; }

    [Column("user_id")]
    public short? UserId { get; set; }

    [Column("user_name")]
    [StringLength(100)]
    public string? UserName { get; set; }

    [Column("treasurie_code")]
    [StringLength(3)]
    public string? TreasurieCode { get; set; }
}
