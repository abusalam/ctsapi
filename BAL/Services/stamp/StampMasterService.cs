using AutoMapper;
using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DTOs;
using CTS_BE.Helper.Authentication;
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
        async Task<bool> IStampMasterService.CreateNewStampLabel(StampLabelMasterInsertDTO stampLabel)
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
            IEnumerable<StampLabelMasterDTO> stampLabelList = await _stampLabelRepo.GetSelectedColumnByConditionAsync(entity => true, entity => new StampLabelMasterDTO
            {
                LabelId = entity.LabelId,
                IsActive = entity.IsActive,
                NoLabelPerSheet = entity.NoLabelPerSheet,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampLabelList;
        }
        async Task<IEnumerable<StampLabelMasterDTO>> IStampMasterService.GetAllStampLabels()
        {
            IEnumerable<StampLabelMasterDTO> stampLabelMasters = await _stampLabelRepo.GetSelectedColumnAsync(entity=>new StampLabelMasterDTO
            {
                LabelId = entity.LabelId,
                IsActive = entity.IsActive,
                NoLabelPerSheet = entity.NoLabelPerSheet,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
            });
            return stampLabelMasters;
        }

        async Task<IEnumerable<StampLabelMasterDTO>> IStampMasterService.GetStampLabelById(long id)
        {

            IEnumerable<StampLabelMasterDTO> stampLabelMasters = await _stampLabelRepo.GetSelectedColumnByConditionAsync(e => e.LabelId == id, entity => new StampLabelMasterDTO
            {
                LabelId = entity.LabelId,
                IsActive = entity.IsActive,
                NoLabelPerSheet = entity.NoLabelPerSheet,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
            });

            return stampLabelMasters;
        }
        async Task<bool> IStampMasterService.DeleteStampLabelsById(long id)
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

        public async Task<IEnumerable<StampCategoryDTO>> GetAllCategoryType()
        {
            IEnumerable<StampCategoryDTO> categoryType = await _stampCategoryRepo.GetSelectedColumnAsync(entity => new StampCategoryDTO
            {
                StampCategoryId = entity.StampCategoryId,
                StampCategory1 = entity.StampCategory1 
            });
            return categoryType;
        }



        public async Task<IEnumerable<StampCategoryDTO>> ListAllStampCategories(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            //Get all stamp categories with sort & filter parameters
            IEnumerable<StampCategoryDTO> stampCategoryList = await _stampCategoryRepo.GetSelectedColumnByConditionAsync(entity => true, entity => new StampCategoryDTO
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

        async Task<IEnumerable<StampCategoryDTO>> IStampMasterService.GetAllStampCategories()
        {
            IEnumerable<StampCategoryDTO> stampCategory = await _stampCategoryRepo.GetSelectedColumnAsync(entity => new StampCategoryDTO
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

        async Task<bool> IStampMasterService.CreateNewStampCategory(StampCategoryInsertDTO stampCategory)
        {
            var stampCat = _mapper.Map<StampCategory>(stampCategory);
            stampCat.CreatedAt = DateTime.Now;
            stampCat.CreatedBy = _auth.GetUserId();
            _stampCategoryRepo.Add(stampCat);
            _stampCategoryRepo.SaveChangesManaged();
            return await Task.FromResult(true);
        }

        async Task<IEnumerable<StampCategoryDTO>> IStampMasterService.GetStampCategoryById(long id)
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

        async Task<bool> IStampMasterService.DeleteStampCategorysById(long id)
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
            IEnumerable<StampVendorDTO> stampVendorList = await _stampVendorRepo.GetSelectedColumnByConditionAsync(entity => true, entity => new StampVendorDTO
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
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                VendorType = entity.VendorType,

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampVendorList;
        }

        async Task<IEnumerable<StampVendorDTO>> IStampMasterService.GetAllStampVendors()
        {
            IEnumerable<StampVendorDTO> stampVendor = await _stampVendorRepo.GetSelectedColumnAsync(entity => new StampVendorDTO {
               
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
                VendorType = entity.VendorType,
            });
            return stampVendor;
        }

        async Task<bool> IStampMasterService.CreateNewStampVendor(StampVendorInsertDTO stampVendor)
        {
            var stampVen = _mapper.Map<StampVendor>(stampVendor);
            stampVen.CreatedAt = DateTime.Now;
            stampVen.CreatedBy = _auth.GetUserId();
            _stampVendorRepo.Add(stampVen);
            _stampVendorRepo.SaveChangesManaged();
            return await Task.FromResult(true);
        }

        async Task<IEnumerable<StampVendorDTO>> IStampMasterService.GetStampVendorById(long id)
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
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
            });
            return stampVendor;
        }

        async Task<bool> IStampMasterService.DeleteStampVendorsById(long id)
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
        async Task<IEnumerable<StampTypeDTO>> IStampMasterService.ListAllStampTypes(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            //Get all stamp types with sort & filter parameters
            IEnumerable<StampTypeDTO> stampTypeList = await _stampTypeRepo.GetSelectedColumnByConditionAsync(entity => true, entity => new StampTypeDTO
            {
                DenominationId = entity.DenominationId,
                Denomination = entity.Denomination,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampTypeList;
        }

         async Task<IEnumerable<StampTypeDTO>> IStampMasterService.GetAllStampTypes()
        {
            IEnumerable<StampTypeDTO> stampType = await _stampTypeRepo.GetSelectedColumnAsync(entity => new StampTypeDTO { 
            
                DenominationId = entity.DenominationId,
                Denomination = entity.Denomination,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy
            });
            return stampType;
        }

         async Task<bool> IStampMasterService.CreateNewStampType(StampTypeInsertDTO stampType)
        {
            var stampTp = _mapper.Map<StampType>(stampType);
            stampTp.CreatedAt = DateTime.Now;
            stampTp.CreatedBy = _auth.GetUserId();
            _stampTypeRepo.Add(stampTp);
            _stampTypeRepo.SaveChangesManaged();
            return await Task.FromResult(true);
        }

        async Task<IEnumerable<StampTypeDTO>> IStampMasterService.GetStampTypeById(long id)
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

        async Task<bool> IStampMasterService.DeleteStampTypesById(long id)
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
            
            IEnumerable<DiscountDetailsDTO> discountDetailList = await _discountDetailRepo.GetSelectedColumnByConditionAsync(entity => true, entity => new DiscountDetailsDTO
            {
                DiscountId = entity.DiscountId,
                DenominationFrom = entity.DenominationFrom,
                DenominationTo = entity.DenominationTo,
                Discount = entity.Discount,
                VendorType = entity.VendorType,
                StampCategory = entity.StampCategory,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy  = entity.CreatedBy

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return discountDetailList;
        }

        async Task<IEnumerable<VendorTypeDTO>> IStampMasterService.GetAllStampVendorTypes()
        {
            IEnumerable<VendorTypeDTO> allVendorTypes = await _stampVendorTypeRepo.GetSelectedColumnAsync(entity => new VendorTypeDTO
            {
                StampVendorId = entity.VendorTypeId,
                VendorType = entity.VendorType,
            });
            return allVendorTypes;
        }
        async Task<bool> IStampMasterService.CreateNewStampDiscountDetails(DiscountDetailsInsertDTO stampDiscountDetails)
        {
            var stampDiscount = _mapper.Map<DiscountDetail>(stampDiscountDetails);
            stampDiscount.CreatedAt = DateTime.Now;
            stampDiscount.CreatedBy = _auth.GetUserId();
            _discountDetailRepo.Add(stampDiscount);
            _discountDetailRepo.SaveChangesManaged();
            return await Task.FromResult(true);
        }

        async Task<IEnumerable<StampCombinationDTO>> IStampMasterService.StampCombinationList(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {

        var stampCombinationEntities = await _stampCombinationRepo.GetSelectedColumnByConditionAsync(
        entity => entity.IsActive == true,
        entity => new StampCombinationDTO
        {
            StampCombinationId = entity.StampCombinationId,
            StampCategory1 = entity.StampCategory.StampCategory1,
            Description = entity.StampCategory.Description,
            Denomination = entity.StampDenomination.Denomination,
            StampDenominationId = entity.StampDenominationId,
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


        async Task<bool> IStampMasterService.DeleteStampCombinationById(long id)
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

        async Task<IEnumerable<GetAllStampCombinationDTO>> IStampMasterService.GetAllStampCombinations()
        {
            IEnumerable<GetAllStampCombinationDTO> stampCombinations = await _stampCombinationRepo.GetSelectedColumnByConditionAsync(entity => entity.IsActive == true, entity => new GetAllStampCombinationDTO
            {
                StampCombinationId = entity.StampCombinationId,
                StampCategory1 = entity.StampCategory.StampCategory1,
                Description = entity.StampCategory.Description,
                Denomination = entity.StampDenomination.Denomination,
                NoLabelPerSheet = entity.StampLabel.NoLabelPerSheet
            });
            return stampCombinations;
        }
    }
}
