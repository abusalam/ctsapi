using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class NomineeService(
        IMapper mapper,
        IPensionerDetailsRepository pensionerDetailsRepository,
        IBankBranchRepository bankBranchRepository,
        INomineeRepository nomineeRepository,
        IClaimService claimService
    ) : BaseService(claimService), INomineeService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository =
            pensionerDetailsRepository;
        private readonly IBankBranchRepository _bankBranchRepository = bankBranchRepository;
        private readonly INomineeRepository _nomineeRepository = nomineeRepository;

        public async Task<NomineeListResponseDTO> GetNomineeByPpoId(int ppoId, string treasuryCode)
        {
            List<Nominee>? nomineeList = [];
            NomineeListResponseDTO response = new();
            try
            {
                nomineeList = await _nomineeRepository.GetNomineeByPpoIdAsync(
                    ppoId,
                    treasuryCode,
                    entity => _mapper.Map<Nominee>(entity)
                );

                response.Nominees = _mapper.Map<List<NomineeResponseDTO>>(nomineeList);
                if (response.NomineeCount == 0)
                {
                    response.FillErrorInDataSource(
                        nomineeList,
                        "No Nominees has been registered yet."
                    );
                }
                return response;
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    nomineeList,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> CreateNomineeDetails<T>(
            NomineeEntryDTO nomineeEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            Nominee nomineeEntity = _mapper.Map<Nominee>(nomineeEntryDTO);
            T? response = _mapper.Map<T>(nomineeEntity);
            try
            {
                Pensioner? pensioner =
                    await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                        nomineeEntryDTO.PpoId,
                        financialYear,
                        treasuryCode,
                        entity => _mapper.Map<Pensioner>(entity)
                    );

                if (pensioner is null)
                {
                    response.FillErrorInDataSource(
                        pensioner,
                        "Pensioner not found. Please check PPO Id. and try again."
                    );
                    return response;
                }

                long branchId = nomineeEntryDTO.BranchId ?? 0;
                if (branchId > 0)
                {
                    Branch? branch = await _bankBranchRepository.GetBranchById(
                        treasuryCode,
                        branchId
                    );
                    if (branch is null)
                    {
                        response.FillErrorInDataSource(
                            nomineeEntity,
                            "Branch not found. Please check branch Id. and try again."
                        );
                        return response;
                    }
                }

                nomineeEntity.FillFrom(nomineeEntryDTO);
                nomineeEntity.TreasuryCode = treasuryCode;
                nomineeEntity.PensionerId = pensioner.Id;
                SetCreatedBy(nomineeEntity);
                return await _nomineeRepository.SaveNomineeDetails<T>(nomineeEntity, treasuryCode);
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    nomineeEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> UpdateNomineeDetailsById<T>(
            long nomineeId,
            NomineeEntryDTO nomineeEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            Nominee? nomineeEntity = new();
            T? response = _mapper.Map<T>(nomineeEntity);

            try
            {
                nomineeEntity = await _nomineeRepository.GetNomineeDetailsByIdAsync(
                    nomineeId,
                    treasuryCode,
                    entity => _mapper.Map<Nominee>(entity)
                );

                if (nomineeEntity is null)
                {
                    response.FillErrorInDataSource(
                        nomineeEntity,
                        "Nominee details does not exist. Please check Id. and try again."
                    );
                    return response;
                }

                long branchId = nomineeEntryDTO.BranchId ?? 0;
                if (branchId > 0)
                {
                    Branch? branch = await _bankBranchRepository.GetBranchById(
                        treasuryCode,
                        branchId
                    );
                    if (branch is null)
                    {
                        response.FillErrorInDataSource(
                            nomineeEntity,
                            "Branch not found. Please check branch Id. and try again."
                        );
                        return response;
                    }
                }

                nomineeEntity.FillFrom(nomineeEntryDTO);
                SetUpdatedBy(nomineeEntity);

                return await _nomineeRepository.UpdateNomineeDetails<T>(
                    nomineeEntity,
                    treasuryCode
                );
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    nomineeEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> GetNomineeDetailsByNomineeId<T>(long nomineeId, string treasuryCode)
        {
            Nominee? nomineeDetails = new();
            T? response = _mapper.Map<T>(nomineeDetails);
            try
            {
                nomineeDetails = await _nomineeRepository.GetNomineeDetailsByIdAsync(
                    nomineeId,
                    treasuryCode,
                    entity => _mapper.Map<Nominee>(entity)
                );

                if (nomineeDetails is null)
                {
                    response.FillErrorInDataSource(
                        nomineeDetails,
                        "Nominee details does not exist. Please check Id. and try again."
                    );
                    return response;
                }

                response = _mapper.Map<T>(nomineeDetails);
                return response;
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    nomineeDetails,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> DeleteNomineeDetailsById<T>(long nomineeId, string treasuryCode)
        {
            Nominee? nomineeDetails = new();
            T? response = _mapper.Map<T>(nomineeDetails);

            try
            {
                nomineeDetails = await _nomineeRepository.GetNomineeDetailsByIdAsync(
                    nomineeId,
                    treasuryCode,
                    entity => _mapper.Map<Nominee>(entity)
                );

                if (nomineeDetails is null)
                {
                    response.FillErrorInDataSource(
                        nomineeDetails,
                        "Nominee details does not exist. Please check Id. and try again."
                    );
                    return response;
                }

                nomineeDetails.ActiveFlag = false;
                SetUpdatedBy(nomineeDetails);

                return await _nomineeRepository.DeleteNomineeDetails<T>(
                    nomineeDetails,
                    treasuryCode
                );
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    nomineeDetails,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }
    }
}
