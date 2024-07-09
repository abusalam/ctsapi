using AutoMapper;
using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DTOs;
using CTS_BE.Helper.Authentication;
using static Dapper.SqlMapper;
namespace CTS_BE.BAL.Services.stamp
{
    public class StampMasterService: IStampMasterService
    {
        private IDiscountDetailsRepository _discountDetailRepo;
        private readonly IStampLabelRepository _stampLabelRepo;
        private readonly IStampCategoryRepository _stampCategoryRepo;
        private readonly IStampVendorRepository _stampVendorRepo;
        private readonly IStampVendorTypeRepository _stampVendorTypeRepo;
        private readonly IStampCombinationRepository _stampCombinationRepo;
        private readonly IStampTypeRepository _stampTypeRepo;
        private readonly IMapper _mapper;
        private readonly IClaimService _auth;

        public StampMasterService(
            IStampLabelRepository stampLabelMasterRepo, 
            IStampCategoryRepository stampCategoryRepo, 
            IStampVendorRepository stampVendorRepo,
            IStampVendorTypeRepository stampVendorTypeRepo,
            IStampTypeRepository stampTypeRepo, 
            IDiscountDetailsRepository discountDetailRepo,
            IStampCombinationRepository stampCombinationRepo,
            IMapper mapper, 
            IClaimService claim)
        {
            _discountDetailRepo = discountDetailRepo;
            _stampLabelRepo = stampLabelMasterRepo;
            _stampCategoryRepo = stampCategoryRepo;
            _stampVendorRepo = stampVendorRepo;
            _stampVendorTypeRepo = stampVendorTypeRepo;
            _stampTypeRepo = stampTypeRepo;
            _stampCombinationRepo = stampCombinationRepo;
            _mapper = mapper;
            _auth = claim;
        }

        // Stamp Label Services
        public async Task<bool> CreateNewStampLabel(StampLabelMasterInsertDTO stampLabel)
        {
            var stampLab = _mapper.Map<StampLabelMaster>(stampLabel);
            stampLab.CreatedAt = DateTime.Now;
            stampLab.CreatedBy = _auth.GetUserId();
            _stampLabelRepo.Add(stampLab);
            _stampLabelRepo.SaveChangesManaged();
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<StampLabelMasterDTO>> ListAllStampLabels(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            //Get all stamp labels with sort & filter parameters
            IEnumerable<StampLabelMasterDTO> stampLabelList = await _stampLabelRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true, 
                entity => new StampLabelMasterDTO
            {
                LabelId = entity.LabelId,
                IsActive = entity.IsActive,
                NoLabelPerSheet = entity.NoLabelPerSheet,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampLabelList;
        }
        
        public async Task<IEnumerable<StampLabelDTO>> GetAllStampLabels()
        {
            IEnumerable<StampLabelDTO> stampLabelMasters = await _stampLabelRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true,
                entity =>new StampLabelDTO
            {
                LabelId = entity.LabelId,
                NoLabelPerSheet = entity.NoLabelPerSheet,
            });
            return stampLabelMasters;
        }

        public async Task<IEnumerable<StampLabelMasterDTO>> GetStampLabelById(long id)
        {

            IEnumerable<StampLabelMasterDTO> stampLabelMasters = await _stampLabelRepo.GetSelectedColumnByConditionAsync(
                e => e.LabelId == id, 
                entity => new StampLabelMasterDTO
            {
                LabelId = entity.LabelId,
                IsActive = entity.IsActive,
                NoLabelPerSheet = entity.NoLabelPerSheet,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
            });

            return stampLabelMasters;
        }
        
        public async Task<bool> DeleteStampLabelsById(long id)
        {
            var stampLabelMasters = await _stampLabelRepo.GetAllByConditionAsync(a=>a.LabelId == id);
            if (stampLabelMasters.Count() > 0)
            {
                foreach (var item in stampLabelMasters)
                {
                    item.IsActive = false;
                    _stampLabelRepo.Update(item);

                }
                _stampLabelRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);

        }

        // Stamp Category Services
        public async Task<IEnumerable<CategoryTypeDTO>> GetAllCategoryType()
        {
            IEnumerable<CategoryTypeDTO> categoryType = await _stampCategoryRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true,
                entity => new CategoryTypeDTO
            {
                StampCategoryId = entity.StampCategoryId,
                StampCategory1 = entity.StampCategory1,
                Description = entity.Description,
            });
            return categoryType;
        }

