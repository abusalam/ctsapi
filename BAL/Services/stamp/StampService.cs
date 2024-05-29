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
        private readonly IStampIndentRepository _stampIndentRepo;
        private readonly IStampInvoiceRepository _stampInvoiceRepo;
        private readonly IMapper _mapper;
        private readonly IClaimService _auth;

        public StampService(
            IStampIndentRepository stampIndentRepo,
            IStampInvoiceRepository stampInvoiceRepo,
            IMapper mapper,
            IClaimService claim)
        {
            _stampIndentRepo = stampIndentRepo;
            _stampInvoiceRepo = stampInvoiceRepo;
            _mapper = mapper;
            _auth = claim;
        }

        public async Task<IEnumerable<StampIndentDTO>> ListAllStampIndentsProcessed(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            //Get all stamp labels with sort & filter parameters
            Console.WriteLine(_auth);
            IEnumerable<StampIndentDTO> stampIndentList = await _stampIndentRepo.GetSelectedColumnByConditionAsync(
                // if _auth user is superintendent
                entity => entity.CreatedBy == _auth.GetUserId() && (entity.Status == (short)StampIndentStatusEnum.ApproveBySuperintendent || entity.Status == (short)StampIndentStatusEnum.ApproveByTreasuryOfficer || entity.Status == (short)StampIndentStatusEnum.RejectBySuperintendent || entity.Status == (short)StampIndentStatusEnum.RejectByTreasuryOfficer),
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
                Status = entity.Status

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampIndentList;
        }
        public async Task<IEnumerable<StampIndentDTO>> ListAllStampIndentsProcessing(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            //Get all stamp labels with sort & filter parameters
            Console.WriteLine(_auth);
            IEnumerable<StampIndentDTO> stampIndentList = await _stampIndentRepo.GetSelectedColumnByConditionAsync(
                // if _auth user is superintendent
                entity => entity.CreatedBy == _auth.GetUserId() && (entity.Status == (short)StampIndentStatusEnum.ForwardedToSuperintendent || entity.Status == (short)StampIndentStatusEnum.ForwardedToTreasuryOfficer),
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
                Status = entity.Status

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampIndentList;
        }

        public async Task<bool> CreateNewStampIndent(StampIndentInsertDTO stampIndent)
        {
                var stampIndentInput = _mapper.Map<StampIndent>(stampIndent);
                stampIndentInput.CreatedAt = DateTime.Now;
                stampIndentInput.CreatedBy = _auth.GetUserId();
                stampIndentInput.Status = stampIndentInput.RaisedToTreasuryCode == null ? (short) 10 : (short) 11;
                _stampIndentRepo.Add(stampIndentInput);
                _stampIndentRepo.SaveChangesManaged();
                return await Task.FromResult(true);
        }



        public async Task<bool> ApproveStampIndent(long stampIndentId)
        {
            var data = await _stampIndentRepo.GetSingleAysnc(e=>e.Id == stampIndentId);
            if (data != null)
            {
                if(data.Status == 10)
                {
                    data.Status = 15;
                }
                else if (data.Status == 11)
                {
                    data.Status = 12;
                }
                _stampIndentRepo.Update(data);
                _stampIndentRepo.SaveChangesManaged();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> RejectStampIndent(long stampIndentId)
        {
            var data = await _stampIndentRepo.GetSingleAysnc(e => e.Id == stampIndentId);
            if (data != null)
            {
                if (data.Status == 10)
                {
                    data.Status = 16;
                }
                if (data.Status == 11)
                {
                    data.Status = 13;
                }
                _stampIndentRepo.Update(data);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(true);
        }



        // stamp invoice

        public async Task<IEnumerable<StampInvoiceDTO>> ListAllStampInvoices(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            //Get all stamp labels with sort & filter parameters
            Console.WriteLine(_auth);
            IEnumerable<StampInvoiceDTO> stampInvoiceList = await _stampInvoiceRepo.GetSelectedColumnByConditionAsync(
                // if _auth user is superintenvoice
                entity => entity.CreatedBy == _auth.GetUserId(),
            entity => new StampInvoiceDTO
            {
                StampIndentId = entity.StampIndentId,
                MemoNumber = entity.StampIndent.MemoNumber,
                MemoDate = entity.StampIndent.MemoDate,
                Remarks = entity.StampIndent.Remarks,
                RaisedToTreasuryCode = entity.StampIndent.RaisedToTreasuryCode,
                StmapCategory = entity.StampIndent.StampCombination.StampCategory.StampCategory1,
                Description = entity.StampIndent.StampCombination.StampCategory.Description,
                Denomination = entity.StampIndent.StampCombination.StampDenomination.Denomination,
                LabelPerSheet = entity.StampIndent.StampCombination.StampLabel.NoLabelPerSheet,
                IndentedSheet = entity.StampIndent.Sheet,
                IndentedLabel = entity.StampIndent.Label,
                Sheet = entity.Sheet,
                Label = entity.Label,
                Quantity = entity.Quantity,
                Amount = entity.Amount,
                Status = entity.StampIndent.Status,
                StampInvoiceId = entity.StampInvoiceId,
                InvoiceNumber = entity.InvoiceNumber,
                InvoiceDate = entity.InvoiceDate,
                CreatedBy = entity.CreatedBy,
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
    }
}