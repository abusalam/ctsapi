using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Keyless]
[Table("tr_master_content", Schema = "master")]
public partial class TrMasterContent
{
    [Column("forms", TypeName = "character varying")]
    public string? Forms { get; set; }

    [Column("has_bt_details")]
    public short? HasBtDetails { get; set; }

    [Column("has_certificate_details")]
    public short? HasCertificateDetails { get; set; }

    [Column("has_neftorcheque_details")]
    public short? HasNeftorchequeDetails { get; set; }

    [Column("has_serviceprov_details")]
    public short? HasServiceprovDetails { get; set; }

    [Column("tr_master_id")]
    public int? TrMasterId { get; set; }
}
