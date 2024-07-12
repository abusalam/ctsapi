using AutoMapper;
using CTS_BE.BAL.Interfaces.stampRequisition;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stampRequisition;
using CTS_BE.DTOs;
using CTS_BE.Helper.Authentication;
using System.Numerics;

namespace CTS_BE.BAL.Services.stampRequisition
{
    public class StampRequisitionService : IStampRequisitionService
    {
        private readonly IStampRequisitionRepository _stampRequisitionRepo;
        private readonly IStampRequisitionApproveRepository _stampRequisitionApproveRepo;
        private readonly IMapper _mapper;
        private readonly IClaimService _auth;

        public StampRequisitionService(
            IStampRequisitionRepository stampRequisitionRepo,
            IStampRequisitionApproveRepository stampRequisitionApproveRepo,
            IMapper mapper,
            IClaimService claim)
        {
            _stampRequisitionRepo = stampRequisitionRepo;
            _stampRequisitionApproveRepo = stampRequisitionApproveRepo;
            _mapper = mapper;
            _auth = claim;
        }
        
        // ======================Stamp Requisitions===========================
        public async Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitions(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionRepo.GetSelectedColumnByConditionAsync(
                entity => true,
            entity => new StampRequisitionDTO
            {
                VendorStampRequisitionId = entity.VendorStampRequisitionId,
                VendorId = entity.VendorId,
                VendorType = entity.Vendor.VendorTypeNavigation.VendorType,
                LicenseNo = entity.Vendor.LicenseNo,
                Quantity = ((short)(entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label)),
                Amount = (entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label) * entity.Combination.StampType.Denomination,
                Status = entity.Status.Name,
                RequisitionDate = entity.RequisitionDate,
                VendorName = entity.Vendor.VendorName,
                RaisedToTreasury = entity.RaisedToTreasury,
                RequisitionNo = entity.RequisitionNo,
                Sheet = entity.Sheet,
                Label = entity.Label,
            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null) ;
            return stampRequisitionList;
        }

        public async Task<bool> CreateStampRequisition(StampRequisitionInsertDTO stampRequisition)
        {
                var stampRequisitionInput = _mapper.Map<VendorStampRequisition>(stampRequisition);
                stampRequisitionInput.CreatedAt = DateTime.Now;
                stampRequisitionInput.CreatedBy = _auth.GetUserId();
                stampRequisitionInput.StatusId = (int)30;
                _stampRequisitionRepo.Add(stampRequisitionInput);
                _stampRequisitionRepo.SaveChangesManaged();
                return await Task.FromResult(true);
        }

        public async Task<bool> RequisitionApprovedByStampClerk(StampRequisitionApprovedByClerkDTO stampRequisition)
        {
            if(stampRequisition != null && await _stampRequisitionRepo.ApproveByStampClerk(stampRequisition.VendorStampRequisitionId, stampRequisition.SheetByClerk, stampRequisition.LabelByClerk))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> RequisitionRejectedByStampClerk(long stampRequisitionId)
        {
                var data = await _stampRequisitionRepo.GetSingleAysnc(e => e.VendorStampRequisitionId == stampRequisitionId);
                if(data != null)
                {
                    data.StatusId = 32;
                    data.UpdatedBy = _auth.GetUserId();
                    data.UpdatedAt = DateTime.Now;
                    _stampRequisitionRepo.Update(data);
                    _stampRequisitionRepo.SaveChangesManaged();
                    return true;
                }
                return false;
        }

        public async Task<bool> RequisitionApprovedByTO(StampRequisitionApprovedByTODTO stampRequisition)
        {
            if (stampRequisition != null && await _stampRequisitionRepo.ApproveByTO(stampRequisition))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> RequisitionRejectedByTO(long stampRequisitionStagingId)
        {
            var data = await _stampRequisitionRepo.GetSingleAysnc(e => e.VendorRequisitionStagingId == stampRequisitionStagingId);
            if (data != null)
            {
                data.StatusId = (int)Enum.StampRequisitionStatusEnum.RejectedByTreasuryOfficer;
                data.UpdatedBy = _auth.GetUserId();
                data.UpdatedAt = DateTime.Now;
                _stampRequisitionRepo.Update(data);
                _stampRequisitionRepo.SaveChangesManaged();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsForClerk(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionRepo.GetSelectedColumnByConditionAsync(
                entity => entity.StatusId == (int) Enum.StampRequisitionStatusEnum.ForwardedToStampCleck,
            entity => new StampRequisitionDTO
            {
                VendorStampRequisitionId = entity.VendorStampRequisitionId,
                VendorId = entity.VendorId,
                VendorType = entity.Vendor.VendorTypeNavigation.VendorType,
                LicenseNo = entity.Vendor.LicenseNo,
                Quantity = ((short)(entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label)),
                Amount = (entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label) * entity.Combination.StampType.Denomination,
                Status = entity.Status.Name,
                RequisitionDate = entity.RequisitionDate,
                VendorName = entity.Vendor.VendorName,
                RaisedToTreasury = entity.RaisedToTreasury,
                RequisitionNo = entity.RequisitionNo,
                Sheet = entity.Sheet,
                Label = entity.Label,
            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampRequisitionList;
        }

        public async Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsForTO(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionRepo.GetSelectedColumnByConditionAsync(
                entity => entity.StatusId == (int)Enum.StampRequisitionStatusEnum.ForwardedToTreasuryOfficer || entity.StatusId == (int)Enum.StampRequisitionStatusEnum.WaitingForPaymentVerification,
            entity => new StampRequisitionDTO
            {
                VendorStampRequisitionId = entity.VendorStampRequisitionId,
                VendorId = entity.VendorId,
                VendorType = entity.Vendor.VendorTypeNavigation.VendorType,
                LicenseNo = entity.Vendor.LicenseNo,
                Quantity = ((short)(entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label)),
                Amount = (entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label) * entity.Combination.StampType.Denomination,
                Status = entity.Status.Name,
                RequisitionDate = entity.RequisitionDate,
                VendorName = entity.Vendor.VendorName,
                RaisedToTreasury = entity.RaisedToTreasury,
                RequisitionNo = entity.RequisitionNo,
                Sheet = entity.Sheet,
                Label = entity.Label,
            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampRequisitionList;
        }

        public async Task<bool> DeliveredByDEO(long stampRequisitionId)
        {
            // store procedure to update the clear balance //todo
            var data = await _stampRequisitionRepo.GetSingleAysnc(e => e.VendorStampRequisitionId == stampRequisitionId);
            if (data != null)
            {
                data.StatusId = (int) Enum.StampRequisitionStatusEnum.DeliveredToVendor;
                data.UpdatedBy = _auth.GetUserId();
                data.UpdatedAt = DateTime.Now;
                _stampRequisitionRepo.Update(data);
                _stampRequisitionRepo.SaveChangesManaged();
                return true;
            }
            return false;
        }

        public async Task<bool> PaymentRegisterByDEO(StampRequisitionPaymentDTO stampRequisition)
        {
            if (stampRequisition != null)
            {

                var data = new VendorRequisitionApprove
                {
                    VendorRequisitionId = stampRequisition.VendorStampRequisitionId,
                    TransactionId = stampRequisition.GRNNo,
                    ApproveBy = _auth.GetUserId(),
                };
                data = _mapper.Map<VendorRequisitionApprove>(data);
                _stampRequisitionApproveRepo.Add(data);
                _stampRequisitionApproveRepo.SaveChangesManaged();
                long approveId = data.VendorRequisitionApproveId;
                var stampRequisitionData = await _stampRequisitionRepo.GetSingleAysnc(e => e.VendorStampRequisitionId == stampRequisition.VendorStampRequisitionId);
                stampRequisitionData.VendorRequisitionApproveId = approveId;
                stampRequisitionData.StatusId = (int) Enum.StampRequisitionStatusEnum.WaitingForPaymentVerification;
                _stampRequisitionRepo.Update(stampRequisitionData);
                _stampRequisitionRepo.SaveChangesManaged();
                return true;
            }
            return false;
        }


        public async Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsWaitingForPaymentVerification(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionRepo.GetSelectedColumnByConditionAsync(
                entity => entity.StatusId == (int)Enum.StampRequisitionStatusEnum.WaitingForPaymentVerification,
            entity => new StampRequisitionDTO
            {
                VendorStampRequisitionId = entity.VendorStampRequisitionId,
                VendorId = entity.VendorId,
                VendorType = entity.Vendor.VendorTypeNavigation.VendorType,
                LicenseNo = entity.Vendor.LicenseNo,
                Quantity = ((short)(entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label)),
                Amount = (entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label) * entity.Combination.StampType.Denomination,
                Status = entity.Status.Name,
                RequisitionDate = entity.RequisitionDate,
                VendorName = entity.Vendor.VendorName,
                RaisedToTreasury = entity.RaisedToTreasury,
                RequisitionNo = entity.RequisitionNo,
                Sheet = entity.Sheet,
                Label = entity.Label,
            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampRequisitionList;
        } 
        
        public async Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsWaitingForDelivery(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionRepo.GetSelectedColumnByConditionAsync(
                entity => entity.StatusId == (int)Enum.StampRequisitionStatusEnum.WaitingForDelivery,
            entity => new StampRequisitionDTO
            {
                VendorStampRequisitionId = entity.VendorStampRequisitionId,
                VendorId = entity.VendorId,
                VendorType = entity.Vendor.VendorTypeNavigation.VendorType,
                LicenseNo = entity.Vendor.LicenseNo,
                Quantity = ((short)(entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label)),
                Amount = (entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label) * entity.Combination.StampType.Denomination,
                Status = entity.Status.Name,
                RequisitionDate = entity.RequisitionDate,
                VendorName = entity.Vendor.VendorName,
                RaisedToTreasury = entity.RaisedToTreasury,
                RequisitionNo = entity.RequisitionNo,
                Sheet = entity.Sheet,
                Label = entity.Label,
            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampRequisitionList;
        }
    }
}