using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("vendor_requisition_approve", Schema = "cts")]
public partial class VendorRequisitionApprove
{
    [Key]
    [Column("vendor_requisition_approve_id")]
    public long VendorRequisitionApproveId { get; set; }

    [Column("vendor_requisition_id")]
    public long VendorRequisitionId { get; set; }

    [Column("challan_id")]
    public long ChallanId { get; set; }

    [Column("transaction_id")]
    public long TransactionId { get; set; }

    [Column("approve_by")]
    public long ApproveBy { get; set; }

    [Column("approve_date", TypeName = "timestamp without time zone")]
    public DateTime? ApproveDate { get; set; }
}
