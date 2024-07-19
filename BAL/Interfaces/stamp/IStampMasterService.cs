using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.stamp
{
    public interface IStampMasterService
    {
        // Stamp Label Interfaces
         Task<IEnumerable<StampLabelMasterDTO>> ListAllStampLabels(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<IEnumerable<StampLabelDTO>> GetAllStampLabels();
         Task<bool> CreateNewStampLabel(StampLabelMasterInsertDTO stampLabel);
         Task<IEnumerable<StampLabelMasterDTO>> GetStampLabelById(long id);
         Task<bool> DeleteStampLabelsById(long id);

        // Stamp category Interfaces
         Task<IEnumerable<StampCategoryDTO>> ListAllStampCategories(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<IEnumerable<StampCategoryDTO>> GetAllStampCategories();
         Task<IEnumerable<CategoryTypeDTO>> GetAllCategoryType();
         Task<bool> CreateNewStampCategory(StampCategoryInsertDTO stampCategory);
         Task<IEnumerable<StampCategoryDTO>> GetStampCategoryById(long id);
         Task<bool> DeleteStampCategorysById(long id);

        // Stamp Vendor Interfaces
         Task<IEnumerable<StampVendorDTO>> ListAllStampVendors(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<IEnumerable<StampVendorDetailsDropdownDTO>> GetAllStampVendors();
         Task<IEnumerable<VendorTypeDTO>> GetAllStampVendorTypes();
         Task<bool> CreateNewStampVendor(StampVendorInsertDTO stampVendor, String vendorPhoto, String vendorPanPhoto, String vendorLicencePhot);
         Task<IEnumerable<StampVendorDTO>> GetStampVendorById(long id);
         Task<bool> DeleteStampVendorsById(long id);

        // Stamp Type Interfaces
        
         Task<IEnumerable<StampTypeDTO>> ListAllStampTypes(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<IEnumerable<StampTypeDataDTO>> GetAllStampTypes();
         Task<bool> CreateNewStampType(StampTypeInsertDTO stampType);
         Task<IEnumerable<StampTypeDTO>> GetStampTypeById(long id);
         Task<bool> DeleteStampTypesById(long id);

        // discount details
         Task<IEnumerable<DiscountDetailsDTO>> ListAllDiscountDetails(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<bool> CreateNewStampDiscountDetails(DiscountDetailsInsertDTO stampDiscountDetails);
         Task<bool> DeleteStampDiscountDetailsById(long id);
         Task<decimal> GetDiscount(long vendorTypeId, long stampCategoryId, decimal amount);

        // stamp combonation
        Task<IEnumerable<StampCombinationDTO>> StampCombinationList(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<bool> DeleteStampCombinationById(long id);
         Task<bool> CreateNewStampCombination(StampCombinationInsertDTO newStampCombination);
         Task<IEnumerable<GetAllStampCombinationDTO>> GetAllStampCombinations();
    }
}