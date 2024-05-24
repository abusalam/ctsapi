using AutoMapper;
using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DTOs;
using CTS_BE.Helper.Authentication;

namespace CTS_BE.BAL.Services.stamp
{
    public class StampService : IStampService
    {
        private readonly IStampIndentRepository _stampIndentRepo;
        //private readonly IMapper _mapper;
        //private readonly IClaimService _auth;

        public StampService(
            IStampIndentRepository stampIndentRepo,
            IMapper mapper,
            IClaimService claim)
        {
            _stampIndentRepo = stampIndentRepo;
            //_mapper = mapper;
            //_auth = claim;
        }

        public async Task<IEnumerable<StampIndentDTO>> ListAllStampIndents(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            //Get all stamp labels with sort & filter parameters
            IEnumerable<StampIndentDTO> stampIndentList = await _stampIndentRepo.GetSelectedColumnByConditionAsync(entity => true, entity => new StampIndentDTO
            {
                StampIndentId = entity.Id,
                MemoNumber = entity.MemoNumber,
                MemoDate = entity.MemoDate,
                Remarks = entity.Remarks,
                RaisedByTreasury = entity.RaisedByTreasuryNavigation.Code,
                RaisedToTreasury = entity.RaisedToTreasury !=null ? entity.RaisedToTreasuryNavigation.Code : "SUPERINTENDENT",
                StmapCategory = entity.StampCombination.StampCategory.StampCategory1,
                Description = entity.StampCombination.StampCategory.Description,
                Denomination = entity.StampCombination.StampType.Denomination,
                LabelPerSheet = entity.StampCombination.StampLabel.NoLabelPerSheet,
                Sheet = entity.Sheet,
                Label = entity.Label,
                Quantity = entity.Quantity,
                Amount = entity.Amount,
                CreatedAt = entity.CreatedAt,
                Status = entity.Status

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampIndentList;
        }
    }
}