using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Entities;

[Table("vendor_requisition_challan_generate", Schema = "cts")]
public partial class VendorRequisitionChallanGenerate
{
    [Key]
    [Column("vendor_requisition_challan_generate_id")]
    public long VendorRequisitionChallanGenerateId { get; set; }

    [Column("vendor_requisition_staging_id")]
    public long VendorRequisitionStagingId { get; set; }

    [Column("sheet_by_to")]
    public short SheetByTo { get; set; }

    [Column("label_by_to")]
    public short LabelByTo { get; set; }

    [Column("discounted_amount")]
    [Precision(10, 2)]
    public decimal DiscountedAmount { get; set; }

    [Column("tax_amount")]
    [Precision(10, 2)]
    public decimal TaxAmount { get; set; }

    [Column("total_amount")]
    [Precision(10, 2)]
    public decimal TotalAmount { get; set; }

    [Column("hoa", TypeName = "character varying")]
    public string? Hoa { get; set; }

    [Column("requisition_no", TypeName = "character varying")]
    public string? RequisitionNo { get; set; }

    [Column("hoa_id")]
    public long? HoaId { get; set; }

    [Column("grn_no", TypeName = "character varying")]
    public string? GrnNo { get; set; }

    [Column("bank_recipt_no", TypeName = "character varying")]
    public string? BankReciptNo { get; set; }

    [Column("challan_no", TypeName = "character varying")]
    public string? ChallanNo { get; set; }

    [Column("challan_date", TypeName = "timestamp without time zone")]
    public DateTime? ChallanDate { get; set; }

    [Column("voucher_no", TypeName = "character varying")]
    public string? VoucherNo { get; set; }

    [Column("bill_id")]
    public long? BillId { get; set; }

    [Column("is_billed")]
    public bool? IsBilled { get; set; }

    [ForeignKey("VendorRequisitionStagingId")]
    [InverseProperty("VendorRequisitionChallanGenerates")]
    public virtual VendorRequisitionStaging VendorRequisitionStaging { get; set; } = null!;

    [InverseProperty("VendorRequisitionChallanGenerate")]
    public virtual ICollection<VendorStampRequisition> VendorStampRequisitions { get; set; } = new List<VendorStampRequisition>();
}