        public async Task<IEnumerable<StampCategoryDTO>> ListAllStampCategories(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            //Get all stamp categories with sort & filter parameters
            IEnumerable<StampCategoryDTO> stampCategoryList = await _stampCategoryRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true, 
                entity => new StampCategoryDTO
            {
                StampCategoryId = entity.StampCategoryId,
                StampCategory1 = entity.StampCategory1,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampCategoryList;
        }

        public async Task<IEnumerable<StampCategoryDTO>> GetAllStampCategories()
        {
            IEnumerable<StampCategoryDTO> stampCategory = await _stampCategoryRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true, 
                entity => new StampCategoryDTO
            {
                StampCategoryId = entity.StampCategoryId,
                StampCategory1 = entity.StampCategory1,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                
                
            });
            return stampCategory;
        }

        public async Task<bool> CreateNewStampCategory(StampCategoryInsertDTO stampCategory)
        {
            var stampCat = _mapper.Map<StampCategory>(stampCategory);
            stampCat.CreatedAt = DateTime.Now;
            stampCat.CreatedBy = _auth.GetUserId();
            _stampCategoryRepo.Add(stampCat);
            _stampCategoryRepo.SaveChangesManaged();
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<StampCategoryDTO>> GetStampCategoryById(long id)
        {

            IEnumerable<StampCategoryDTO> stampCategory = await _stampCategoryRepo.GetSelectedColumnByConditionAsync(e => e.StampCategoryId == id, entity => new StampCategoryDTO
            {
                StampCategoryId = entity.StampCategoryId,
                StampCategory1 = entity.StampCategory1,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                
                
            });

            return stampCategory;
        }

        public async Task<bool> DeleteStampCategorysById(long id)
        {
            var stampCategory = await _stampCategoryRepo.GetAllByConditionAsync(a => a.StampCategoryId == id);
            if (stampCategory.Count() > 0)
            {
                foreach (var item in stampCategory)
                {
                    item.IsActive = false;
                    _stampCategoryRepo.Update(item);
                }
                _stampCategoryRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        // Stamp Vendor Services
        public async Task<IEnumerable<StampVendorDTO>> ListAllStampVendors(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            //Get all stamp vendors with sort & filter parameters
            IEnumerable<StampVendorDTO> stampVendorList = await _stampVendorRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true, 
                entity => new StampVendorDTO
            {
                VendorName = entity.VendorName,
                LicenseNo = entity.LicenseNo,
                EffectiveFrom = entity.EffectiveFrom.ToString(),
                PanNumber = entity.PanNumber,
                PhoneNumber = entity.PhoneNumber,
                StampVendorId = entity.StampVendorId,
                Address = entity.Address,
                ActiveAtGrips = entity.ActiveAtGrips,
                ValidUpto = entity.ValidUpto.ToString(),
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                VendorType = entity.VendorTypeNavigation.VendorType,
                VendorLicencePhoto = entity.VendorPhoto,
                VendorPanPhoto = entity.VendorPanPhoto,
                VendorPhoto = entity.VendorPhoto,

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampVendorList;
        }

        public async Task<IEnumerable<StampVendorDetailsDropdownDTO>> GetAllStampVendors()
        {
            IEnumerable<StampVendorDetailsDropdownDTO> stampVendor = await _stampVendorRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true, 
                entity => new StampVendorDetailsDropdownDTO {
               
                VendorName = entity.VendorName,
                LicenseNo = entity.LicenseNo,
                PanNumber = entity.PanNumber,
                PhoneNumber = entity.PhoneNumber,
                StampVendorId = entity.StampVendorId,
                VendorTypeId = entity.VendorType,
                VendorType = entity.VendorTypeNavigation.VendorType,
                //EffectiveFrom = entity.EffectiveFrom.ToString(),
                //Address = entity.Address,
                //ActiveAtGrips = entity.ActiveAtGrips,
                //ValidUpto = entity.ValidUpto.ToString(),
                //IsActive = entity.IsActive,
                //CreatedAt = entity.CreatedAt,
                //CreatedBy = entity.CreatedBy,
                //VendorLicencePhoto = entity.VendorPhoto,
                //VendorPanPhoto = entity.VendorPanPhoto,
                //VendorPhoto = entity.VendorPhoto,
            });
            return stampVendor;
        }

        public async Task<bool> CreateNewStampVendor(StampVendorInsertDTO stampVendor, String vendorPhoto, String vendorPanPhoto, String vendorLicencePhoto)
        {
            var stampVen = _mapper.Map<StampVendor>(stampVendor);
            stampVen.CreatedAt = DateTime.Now;
            stampVen.CreatedBy = _auth.GetUserId();
            stampVen.VendorPhoto = vendorPhoto;
            stampVen.VendorPanPhoto = vendorPanPhoto;
            stampVen.VendorLicencePhoto = vendorLicencePhoto;
            _stampVendorRepo.Add(stampVen);
            _stampVendorRepo.SaveChangesManaged();
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<VendorTypeDTO>> GetAllStampVendorTypes()
        {
            IEnumerable<VendorTypeDTO> allVendorTypes = await _stampVendorTypeRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true,
                entity => new VendorTypeDTO
                {
                    StampVendorId = entity.VendorTypeId,
                    VendorType = entity.VendorType,
                });
            return allVendorTypes;
        }
        
        public async Task<IEnumerable<StampVendorDTO>> GetStampVendorById(long id)
        {
            IEnumerable<StampVendorDTO> stampVendor = await _stampVendorRepo.GetSelectedColumnByConditionAsync(e => e.StampVendorId == id, entity => new StampVendorDTO
            {
                LicenseNo = entity.LicenseNo,
                EffectiveFrom = entity.EffectiveFrom.ToString(),
                PanNumber = entity.PanNumber,
                PhoneNumber = entity.PhoneNumber,
                StampVendorId = entity.StampVendorId,
                Address = entity.Address,
                ActiveAtGrips = entity.ActiveAtGrips,
                ValidUpto = entity.ValidUpto.ToString(),
                IsActive = entity.IsActive,
                VendorLicencePhoto = entity.VendorPhoto,
                VendorPanPhoto = entity.VendorPanPhoto,
                VendorPhoto = entity.VendorPhoto,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
            });
            return stampVendor;
        }

        public async Task<bool> DeleteStampVendorsById(long id)
        {
            var stampVendor = await _stampVendorRepo.GetAllByConditionAsync(a => a.StampVendorId == id);
            if (stampVendor.Count() > 0)
            {
                foreach (var item in stampVendor)
                {
                    item.IsActive = false;
                    _stampVendorRepo.Update(item);

                }
                _stampVendorRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        // Stamp Type Services
        public async Task<IEnumerable<StampTypeDTO>> ListAllStampTypes(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            //Get all stamp types with sort & filter parameters
            IEnumerable<StampTypeDTO> stampTypeList = await _stampTypeRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true, 
                entity => new StampTypeDTO
            {
                DenominationId = entity.DenominationId,
                Denomination = entity.Denomination,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampTypeList;
        }

         public async Task<IEnumerable<StampTypeDataDTO>> GetAllStampTypes()
        {
            IEnumerable<StampTypeDataDTO> stampType = await _stampTypeRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true, 
                entity => new StampTypeDataDTO { 
            
                DenominationId = entity.DenominationId,
                Denomination = entity.Denomination
            });
            return stampType;
        }

         public async Task<bool> CreateNewStampType(StampTypeInsertDTO stampType)
        {
            var stampTp = _mapper.Map<StampType>(stampType);
            stampTp.CreatedAt = DateTime.Now;
            stampTp.CreatedBy = _auth.GetUserId();
            _stampTypeRepo.Add(stampTp);
            _stampTypeRepo.SaveChangesManaged();
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<StampTypeDTO>> GetStampTypeById(long id)
        {
            IEnumerable<StampTypeDTO> stampType = await _stampTypeRepo.GetSelectedColumnByConditionAsync(e => e.DenominationId == id, entity => new StampTypeDTO
            {

                DenominationId = entity.DenominationId,
                Denomination = entity.Denomination,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
            });
            return stampType;
        }

        public async Task<bool> DeleteStampTypesById(long id)
        {
            var stampType = await _stampTypeRepo.GetAllByConditionAsync(a => a.DenominationId == id);
            if (stampType.Count() > 0)
            {
                foreach (var item in stampType)
                {
                    item.IsActive = false;
                    _stampTypeRepo.Update(item);

                }
                _stampTypeRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        // Stamp discount details
        public async Task<IEnumerable<DiscountDetailsDTO>> ListAllDiscountDetails(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            
            IEnumerable<DiscountDetailsDTO> discountDetailList = await _discountDetailRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true, 
                entity => new DiscountDetailsDTO
            {
                DiscountId = entity.DiscountId,
                DenominationFrom = entity.DenominationFrom,
                DenominationTo = entity.DenominationTo,
                Discount = entity.Discount,
                VendorType = entity.VendorTypeNavigation.VendorType,
                StampCategory = entity.StampCategory.StampCategory1,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy  = entity.CreatedBy

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return discountDetailList;
        }

        public async Task<bool> CreateNewStampDiscountDetails(DiscountDetailsInsertDTO stampDiscountDetails)
        {
            var stampDiscount = _mapper.Map<DiscountDetail>(stampDiscountDetails);
            stampDiscount.CreatedAt = DateTime.Now;
            stampDiscount.CreatedBy = _auth.GetUserId();
            _discountDetailRepo.Add(stampDiscount);
            _discountDetailRepo.SaveChangesManaged();
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteStampDiscountDetailsById(long id)
        {
            var stampComb = await _discountDetailRepo.GetAllByConditionAsync(a => a.DiscountId == id);
            if (stampComb.Count > 0)
            {
                foreach (var item in stampComb)
                {
                    item.IsActive = false;
                    _discountDetailRepo.Update(item);

                }
                _discountDetailRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<decimal> GetDiscount(long vendorTypeId, long stampCategoryId, decimal amount)
        {
            var discount = await _discountDetailRepo.GetSingleSelectedColumnByConditionAsync(
                entity => entity.IsActive == true && entity.VendorType == vendorTypeId && entity.StampCategoryId == stampCategoryId && amount >= entity.DenominationFrom && amount <= entity.DenominationTo,
                entity => new DiscountDetailsDTO
                {
                    Discount = entity.Discount
                });
            if (discount != null )
            {
                return discount.Discount;
            }
            return 0;
        }
        // Stamp combination
        public async Task<IEnumerable<StampCombinationDTO>> StampCombinationList(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {

        var stampCombinationEntities = await _stampCombinationRepo.GetSelectedColumnByConditionAsync(
        entity => entity.IsActive == true,
        entity => new StampCombinationDTO
        {
            StampCombinationId = entity.StampCombinationId,
            StampCategory1 = entity.StampCategory.StampCategory1,
            Description = entity.StampCategory.Description,
            Denomination = entity.StampType.Denomination,
            StampDenominationId = entity.StampTypeId,
            NoLabelPerSheet = entity.StampLabel.NoLabelPerSheet,
            StampLabelId = entity.StampLabelId,
            IsActive = (bool)entity.IsActive,
            StampCategoryId = entity.StampCategoryId,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
        },
        pageIndex,
        pageSize,
        filters,
        sortParameters?.Field,
        sortParameters?.Order
    );

            // Return the list of StampCombinationDTO
            return stampCombinationEntities;
        }
        
        public async Task<bool> CreateNewStampCombination(StampCombinationInsertDTO newStampCombination)
        {
            var stampCombination = _mapper.Map<StampCombination>(newStampCombination);
            stampCombination.CreatedAt = DateTime.Now;
            stampCombination.CreatedBy = _auth.GetUserId();
            _stampCombinationRepo.Add(stampCombination);
            _stampCombinationRepo.SaveChangesManaged();
            return await Task.FromResult(true);
        }
        
        public async Task<IEnumerable<GetAllStampCombinationDTO>> GetAllStampCombinations()
        {
            IEnumerable<GetAllStampCombinationDTO> stampCombinations = await _stampCombinationRepo.GetSelectedColumnByConditionAsync(
                entity => entity.IsActive == true, 
                entity => new GetAllStampCombinationDTO
            {
                StampCombinationId = entity.StampCombinationId,
                StampCategoryId = entity.StampCategoryId,
                StampCategory1 = entity.StampCategory.StampCategory1,
                Description = entity.StampCategory.Description,
                Denomination = entity.StampType.Denomination,
                NoLabelPerSheet = entity.StampLabel.NoLabelPerSheet
            });
            return stampCombinations;
        }

        public async Task<bool> DeleteStampCombinationById(long id)
        {
            var stampComb = await _stampCombinationRepo.GetAllByConditionAsync(a => a.StampCombinationId == id);
            if (stampComb.Count > 0)
            {
                foreach (var item in stampComb)
                {
                    item.IsActive = false;
                    _stampCombinationRepo.Update(item);

                }
                _stampCombinationRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

    }
}
