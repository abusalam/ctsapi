using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("treasury_has_branch", Schema = "cts-bank-master")]
[Index("BranchsId", Name = "fki_branch_id_fkey")]
[Index("TreasuryId", Name = "fki_treasury_id_fkey")]
public partial class TreasuryHasBranch
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("treasury_id")]
    public short? TreasuryId { get; set; }

    [Column("branchs_id")]
    public long? BranchsId { get; set; }

    [ForeignKey("BranchsId")]
    [InverseProperty("TreasuryHasBranches")]
    public virtual Branch? Branchs { get; set; }

    [ForeignKey("TreasuryId")]
    [InverseProperty("TreasuryHasBranches")]
    public virtual Treasury? Treasury { get; set; }
}
