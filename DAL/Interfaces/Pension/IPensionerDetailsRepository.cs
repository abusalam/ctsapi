using CTS_BE.DAL.Entities;
using CTS_BE.DTOs.PensionDTO;
using CTS_BE.Model.Pension;
using CTS_BE.DAL.Interfaces.Pension;
namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPensionerDetailsRepository : IRepository<PMmPenPrepPensionerDetl>
                                                    , IBaseRepository<PMmPenPrepPensionerDetl>
    {

    }
}