using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeEntryService
    {
        public Task<bool> Insert(long userId,string treasurieCode, string MicrCode, string series, short start, short end, short quantity);
        public Task<IEnumerable<ChequeListDTO>> List(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        public Task<IEnumerable<DropdownDTO>> AllSeries();
        public Task<ChequeSeriesDetailDTO> SeriesDetailsById(long id);
    }
}