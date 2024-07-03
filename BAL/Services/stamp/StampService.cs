using AutoMapper;
using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DTOs;
using CTS_BE.Enum;
using CTS_BE.Helper.Authentication;

namespace CTS_BE.BAL.Services.stamp
{
    public class StampService : IStampService
    {
        private readonly IStampWalletRepository _stampWalletRepo;
        private readonly IStampIndentRepository _stampIndentRepo;
        private readonly IStampInvoiceRepository _stampInvoiceRepo;
        private readonly IMapper _mapper;
        private readonly IClaimService _auth;

        public StampService(
            IStampWalletRepository stampWalletRepo,
            IStampIndentRepository stampIndentRepo,
            IStampInvoiceRepository stampInvoiceRepo,
            IMapper mapper,
            IClaimService claim)
        {
            _stampWalletRepo = stampWalletRepo;
            _stampIndentRepo = stampIndentRepo;
            _stampInvoiceRepo = stampInvoiceRepo;
            _mapper = mapper;
            _auth = claim;
        }
        
        // ======================Stamp Indents===========================
        public async Task<IEnumerable<StampIndentDTO>> ListAllStampIndentsProcessed(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampIndentDTO> stampIndentList = await _stampIndentRepo.GetSelectedColumnByConditionAsync(
                // if _auth user is superintendent
                entity => entity.CreatedBy == _auth.GetUserId() && (entity.StatusId == (short)StampIndentStatusEnum.ApproveBySuperintendent || entity.StatusId == (short)StampIndentStatusEnum.ApproveByTreasuryOfficer ),
            entity => new StampIndentDTO
            {
                StampIndentId = entity.Id,
                MemoNumber = entity.MemoNumber,
                MemoDate = entity.MemoDate,
                Remarks = entity.Remarks,
                RaisedToTreasuryCode = entity.RaisedToTreasuryCode != null ? entity.RaisedToTreasuryCode : "SUPERINTENDENT",
                StmapCategory = entity.StampCombination.StampCategory.StampCategory1,
                Description = entity.StampCombination.StampCategory.Description,
                Denomination = entity.StampCombination.StampType.Denomination,
                LabelPerSheet = entity.StampCombination.StampLabel.NoLabelPerSheet,
                Sheet = entity.Sheet,
                Label = entity.Label,
                Quantity = entity.Quantity,
                Amount = entity.Amount,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                Status = entity.Status.Name

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampIndentList;
        }

        public async Task<IEnumerable<StampIndentDTO>> StampIndentList(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampIndentDTO> stampIndentList = await _stampIndentRepo.GetSelectedColumnByConditionAsync(
                // if _auth user is superintendent
                entity => entity.RaisedByTreasuryCode == _auth.GetScope(),
            entity => new StampIndentDTO
            {
                StampIndentId = entity.Id,
                MemoNumber = entity.MemoNumber,
                MemoDate = entity.MemoDate,
                Remarks = entity.Remarks,
                RaisedToTreasuryCode = entity.RaisedToTreasuryCode != null ? entity.RaisedToTreasuryCode : "SUPERINTENDENT",
                StmapCategory = entity.StampCombination.StampCategory.StampCategory1,
                Description = entity.StampCombination.StampCategory.Description,
                Denomination = entity.StampCombination.StampType.Denomination,
                LabelPerSheet = entity.StampCombination.StampLabel.NoLabelPerSheet,
                Sheet = entity.Sheet,
                Label = entity.Label,
                Quantity = entity.Quantity,
                Amount = entity.Amount,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                Status = entity.Status.Name

            }, 
            pageIndex, 
            pageSize, 
            filters, 
            (sortParameters != null) ? sortParameters.Field : null, 
            (sortParameters != null) ? sortParameters.Order : null);
            return stampIndentList;
        }

        public async Task<IEnumerable<StampIndentDTO>> ListAllStampIndentsProcessing(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampIndentDTO> stampIndentList = await _stampIndentRepo.GetSelectedColumnByConditionAsync(
                // if _auth user is superintendent
                entity => entity.RaisedToTreasuryCode == _auth.GetScope() && (entity.StatusId == (short)StampIndentStatusEnum.ForwardedToSuperintendent || entity.StatusId == (short)StampIndentStatusEnum.ForwardedToTreasuryOfficer),
            entity => new StampIndentDTO
            {
                StampIndentId = entity.Id,
                MemoNumber = entity.MemoNumber,
                MemoDate = entity.MemoDate,
                Remarks = entity.Remarks,
                RaisedToTreasuryCode = entity.RaisedToTreasuryCode != null ? entity.RaisedToTreasuryCode : "SUPERINTENDENT",
                StmapCategory = entity.StampCombination.StampCategory.StampCategory1,
                Description = entity.StampCombination.StampCategory.Description,
                Denomination = entity.StampCombination.StampType.Denomination,
                LabelPerSheet = entity.StampCombination.StampLabel.NoLabelPerSheet,
                Sheet = entity.Sheet,
                Label = entity.Label,
                Quantity = entity.Quantity,
                Amount = entity.Amount,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                Status = entity.Status.Name

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampIndentList;
        }

        public async Task<bool> CreateNewStampIndent(StampIndentInsertDTO stampIndent)
        {
                var stampIndentInput = _mapper.Map<StampIndent>(stampIndent);
                stampIndentInput.CreatedAt = DateTime.Now;
                stampIndentInput.CreatedBy = _auth.GetUserId();
                stampIndentInput.RaisedByTreasuryCode = _auth.GetScope();
                stampIndentInput.StatusId = stampIndentInput.RaisedToTreasuryCode == null ? (int) 23 : (int) 26;
                _stampIndentRepo.Add(stampIndentInput);
                _stampIndentRepo.SaveChangesManaged();
                return await Task.FromResult(true);
        }

        public async Task<bool> ApproveStampIndent(long stampIndentId)
        {
            var data = await _stampIndentRepo.GetSingleAysnc(e=>e.Id == stampIndentId);
            if (data != null)
            {
                if (await _stampIndentRepo.IndentApprove(data.RaisedToTreasuryCode, data.Quantity))
                {    
                    if(data.StatusId == 23)
                    {
                        data.StatusId = 24;
                    }
                    else if (data.StatusId == 26)
                    {
                        data.StatusId = 27;
                    }
                    _stampIndentRepo.Update(data);
                    _stampIndentRepo.SaveChangesManaged();
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> RejectStampIndent(long stampIndentId)
        {
            var data = await _stampIndentRepo.GetSingleAysnc(e => e.Id == stampIndentId);
            if (data != null)
            {
                if (data.StatusId == 23)
                {
                    data.StatusId = 25;
                }
                else if (data.StatusId == 26)
                {
                    data.StatusId = 28;
                }
                _stampIndentRepo.Update(data);
                _stampIndentRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }


        // =========================Stamp Invoice==========================
        public async Task<IEnumerable<StampInvoiceDTO>> ListAllStampInvoices(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampInvoiceDTO> stampInvoiceList = await _stampInvoiceRepo.GetSelectedColumnByConditionAsync(
                // if _auth user is superintenvoice
            entity => entity.CreatedBy == _auth.GetUserId() && (entity.StampIndent.StatusId == (short)StampIndentStatusEnum.ApproveBySuperintendent || entity.StampIndent.StatusId == (short)StampIndentStatusEnum.ApproveByTreasuryOfficer ),
            entity => new StampInvoiceDTO
            {
                StampIndentId = entity.StampIndentId,
                MemoNumber = entity.StampIndent.MemoNumber,
                MemoDate = entity.StampIndent.MemoDate,
                //Remarks = entity.StampIndent.Remarks,
                StampInvoiceId = entity.StampInvoiceId,
                InvoiceNumber = entity.InvoiceNumber,
                InvoiceDate = entity.InvoiceDate,
                RaisedToTreasuryCode = entity.StampIndent.RaisedToTreasuryCode,
                StmapCategory = entity.StampIndent.StampCombination.StampCategory.StampCategory1,
                Description = entity.StampIndent.StampCombination.StampCategory.Description,
                Denomination = entity.StampIndent.StampCombination.StampType.Denomination,
                //LabelPerSheet = entity.StampIndent.StampCombination.StampLabel.NoLabelPerSheet,
                //IndentedSheet = entity.StampIndent.Sheet,
                //IndentedLabel = entity.StampIndent.Label,
                Sheet = entity.Sheet,
                Label = entity.Label,
                Quantity = entity.Quantity,
                Amount = entity.Amount,
                Status = entity.StampIndent.Status.Name
                //CreatedBy = entity.CreatedBy,   // todo raised by treasury code
            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampInvoiceList;
        }

        public async Task<bool> CreateNewStampInvoice(StampInvoiceInsertDTO stampInvoice)
        {
            var stampInvoiceInput = _mapper.Map<StampInvoice>(stampInvoice);
            stampInvoiceInput.CreatedBy = _auth.GetUserId();
            stampInvoiceInput.CreatedAt = DateTime.Now;
            _stampInvoiceRepo.Add(stampInvoiceInput);
            bool res = await this.ApproveStampIndent(stampInvoice.StampIndentId);
            if (res)
            {
                _stampInvoiceRepo.SaveChangesManaged();
                return await Task.FromResult(true); 
            }
            _stampInvoiceRepo.SaveChangesManaged();
            return await Task.FromResult(false);
        }

        public async Task<StampIndentDTO> GetStampIndentById(long IndentId)
        {
            var stampIndent = await _stampIndentRepo.GetSingleSelectedColumnByConditionAsync(
                e=>e.Id == IndentId,
                e=>new StampIndentDTO
                    {
                            StampIndentId = e.Id,
                            MemoNumber = e.MemoNumber,
                            MemoDate = e.MemoDate,
                            Remarks = e.Remarks,
                            RaisedByTreasuryCode = e.RaisedByTreasuryCode,
                            RaisedToTreasuryCode = e.RaisedToTreasuryCode,
                            StmapCategory = e.StampCombination.StampCategory.StampCategory1,
                            Description = e.StampCombination.StampCategory.Description,
                            Denomination = e.StampCombination.StampType.Denomination,
                            LabelPerSheet = e.StampCombination.StampLabel.NoLabelPerSheet,
                            Sheet = e.Sheet,
                            Label = e.Label,
                            Quantity = e.Quantity,
                            Amount = e.Amount,
                            CreatedAt = e.CreatedAt,
                            Status = e.Status.Name

                }
                );
            return _mapper.Map<StampIndentDTO>(stampIndent);

        }

        public async Task<bool> ReceiveStampIndent(long stampIndentId)
        {
            var data = await _stampIndentRepo.GetSingleAysnc(e => e.Id == stampIndentId);
            if (data != null)
            {
                if (await _stampIndentRepo.IndentRecieve(data.RaisedToTreasuryCode, data.RaisedByTreasuryCode, data.Quantity, data.Id))
                {
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

    }
}