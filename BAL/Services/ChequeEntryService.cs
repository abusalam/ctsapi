using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;

namespace CTS_BE.BAL
{
    public class ChequeEntryService : IChequeEntryService
    {
        private readonly IChequeEntryRepository _ChequeEntryRepository;
        private readonly IMapper _mapper;
        public ChequeEntryService(IChequeEntryRepository ChequeEntryRepository, IMapper mapper) {
            _ChequeEntryRepository = ChequeEntryRepository;
            _mapper = mapper;
        }
        public async Task<bool> Insert(string series,short start,short end,short quantity)
        {
            ChequeEntry chequeEntry = new ChequeEntry
            {
                SeriesNo = series,
                Start = start,
                End = end,
                Quantity = quantity,
                //TODO:: Change to logged in user id
                CreatedBy = 1,
            };
            if (_ChequeEntryRepository.Add(chequeEntry))
            {
                _ChequeEntryRepository.SaveChangesManaged();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<ChequeListDTO>> List(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<ChequeListDTO> chequeList =(IEnumerable<ChequeListDTO>) await _ChequeEntryRepository.GetSelectedColumnByConditionAsync(entity=>true,entity=>new ChequeListDTO
            {
                Series = entity.SeriesNo,
                Start = entity.Start,
                End = entity.End,
                Quantity = entity.Quantity
                
            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return chequeList;
        }
    }
}