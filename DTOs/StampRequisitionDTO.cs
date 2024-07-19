using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CTS_BE.Common;
using CTS_BE.DAL.Entities;
using System.ComponentModel;

namespace CTS_BE.DTOs
{
    public class StampRequisitionDTO
    {
        public long VendorStampRequisitionId { get; set; }
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorType { get; set; }
        public string LicenseNo { get; set; }
        public decimal Amount { get; set; }
        public short Quantity { get; set; }
        public string Status { get; set; }
        public DateOnly RequisitionDate { get; set; }
        public string RaisedToTreasury { get; set; } = null!;
        public short Sheet { get; set; }
        public short Label { get; set; }
        public string RequisitionNo { get; set; } = null!;
    }
    public class StampRequisitionInsertDTO
    {
        public long VendorId { get; set; }
        public short Sheet { get; set; }
        public short Label { get; set; }
        public long CombinationId { get; set; }
        public DateTime? RequisitionDate { get; set; }
        public string RequisitionNo { get; set; } = null!;
        public decimal ChallanAmount { get; set; }
        public string RaisedToTreasury { get; set; } = null!;
    }
    public class StampRequisitionApprovedByClerkDTO
    {
        public long VendorStampRequisitionId { get; set; }
        public short SheetByClerk { get; set; }
        public short LabelByClerk { get; set; }
    }
    public class StampRequisitionApprovedByTODTO
    {
        public long? VendorRequisitionStagingId { get; set; }
        public short SheetByTo { get; set; }
        public short LabelByTo { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ChallanAmount { get; set; }
        public string RequisitionNo { get; set; }
        public string Head { get; set; }
        public long? HoaId { get; set; }

    }

    public class StampRequisitionApprovedByTODataDTO
    {
        public long vendorStampRequisitionId { get; set; }
        public short SheetByTo { get; set; }
        public short LabelByTo { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ChallanAmount { get; set; }

    }
    public class HoaDataDTO
    {
        public string Head { get; set; } = "";
        public long? HoaId { get; set; } 
        public string RequisitionNo { get; set; } = "";
        public long? VendorRequisitionStagingId { get; set; }
    }

    public class StampRequisitionPaymentDTO
    {
        public long VendorStampRequisitionId { get; set; }
        public long GRNNo { get; set; }

    }
    public class TRFormDataDTO
    {
        public string RaisedToTreasury { get; set; } = "";
        public string Hoa { get; set; } = "";
        public string DetailHead { get; set; } = "";
        public decimal Amount { get; set; } = 0;
        public string VendorName { get; set; } = "";
        public string VendorAddress { get; set; } = "";
        public string TreasuryName { get; set; } = "";
    }

    public class CalculationDTO
    {
        public decimal Amount { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal TaxAmount { get; set; } = 0;
        public decimal ChallanAmount { get; set; } = 0;
    }
    
    public class DataForCalculationDTO
    {
        public long VendorStampRequisitionId { get; set; }
        public short Sheet { get; set; }
        public short Label { get; set; }
    }


}