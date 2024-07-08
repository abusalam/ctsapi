using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("vendor_requisition_staging", Schema = "cts")]
public partial class VendorRequisitionStaging
{
    [Key]
    [Column("vendor_requisition_staging_id")]
    public long VendorRequisitionStagingId { get; set; }

    [Column("vendor_requisition_id")]
    public long VendorRequisitionId { get; set; }

    [Column("sheet_by_clerk")]
    public short SheetByClerk { get; set; }

    [Column("label_by_clerk")]
    public short LabelByClerk { get; set; }
}
