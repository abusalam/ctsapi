using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.stamp
{
    public interface IStampMasterService
    {
        // Stamp Label Interfaces
        Task<IEnumerable<StampLabelMasterDTO>> ListAllStampLabels(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        public Task<IEnumerable<StampLabelMasterDTO>> GetAllStampLabels();
        public Task<bool> CreateNewStampLabel(StampLabelMasterInsertDTO stampLabel);
        public Task<IEnumerable<StampLabelMasterDTO>> GetStampLabelById(long id);
        public Task<bool> DeleteStampLabelsById(long id);

        // Stamp category Interfaces
        public Task<IEnumerable<StampCategoryDTO>> ListAllStampCategories(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        public Task<IEnumerable<StampCategoryDTO>> GetAllStampCategories();
        public Task<IEnumerable<StampCategoryDTO>> GetAllCategoryType();
        public Task<bool> CreateNewStampCategory(StampCategoryInsertDTO stampCategory);
        public Task<IEnumerable<StampCategoryDTO>> GetStampCategoryById(long id);
        public Task<bool> DeleteStampCategorysById(long id);

        // Stamp Vendor Interfaces
        public Task<IEnumerable<StampVendorDTO>> ListAllStampVendors(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        public Task<IEnumerable<StampVendorDTO>> GetAllStampVendors();
        public Task<IEnumerable<VendorTypeDTO>> GetAllStampVendorTypes();
        public Task<bool> CreateNewStampVendor(StampVendorInsertDTO stampVendor);
        public Task<IEnumerable<StampVendorDTO>> GetStampVendorById(long id);
        public Task<bool> DeleteStampVendorsById(long id);

        // Stamp Type Interfaces
        
        public Task<IEnumerable<StampTypeDTO>> ListAllStampTypes(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        public Task<IEnumerable<StampTypeDTO>> GetAllStampTypes();
        public Task<bool> CreateNewStampType(StampTypeInsertDTO stampType);
        public Task<IEnumerable<StampTypeDTO>> GetStampTypeById(long id);
        public Task<bool> DeleteStampTypesById(long id);

        // discount details
        public Task<IEnumerable<DiscountDetailsDTO>> ListAllDiscountDetails(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        public Task<bool> CreateNewStampDiscountDetails(DiscountDetailsInsertDTO stampDiscountDetails);


        // stamp combonation
        public Task<IEnumerable<StampCombinationDTO>> StampCombinationList(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        public Task<bool> DeleteStampCombinationById(long id);
        public Task<IEnumerable<GetAllStampCombinationDTO>> GetAllStampCombinations();
    }
}