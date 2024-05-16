using AutoMapper;
using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DTOs;
using CTS_BE.Helper.Authentication;
using CTS_BE.Model.e_Kuber;
using System.Collections.Generic;

namespace CTS_BE.BAL.Services.stamp
{
    public class StampMasterService: IStampMasterService
    {
        private readonly IStampLabelRepository _stampLabelRepo;
        private readonly IStampCategoryRepository _stampCategoryRepo;
        private readonly IStampVendorRepository _stampVendorRepo;
        private readonly IStampTypeRepository _stampTypeRepo;
        private readonly IMapper _mapper;
        private readonly IClaimService _auth;

        public StampMasterService(IStampLabelRepository stampLabelMasterRepo, IStampCategoryRepository stampCategoryRepo, IStampVendorRepository stampVendorRepo, IStampTypeRepository stampTypeRepo, IMapper mapper, IClaimService claim)
        {
            _stampLabelRepo = stampLabelMasterRepo;
            _stampCategoryRepo = stampCategoryRepo;
            _stampVendorRepo = stampVendorRepo;
            _stampTypeRepo = stampTypeRepo;
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

        async Task<IEnumerable<StampLabelMasterDTO>> IStampMasterService.GetAllStampLabels()
        {
            IEnumerable<StampLabelMasterDTO> stampLabelMasters = await _stampLabelRepo.GetSelectedColumnAsync(entity=>new StampLabelMasterDTO
            {
                LabelId = entity.LabelId,
                IsActive = entity.IsActive,
                NoLabelPerSheet = entity.NoLabelPerSheet,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy,

            });
            return stampLabelMasters;
        }

        async Task<IEnumerable<StampLabelMasterDTO>> IStampMasterService.GetStampLabelById(long id)
        {
            var stampLabelMasters = await _stampLabelRepo.GetAllByConditionAsync(a=>a.LabelId == id);
            return _mapper.Map<List<StampLabelMasterDTO>>(stampLabelMasters);
        }
        async Task<bool> IStampMasterService.DeleteStampLabelsById(long id)
        {
            var stampLabelMasters = await _stampLabelRepo.GetAllByConditionAsync(a=>a.LabelId == id);
            if (stampLabelMasters.Count() > 0)
            {
                foreach (var item in stampLabelMasters)
                {
                    _stampLabelRepo.Delete(item);

                }
                _stampLabelRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);

        }


        // Stamp Category Services

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
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy
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
            var stampCategory = await _stampCategoryRepo.GetAllByConditionAsync(a => a.StampCategoryId == id);
            return _mapper.Map<List<StampCategoryDTO>>(stampCategory);
        }

        async Task<bool> IStampMasterService.DeleteStampCategorysById(long id)
        {
            var stampCategory = await _stampCategoryRepo.GetAllByConditionAsync(a => a.StampCategoryId == id);
            if (stampCategory.Count() > 0)
            {
                foreach (var item in stampCategory)
                {
                    _stampCategoryRepo.Delete(item);

                }
                _stampCategoryRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        // Stamp Vendor Services

        async Task<IEnumerable<StampVendorDTO>> IStampMasterService.GetAllStampVendors()
        {
            IEnumerable<StampVendorDTO> stampVendor = await _stampVendorRepo.GetSelectedColumnAsync(entity => new StampVendorDTO {
               
                LicenseNo = entity.LicenseNo,
                //EffectiveFrom = entity.EffectiveFrom.ToString,
                PanNumber = entity.PanNumber,
                PhoneNumber = entity.PhoneNumber,
                StampVendorId = entity.StampVendorId,
                Address = entity.Address,
                ActiveAtGrips = entity.ActiveAtGrips,
                //ValidUpto = entity.ValidUpto,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy
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
            var stampVendor = await _stampVendorRepo.GetAllByConditionAsync(a => a.StampVendorId == id);
            return _mapper.Map<List<StampVendorDTO>>(stampVendor);
        }

        async Task<bool> IStampMasterService.DeleteStampVendorsById(long id)
        {
            var stampVendor = await _stampVendorRepo.GetAllByConditionAsync(a => a.StampVendorId == id);
            if (stampVendor.Count() > 0)
            {
                foreach (var item in stampVendor)
                {
                    _stampVendorRepo.Delete(item);

                }
                _stampVendorRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
        
        // Stamp Type Services

        async Task<IEnumerable<StampTypeDTO>> IStampMasterService.GetAllStampTypes()
        {
            IEnumerable<StampTypeDTO> stampType = await _stampTypeRepo.GetSelectedColumnAsync(entity => new StampTypeDTO { 
            
                DenominationId = entity.DenominationId,
                Denomination = entity.Denomination,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy,
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
            var stampType = await _stampTypeRepo.GetAllByConditionAsync(a => a.DenominationId == id);
            return _mapper.Map<List<StampTypeDTO>>(stampType);
        }

        async Task<bool> IStampMasterService.DeleteStampTypesById(long id)
        {
            var stampType = await _stampTypeRepo.GetAllByConditionAsync(a => a.DenominationId == id);
            if (stampType.Count() > 0)
            {
                foreach (var item in stampType)
                {
                    _stampTypeRepo.Delete(item);

                }
                _stampTypeRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
