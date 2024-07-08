using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("vendor_stamp_requisition", Schema = "cts")]
public partial class VendorStampRequisition
{
    [Key]
    [Column("vendor_stamp_requisition_id")]
    public long VendorStampRequisitionId { get; set; }

    [Column("vendor_id")]
    public long VendorId { get; set; }

    [Column("raised_to_treasury")]
    [StringLength(3)]
    public string RaisedToTreasury { get; set; } = null!;

    [Column("sheet")]
    public short Sheet { get; set; }

    [Column("label")]
    public short Label { get; set; }

    [Column("combination_id")]
    public long CombinationId { get; set; }

    [Column("requisition_no", TypeName = "character varying")]
    public string RequisitionNo { get; set; } = null!;

    [Column("requisition_date")]
    public DateOnly RequisitionDate { get; set; }

    [Column("vendor_requisition_staging_id")]
    public long? VendorRequisitionStagingId { get; set; }

    [Column("vendor_requisition_challan_generate_id")]
    public long? VendorRequisitionChallanGenerateId { get; set; }

    [Column("vendor_requisition_approve_id")]
    public long? VendorRequisitionApproveId { get; set; }

    [Column("status_id")]
    public int StatusId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public long CreatedBy { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_by")]
    public long? UpdatedBy { get; set; }

    [ForeignKey("CombinationId")]
    [InverseProperty("VendorStampRequisitions")]
    public virtual StampCombination Combination { get; set; } = null!;

    public virtual Treasury RaisedToTreasuryNavigation { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("VendorStampRequisitions")]
    public virtual Status Status { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("VendorStampRequisitions")]
    public virtual StampVendor Vendor { get; set; } = null!;

    [ForeignKey("VendorRequisitionApproveId")]
    [InverseProperty("VendorStampRequisitions")]
    public virtual VendorRequisitionApprove? VendorRequisitionApprove { get; set; }

    [InverseProperty("VendorRequisition")]
    public virtual ICollection<VendorRequisitionApprove> VendorRequisitionApproves { get; set; } = new List<VendorRequisitionApprove>();

    [ForeignKey("VendorRequisitionChallanGenerateId")]
    [InverseProperty("VendorStampRequisitions")]
    public virtual VendorRequisitionChallanGenerate? VendorRequisitionChallanGenerate { get; set; }

    [ForeignKey("VendorRequisitionStagingId")]
    [InverseProperty("VendorStampRequisitions")]
    public virtual VendorRequisitionStaging? VendorRequisitionStaging { get; set; }

    [InverseProperty("VendorRequisition")]
    public virtual ICollection<VendorRequisitionStaging> VendorRequisitionStagings { get; set; } = new List<VendorRequisitionStaging>();
}
