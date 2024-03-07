using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("tr_master", Schema = "master")]
public partial class TrMaster
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("form_name", TypeName = "character varying")]
    public string? FormName { get; set; }

    [Column("wb_form_code", TypeName = "character varying")]
    public string? WbFormCode { get; set; }

    [Column("is_employee")]
    public int? IsEmployee { get; set; }

    [Column("is_integrated_form")]
    public int? IsIntegratedForm { get; set; }

    [Column("form_url", TypeName = "character varying")]
    public string? FormUrl { get; set; }

    [Column("view_url", TypeName = "character varying")]
    public string? ViewUrl { get; set; }

    [Column("enabled")]
    public int? Enabled { get; set; }

    [Column("pdate")]
    public DateTime? Pdate { get; set; }

    [Column("tdate")]
    public DateTime? Tdate { get; set; }

    [Column("ag_category", TypeName = "character varying")]
    public string? AgCategory { get; set; }

    [Column("go_no", TypeName = "character varying")]
    public string? GoNo { get; set; }

    [InverseProperty("TrMaster")]
    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
}
