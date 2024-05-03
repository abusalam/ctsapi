using CTS_BE.DTOs;
using CTS_BE.Model.Cheque;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeEntryService
    {
        public Task<bool> Insert(ChequeEntryModel chequeEntryModel);
        public Task<IEnumerable<ChequeListDTO>> List(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        public Task<IEnumerable<DropdownDTO>> AllSeries();
        public Task<ChequeSeriesDetailDTO> SeriesDetailsById(long id);
        public Task<List<ChequeSeriesDetailDTO>> SeriesDetailsByMICRCode(string micrCode);
    }
}