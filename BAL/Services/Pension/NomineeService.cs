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
    public class NomineeService : BaseService, INomineeService
    {
        private readonly IMapper _mapper;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository;
        private readonly IBankBranchRepository _bankBranchRepository;
        private readonly INomineeRepository _nomineeRepository;

        public NomineeService(
            IMapper mapper,
            IPensionerDetailsRepository pensionerDetailsRepository,
            IBankBranchRepository bankBranchRepository,
            INomineeRepository nomineeRepository,
            IClaimService claimService
        ) : base(claimService)
        {
            _mapper = mapper;
            _pensionerDetailsRepository = pensionerDetailsRepository;
            _bankBranchRepository = bankBranchRepository;
            _nomineeRepository = nomineeRepository;
        }

        public async Task<NomineeListResponseDTO> GetNomineeByPpoId(
            int ppoId,
            string treasuryCode
        )
        {
            List<Nominee>? nomineeList = new();
            NomineeListResponseDTO response = new();
            try {
                nomineeList = await _nomineeRepository.GetNomineeByPpoIdAsync(
                    ppoId,
                    treasuryCode,
                    entity => _mapper.Map<Nominee>(entity)
                );

                response.Nominees = _mapper.Map<List<NomineeResponseDTO>>(nomineeList);
                if (response.NomineeCount == 0)
                {
                    response.FillDataSource(
                        nomineeList,
                        "No Nominees has been registered yet."
                    );
                }
                return response;
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    nomineeList,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
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
            Nominee nomineeEntity = new();
            T? response = _mapper.Map<T>(nomineeEntryDTO);
            try
            {
                Pensioner? pensioner = await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                    nomineeEntryDTO.PpoId,
                    financialYear,
                    treasuryCode,
                    entity => _mapper.Map<Pensioner>(entity)
                );

                if (pensioner is null)
                {
                    response.FillDataSource(
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
                        response.FillDataSource(
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
                return await _nomineeRepository.SaveNomineeDetails<T>(
                    nomineeEntity,
                    treasuryCode
                );
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    nomineeEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
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
            T? response =  _mapper.Map<T>(nomineeEntryDTO);
            Nominee nomineeEntity = new();

            try
            {

                Nominee? nomineeDetails = await _nomineeRepository.GetNomineeDetailsByIdAsync(
                    nomineeId,
                    treasuryCode,
                    entity => _mapper.Map<Nominee>(entity)
                );

                if (nomineeDetails is null)
                {
                    response.FillDataSource(
                        nomineeDetails,
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
                        response.FillDataSource(
                            nomineeEntity,
                            "Branch not found. Please check branch Id. and try again."
                        );
                        return response;
                    }
                }

                nomineeEntity.FillFrom(nomineeEntryDTO);
                nomineeEntity.Id = nomineeId;
                nomineeEntity.PensionerId = nomineeDetails.PensionerId;
                nomineeEntity.TreasuryCode = nomineeDetails.TreasuryCode;
                nomineeEntity.PpoId = nomineeDetails.PpoId;
                SetUpdatedBy(nomineeEntity);

                return await _nomineeRepository.UpdateNomineeDetails<T>(
                    nomineeEntity,
                    treasuryCode
                );
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                        nomineeEntity,
                        $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                    );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                        nomineeEntity,
                        $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                    );
                return response;
            }
        }

        public async Task<T> GetNomineeDetailsByNomineeId<T>(
            long nomineeId,
            string treasuryCode
        )
        {
            T? response = _mapper.Map<T>(new Nominee());
            try {
                Nominee? nomineeDetails = await _nomineeRepository.GetNomineeDetailsByIdAsync(
                    nomineeId,
                    treasuryCode,
                    entity => _mapper.Map<Nominee>(entity)
                );

                if (nomineeDetails is null)
                {
                    response.FillDataSource(
                        nomineeDetails,
                        "Nominee details does not exist. Please check Id. and try again."
                    );
                    return response;
                }

                response = _mapper.Map<T>(nomineeDetails);
                return response;

            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                    new Nominee(),
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex) {
                response.FillDataSource(
                    new Nominee(),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> DeleteNomineeDetailsById<T>(long nomineeId, string treasuryCode)
        {
            T? response =  _mapper.Map<T>(new NomineeEntryDTO());
            Nominee? nomineeDetails = new();

            try
            {

                nomineeDetails = await _nomineeRepository.GetNomineeDetailsByIdAsync(
                    nomineeId,
                    treasuryCode,
                    entity => _mapper.Map<Nominee>(entity)
                );

                if (nomineeDetails is null)
                {
                    response.FillDataSource(
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
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                        nomineeDetails,
                        $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                    );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                        nomineeDetails,
                        $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                    );
                return response;
            }
        }
    }
}