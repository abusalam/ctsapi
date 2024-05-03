using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Model.Cheque;

namespace CTS_BE.BAL
{
    public class ChequeEntryService : IChequeEntryService
    {
        private readonly IChequeEntryRepository _ChequeEntryRepository;
        private readonly IMapper _mapper;
        public ChequeEntryService(IChequeEntryRepository ChequeEntryRepository, IMapper mapper)
        {
            _ChequeEntryRepository = ChequeEntryRepository;
            _mapper = mapper;
        }
        public async Task<bool> Insert(ChequeEntryModel chequeEntryModel)
        {
            string chequeEntryData = JSONHelper.ObjectToJson(chequeEntryModel);
            if(await _ChequeEntryRepository.NewChequeEntries(chequeEntryData)){
                return true;
            }   
            return false;
        }
        public async Task<IEnumerable<ChequeListDTO>> List(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<ChequeListDTO> chequeList = (IEnumerable<ChequeListDTO>)await _ChequeEntryRepository.GetSelectedColumnByConditionAsync(entity => true, entity => new ChequeListDTO
            {
                TreasurieCode = entity.TreasurieCode,
                MicrCode = entity.MicrCode,
                Series = entity.SeriesNo,
                Start = entity.Start,
                End = entity.End,
                Quantity = entity.Quantity

            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return chequeList;
        }
        public async Task<IEnumerable<DropdownDTO>> AllSeries()
        {
            return await _ChequeEntryRepository.GetSelectedColumnByConditionAsync(entity => !entity.IsUsed, entity => new DropdownDTO
            {
                Name = entity.SeriesNo+" - "+entity.MicrCode+" - "+CommonHelper.calculateAvailableQuantity(entity.CurrentPosition,entity.End,entity.Start).ToString(),
                Code = entity.Id
            });
        }
        // public async Task<IEnumerable<DropdownDTO>> AllSeries()
        // {
        //     return await _ChequeEntryRepository.GetSelectedColumnByConditionAsync(entity => !entity.IsUsed, entity => new DropdownDTO
        //     {
        //         Name = entity.SeriesNo+" - "+entity.MicrCode+" - "+CommonHelper.calculateAvailableQuantity(entity.CurrentPosition,entity.End,entity.Start).ToString(),
        //         Code = entity.Id
        //     });
        // }
        public async Task<ChequeSeriesDetailDTO> SeriesDetailsById(long id)
        {
            return await _ChequeEntryRepository.GetSingleSelectedColumnByConditionAsync(entity => entity.Id == id, entity => new ChequeSeriesDetailDTO
            {
                Id = entity.Id,
                Series = entity.SeriesNo,
                Start = entity.Start,
                End = entity.End,
                Quantity = entity.Quantity,
                AvailableQuantity = CommonHelper.calculateAvailableQuantity(entity.CurrentPosition,entity.End,entity.Start)
            });
        }
        public async Task<List<ChequeSeriesDetailDTO>> SeriesDetailsByMICRCode(string micrCode){
                return  (List<ChequeSeriesDetailDTO>) await _ChequeEntryRepository.GetSelectedColumnByConditionAsync(entity => entity.MicrCode == micrCode, entity => new ChequeSeriesDetailDTO{
                    Series = entity.SeriesNo,
                    Quantity = entity.Quantity,
                    AvailableQuantity = CommonHelper.calculateAvailableQuantity(entity.CurrentPosition,entity.End,entity.Start),
                    TreasurieCode = entity.TreasurieCode,
                    MicrCode = entity.MicrCode
                });
        }
    }
}