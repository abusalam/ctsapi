using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CTS_BE.Common;

namespace CTS_BE.DTOs
{
    public class StampLabelMasterDTO
    {
        public int NoLabelPerSheet { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }

        public long LabelId { get; set; }
    }
    public class StampLabelMasterInsertDTO
    {
        public int NoLabelPerSheet { get; set; }

        [Required]
        public bool? IsActive { get; set; }
    }

    public class StampCategoryDTO
    {
        public string StampCategory1 { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }

        public long StampCategoryId { get; set; }
    }
    public class StampCategoryInsertDTO
    {

        [StringLength(2, ErrorMessage = "String Length must be 2.")]
        public string StampCategory1 { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public bool? IsActive { get; set; }

    }

    public class StampVendorDTO
    {
        public string LicenseNo { get; set; } = null!;

        public string Address { get; set; } = null!;

        public long? PhoneNumber { get; set; }

        public string? EffectiveFrom { get; set; }

        public string? ValidUpto { get; set; }

        public string PanNumber { get; set; } = null!;

        public bool? IsActive { get; set; }

        public bool? ActiveAtGrips { get; set; }

        public DateTime? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }

        public long StampVendorId { get; set; }

    }
    public class StampVendorInsertDTO
    {

        public string LicenseNo { get; set; } = null!;

        public string Address { get; set; } = null!;

        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Invalid Phone Number.")]
        public long? PhoneNumber { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? ValidUpto { get; set; }

        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN Number.")]
        public string PanNumber { get; set; } = null!;

        public bool? IsActive { get; set; }

        public bool? ActiveAtGrips { get; set; }

    }
    public class StampTypeDTO
    {
        public decimal Denomination { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }

        public long DenominationId { get; set; }
    }
    public class StampTypeInsertDTO
    {

        [Precision(10, 2)]
        public decimal Denomination { get; set; }

        [Required]
        public bool? IsActive { get; set; }
    }
}
