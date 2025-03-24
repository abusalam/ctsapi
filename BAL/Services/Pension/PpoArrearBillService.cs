using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoArrearBillService(
        IClaimService claimService,
        IMapper mapper,
        IPpoArrearBillRepository ppoArrearBillRepository
    ) : BaseService(claimService), IPpoArrearBillService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPpoArrearBillRepository _ppoArrearBillRepository =
            ppoArrearBillRepository;

        public async Task<T> GetPposForArrearBillGeneration<T>(
            short financialYear,
            string treasuryCode
        )
        {
            PpoArrearBillListResponseDTO ppoListResponseDTO = new();

            try
            {
                List<Pensioner>? ppoList =
                    await _ppoArrearBillRepository.GetPensionersForArrearBillGeneration(
                        financialYear,
                        treasuryCode
                    );

                ppoListResponseDTO.PpoList = _mapper.Map<List<PpoArrearBillResponseDTO>>(ppoList);

                foreach (var pensioner in ppoList)
                {
                    var ppo = ppoListResponseDTO.PpoList.FirstOrDefault(x => x.Id == pensioner.Id);
                    if (ppo != null && pensioner.Branch != null && pensioner.Branch.Bank != null)
                    {
                        ppo.BankName = pensioner.Branch.Bank.BankName;
                    }
                }

                return _mapper.Map<T>(ppoListResponseDTO);
            }
            catch (Exception ex)
            {
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillErrorInDataSource(new { }, "ServiceException: ", ex);
                return ppoBillResponseDTO;
            }
        }

        public async Task<PpoArrearBillResponseDTO> GetArrearBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            PpoArrearBillResponseDTO ppoBillResponseDTO = new();
            try
            {
                if (
                    !await _ppoArrearBillRepository.IsFirstBillAlreadyGenerated(
                        ppoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    ppoBillResponseDTO.FillErrorInDataSource(
                        ppoId,
                        $"First Pension Bill Not Found. Please Generate First Pension Bill Or Check PPO Id: {ppoId}."
                    );
                    return ppoBillResponseDTO;
                }

                var pensioner = await _ppoArrearBillRepository.GetPpoArrearBillByPpoId(
                    ppoId,
                    financialYear,
                    treasuryCode
                );

                if (pensioner == null)
                {
                    ppoBillResponseDTO.FillErrorInDataSource(
                        ppoId,
                        "Pensioner not found. Please check PPO ID."
                    );
                    return ppoBillResponseDTO;
                }

                bool isRunning = pensioner.PpoStatusFlags.Any(status =>
                    status.ActiveFlag && status.StatusFlag == PensionStatusFlag.PpoRunning
                );

                if (!isRunning)
                {
                    ppoBillResponseDTO.FillErrorInDataSource(
                        ppoId,
                        "Pensioner's status is not running."
                    );
                    return ppoBillResponseDTO;
                }

                ppoBillResponseDTO.Id = pensioner.Id;
                ppoBillResponseDTO.PpoNo = pensioner.PpoNo;
                ppoBillResponseDTO.PensionerName = pensioner.PensionerName;
                ppoBillResponseDTO.BankAcNo = pensioner.BankAcNo;

                if (pensioner != null && pensioner.Branch != null && pensioner.Branch.Bank != null)
                {
                    ppoBillResponseDTO.BankName = pensioner.Branch.Bank.BankName;
                }

                return ppoBillResponseDTO;
            }
            catch (Exception ex)
            {
                ppoBillResponseDTO.FillErrorInDataSource(ppoId, $"ServiceException: {ex.Message}");
                return ppoBillResponseDTO;
            }
        }
    }
}
