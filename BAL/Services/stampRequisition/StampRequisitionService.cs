using AutoMapper;
using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.BAL.Interfaces.stampRequisition;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stampRequisition;
using CTS_BE.DTOs;
using CTS_BE.Helper.Authentication;

namespace CTS_BE.BAL.Services.stampRequisition
{
    public class StampRequisitionService : IStampRequisitionService
    {
        private readonly IStampRequisitionRepository _stampRequisitionRepo;
        private readonly IStampRequisitionApproveRepository _stampRequisitionApproveRepo;
        private readonly IStampRequisitionChallanGenerateRepository _stampRequisitionChallanGenerateRepo;
        private readonly IStampMasterService _stampMasterService;
        private readonly IMapper _mapper;
        private readonly IClaimService _auth;

        public StampRequisitionService(
            IStampRequisitionRepository stampRequisitionRepo,
            IStampRequisitionApproveRepository stampRequisitionApproveRepo,
            IStampMasterService stampMasterService,
            IStampRequisitionChallanGenerateRepository stampRequisitionChallanGenerateRepo,

            IMapper mapper,
            IClaimService claim)
        {
            _stampRequisitionRepo = stampRequisitionRepo;
            _stampRequisitionApproveRepo = stampRequisitionApproveRepo;
            _stampMasterService = stampMasterService;
            _stampRequisitionChallanGenerateRepo = stampRequisitionChallanGenerateRepo;
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
                    data.StatusId = (int) Enum.StampRequisitionStatusEnum.RejectedByStampClerk;
                    data.UpdatedBy = _auth.GetUserId();
                    data.UpdatedAt = DateTime.Now;
                    _stampRequisitionRepo.Update(data);
                    _stampRequisitionRepo.SaveChangesManaged();
                    return true;
                }
                return false;
        }

        public async Task<bool> RequisitionApprovedByTO(StampRequisitionApprovedByTODataDTO stampRequisition)
        {
            //StampRequisitionApprovedByTODataDTO
            var srd = await _stampRequisitionRepo.GetSingleSelectedColumnByConditionAsync(
                    e => e.VendorStampRequisitionId == stampRequisition.vendorStampRequisitionId,
                    e => new HoaDataDTO
                    {
                        Head = e.Combination.StampCategory.Hoa,
                        HoaId = e.Combination.StampCategory.HoaId,
                        RequisitionNo = e.RequisitionNo,
                        VendorRequisitionStagingId = e.VendorRequisitionStagingId
                    }
                );
            if (srd != null)
            {
                var data = new StampRequisitionApprovedByTODTO
                {
                    VendorRequisitionStagingId = srd.VendorRequisitionStagingId,
                    ChallanAmount = stampRequisition.ChallanAmount,
                    DiscountedAmount = stampRequisition.DiscountedAmount,
                    Head = srd.Head,
                    HoaId = srd.HoaId,
                    LabelByTo = stampRequisition.LabelByTo,
                    RequisitionNo = srd.RequisitionNo,
                    SheetByTo = stampRequisition.SheetByTo,
                    TaxAmount = stampRequisition.TaxAmount,
                };
                if (stampRequisition != null && await _stampRequisitionRepo.ApproveByTO(data))
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> RequisitionRejectedByTO(long requisitionId)
        {
            var data = await _stampRequisitionRepo.GetSingleAysnc(e => e.VendorStampRequisitionId == requisitionId);
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

        public async Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsWaitingForApprovalByTO(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionRepo.GetSelectedColumnByConditionAsync(
                entity => entity.StatusId == (int)Enum.StampRequisitionStatusEnum.ForwardedToTreasuryOfficer,
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
                Sheet = entity.VendorRequisitionStaging.SheetByClerk,
                Label = entity.VendorRequisitionStaging.LabelByClerk,
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
                stampRequisitionData.StatusId = (int) Enum.StampRequisitionStatusEnum.WaitingForDelivery;
                _stampRequisitionRepo.Update(stampRequisitionData);
                _stampRequisitionRepo.SaveChangesManaged();

                var sr = await _stampRequisitionChallanGenerateRepo.GetSingleAysnc(e => e.VendorRequisitionStagingId == stampRequisitionData.VendorRequisitionStagingId);
                sr.GrnNo = stampRequisition.GRNNo.ToString();
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
                CombinationId = entity.CombinationId
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

        public async Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitionsWaitingForPayment(List<FilterParameter> filters, int pageIndex, int pageSize, SortParameter sortParameters)
        {
            IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionRepo.GetSelectedColumnByConditionAsync(
                entity => entity.StatusId == (int)Enum.StampRequisitionStatusEnum.WaitingForPayment,
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
        public async Task<TRFormDataDTO> TrFromGenerationData(long stampRequisitionId)
        {
            var data = await _stampRequisitionChallanGenerateRepo.GetSingleAysnc(e => e.VendorRequisitionStaging.VendorRequisitionId == stampRequisitionId);
            TRDataDTO TrDataGather = await _stampRequisitionRepo.GetSingleSelectedColumnByConditionAsync(
                e => e.VendorStampRequisitionId == stampRequisitionId,
                e => new TRDataDTO
                {
                    VendorName = e.Vendor.VendorName,
                    VendorAddress = e.Vendor.Address,
                    TreasuryName = e.RaisedToTreasuryNavigation.Name
                });
                if (data == null)
               {
                    return new TRFormDataDTO() ;
               }
            string hoa_code = data.Hoa;
            string[] parts = hoa_code.Split('-');
            var res = new TRFormDataDTO();
            res.Amount = data.TotalAmount;
            res.DetailHead = parts[6];
            res.Hoa = data.Hoa;
            res.VendorName = TrDataGather.VendorName;
            res.VendorAddress = TrDataGather.VendorAddress;
            res.RaisedToTreasury = _auth.GetScope();
            res.TreasuryName = TrDataGather.TreasuryName;
            return res;
        }

        public async Task<CalculationDTO> CalculateAllDetails(DataForCalculationDTO srd)
        {
            if (srd != null)
            {
                var data = await _stampRequisitionRepo.GetSingleSelectedColumnByConditionAsync(
                       e => e.VendorStampRequisitionId == srd.VendorStampRequisitionId,
                       e => new
                       {
                           SheetCount = e.Combination.StampLabel.NoLabelPerSheet,
                           Denomination = e.Combination.StampType.Denomination,
                           VendorType = e.Vendor.VendorType,
                           CategoryId = e.Combination.StampCategoryId
                       }
                    );
                var quantity = (decimal)(srd.Sheet * data.SheetCount) + srd.Label;
                var discountCheck = await _stampMasterService.GetDiscount(data.VendorType, data.CategoryId, quantity);
                return new CalculationDTO
                {
                    Amount = quantity * data.Denomination,
                    DiscountAmount = discountCheck,
                    TaxAmount = discountCheck * (decimal) 0.1,
                    ChallanAmount = (quantity * data.Denomination) - discountCheck + (discountCheck * (decimal)0.1),
                };
            }
            return new CalculationDTO();
        }
    }
}