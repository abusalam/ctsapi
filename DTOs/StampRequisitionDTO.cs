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
        public string Status { get; set; }
        public DateOnly RequisitionDate { get; set; }
        public string RaisedToTreasury { get; set; } = null!;
        public short Sheet { get; set; }
        public short Label { get; set; }
        public string RequisitionNo { get; set; } = null!;

        


    }
}