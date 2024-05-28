using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.stamp
{
    public interface IStampMasterService
    {
        // Stamp Label Interfaces
         Task<IEnumerable<StampLabelMasterDTO>> ListAllStampLabels(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<IEnumerable<StampLabelMasterDTO>> GetAllStampLabels();
         Task<bool> CreateNewStampLabel(StampLabelMasterInsertDTO stampLabel);
         Task<IEnumerable<StampLabelMasterDTO>> GetStampLabelById(long id);
         Task<bool> DeleteStampLabelsById(long id);

        // Stamp category Interfaces
         Task<IEnumerable<StampCategoryDTO>> ListAllStampCategories(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<IEnumerable<StampCategoryDTO>> GetAllStampCategories();
         Task<IEnumerable<StampCategoryDTO>> GetAllCategoryType();
         Task<bool> CreateNewStampCategory(StampCategoryInsertDTO stampCategory);
         Task<IEnumerable<StampCategoryDTO>> GetStampCategoryById(long id);
         Task<bool> DeleteStampCategorysById(long id);

        // Stamp Vendor Interfaces
         Task<IEnumerable<StampVendorDTO>> ListAllStampVendors(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<IEnumerable<StampVendorDTO>> GetAllStampVendors();
         Task<IEnumerable<VendorTypeDTO>> GetAllStampVendorTypes();
         Task<bool> CreateNewStampVendor(StampVendorInsertDTO stampVendor);
         Task<IEnumerable<StampVendorDTO>> GetStampVendorById(long id);
         Task<bool> DeleteStampVendorsById(long id);

        // Stamp Type Interfaces
        
         Task<IEnumerable<StampTypeDTO>> ListAllStampTypes(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<IEnumerable<StampTypeDTO>> GetAllStampTypes();
         Task<bool> CreateNewStampType(StampTypeInsertDTO stampType);
         Task<IEnumerable<StampTypeDTO>> GetStampTypeById(long id);
         Task<bool> DeleteStampTypesById(long id);

        // discount details
         Task<IEnumerable<DiscountDetailsDTO>> ListAllDiscountDetails(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<bool> CreateNewStampDiscountDetails(DiscountDetailsInsertDTO stampDiscountDetails);


        // stamp combonation
         Task<IEnumerable<StampCombinationDTO>> StampCombinationList(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
         Task<bool> DeleteStampCombinationById(long id);
         Task<IEnumerable<GetAllStampCombinationDTO>> GetAllStampCombinations();
    }
}