using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Repositories.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Model.Pension;
using CTS_BE.DTOs.PensionDTO;
using AutoMapper;
using CTS_BE.DAL.Entities;

namespace CTS_BE.BAL.Services.Pension
{
  public class PensionService : IPensionService
  {
    private readonly IPensionerDetailsRepository _pensionerDetailsRepository;
    private readonly IMapper _mapper;
    public PensionService(IPensionerDetailsRepository pensionerDetailsRepository, IMapper mapper)
    {
        _pensionerDetailsRepository = pensionerDetailsRepository;
        _mapper = mapper;
    }

    public async Task<int> CreatePensionerDetails(PensionerDetailsDTO details)
    {
        if(details == null)
        {
            throw new ArgumentNullException(nameof(details));
        }
        PMmPenPrepPensionerDetl pensionerDetailsEntity = _mapper.Map<PMmPenPrepPensionerDetl>(details);
        _pensionerDetailsRepository.Add(pensionerDetailsEntity);
        return await _pensionerDetailsRepository.SaveChangesManagedAsync();
    }
  }
}