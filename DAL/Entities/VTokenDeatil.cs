using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("v_token_deatils")]
public partial class VTokenDeatil
{
    [Column("token_id")]
    public long? TokenId { get; set; }
}
