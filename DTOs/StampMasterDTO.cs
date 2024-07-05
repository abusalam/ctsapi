using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CTS_BE.Common;
using CTS_BE.DAL.Entities;

namespace CTS_BE.DTOs   // TODO: Update API will be required in future.
{
    public class StampLabelMasterDTO
    {
        public short NoLabelPerSheet { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }

        public long LabelId { get; set; }
    }

    public class StampLabelDTO
    {
        public short NoLabelPerSheet { get; set; }

        public long LabelId { get; set; }
    }

    public class StampLabelMasterInsertDTO
    {
        [Required]
        public short NoLabelPerSheet { get; set; }
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

    public class CategoryTypeDTO
    {
        public string? StampCategory1 { get; set; }

        public long StampCategoryId { get; set; }

        public string Description { get; set; } = null!;

    }

    public class StampCategoryInsertDTO
    {

        [Required]
        [StringLength(2, ErrorMessage = "String Length must be 2.")]
        public string StampCategory1 { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
    }

    public class StampVendorDTO
    {

        public string? VendorType { get; set; }

        public long StampVendorId { get; set; }

        public string LicenseNo { get; set; } = null!;

        public string Address { get; set; } = null!;

        public long? PhoneNumber { get; set; }

        public string? EffectiveFrom { get; set; }

        public string? ValidUpto { get; set; }

        public string PanNumber { get; set; } = null!;

        public bool? IsActive { get; set; }

        public bool? ActiveAtGrips { get; set; }
        public string? VendorPhoto { get; set; }

        public string? VendorPanPhoto { get; set; }

        public string? VendorLicencePhoto { get; set; }

        public DateTime? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }
    }

    public class VendorTypeDTO
    {
        public string? VendorType { get; set; }

        public long StampVendorId { get; set; }
    }
    
    public class StampVendorInsertDTO
    {
        [Required]
        public string? VendorType { get; set; }

        [Required]
        public string LicenseNo { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Invalid Phone Number.")]
        public long? PhoneNumber { get; set; }

        [Required]
        public DateTime? EffectiveFrom { get; set; }

        [Required]
        public DateTime? ValidUpto { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN Number.")]
        public string PanNumber { get; set; } = null!;

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

    public class StampTypeDataDTO
    {
        public decimal Denomination { get; set; }

        public long DenominationId { get; set; }
    }

    public class StampTypeInsertDTO
    {
        [Required]
        [Precision(10, 2)]
        public decimal Denomination { get; set; }
    }

    public class DiscountDetailsDTO
    {
        public long DiscountId { get; set; }

        public decimal DenominationFrom { get; set; }

        public decimal DenominationTo { get; set; }

        public decimal Discount { get; set; }

        public string? VendorType { get; set; }

        public string? StampCategory { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public long? CreatedBy { get; set; }
    }

    public class DiscountDetailsInsertDTO
    {
        [Required, Precision(10, 2)]
        public decimal DenominationFrom { get; set; }

        [Required, Precision(10, 2)]
        public decimal DenominationTo { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public string? VendorType { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "String Length must be 2.")]
        public string? StampCategory { get; set; }

    }
    
    public class StampCombinationDTO
    {
        public long StampCombinationId { get; set; }
        //public long LabelId { get; set; }
        public string? StampCategory1 { get; set; }
        public long StampCategoryId { get; set; }
        public string? Description { get; set; }
        public decimal Denomination { get; set; }
        public long StampDenominationId { get; set; }
        public int NoLabelPerSheet { get; set; }
        public long StampLabelId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? CreatedBy { get; set; }
    }
    
    public class GetAllStampCombinationDTO
    {
        public long StampCombinationId { get; set; }
        public string? StampCategory1 { get; set; }
        public string? Description { get; set; }
        public decimal Denomination { get; set; }
        public int NoLabelPerSheet { get; set; }
    }
    public class StampCombinationInsertDTO
    {
        [Required]
        public long StampCategoryId { get; set; }

        [Required]
        public long StampTypeId { get; set; }

        [Required] 
        public long StampLabelId { get; set; }
    }

    public class DiscountDetailsUpdateDTO
    {
        public long DiscountId { get; set; }

        [Required]
        public decimal DenominationFrom { get; set; }

        [Required]
        public decimal DenominationTo { get; set; }

        [Required]
        public decimal Discount { get; set; }

    }

}