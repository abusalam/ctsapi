using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("rbi_ifsc_stock", Schema = "master")]
public partial class RbiIfscStock
{
    [Column("BranchID")]
    public int? BranchId { get; set; }

    [Column("BankID")]
    public int? BankId { get; set; }

    [Column(TypeName = "character varying")]
    public string? BankName { get; set; }

    [Column("IFSC", TypeName = "character varying")]
    public string? Ifsc { get; set; }

    [Column(TypeName = "character varying")]
    public string? Office { get; set; }

    [Column(TypeName = "character varying")]
    public string? Address { get; set; }

    [Column(TypeName = "character varying")]
    public string? District { get; set; }

    [Column(TypeName = "character varying")]
    public string? City { get; set; }

    [Column(TypeName = "character varying")]
    public string? State { get; set; }

    [Column(TypeName = "character varying")]
    public string? Phone { get; set; }
}
