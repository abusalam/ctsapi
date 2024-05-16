using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DTOs
{
    public class StampLabelMasterDTO
    {
        public long Id { get; set; }
        public int NoLabelPerSheet { get; set; }
        public bool? IsActive { get; set; }
        public long LabelId { get; set; }
    }
    public class StampLabelMasterInsertDTO
    {
        public long LabelId { get; set; }
        public int NoLabelPerSheet { get; set; }
        public bool? IsActive { get; set; }
    }

    public class StampCategoryDTO
    {
        public long Id { get; set; }

        [StringLength(2, ErrorMessage = "String Length must be 2.")]
        public string StampCategory1 { get; set; } = null!;

        public string? Description { get; set; }

        public bool? IsActive { get; set; }

        public long StampCategoryId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }
    }
    public class StampCategoryInsertDTO
    {
        [StringLength(2, ErrorMessage = "String Length must be 2.")]
        public string StampCategory1 { get; set; } = null!;

        public string? Description { get; set; }

        public bool? IsActive { get; set; }

        public long StampCategoryId { get; set; }

    }

    public class StampVendorDTO
    {
        public long Id { get; set; }

        public int VendorCode { get; set; }

        public string VendorType { get; set; } = null!;

        public string LicenseNo { get; set; } = null!;

        public string Address { get; set; } = null!;

        public long? PhoneNumber { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? ValidUpto { get; set; }

        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN Number.")]
        public string PanNumber { get; set; } = null!;

        public bool? IsActive { get; set; }

        public bool? ActiveAtGrips { get; set; }

        public DateTime? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }
    }
    public class StampVendorInsertDTO
    {

        public int VendorCode { get; set; }

        public string VendorType { get; set; } = null!;

        public string LicenseNo { get; set; } = null!;

        public string Address { get; set; } = null!;

        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Invalid Phone Number.")]
        public long? PhoneNumber { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? ValidUpto { get; set; }

        public string PanNumber { get; set; } = null!;

        public bool? IsActive { get; set; }

        public bool? ActiveAtGrips { get; set; }

    }

    public class StampTypeDTO
    {
        public long Id { get; set; }

        public decimal Denomination { get; set; }

        public bool? IsActive { get; set; }

        public long DenominationId { get; set; }
    }
    public class StampTypeInsertDTO
    {

        public decimal Denomination { get; set; }

        public bool? IsActive { get; set; }

        public long DenominationId { get; set; }
    }
}
