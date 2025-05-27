using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    // [Authorize("roles:clerk|permissions:can-receive-bill")]
    public class PpoRegularBillController(
        IPpoRegularBillService ppoRegularBillService,
        IClaimService claimService,
        ILogger<PpoRegularBillController> logger
    ) : ApiBaseController(claimService)
    {
        private readonly IPpoRegularBillService _ppoRegularBillService = ppoRegularBillService;
        private readonly ILogger<PpoRegularBillController> _logger = logger;

        [Authorize("roles:Accountant,Admin|permissions:can-bill-check")]
        [HttpGet("regular-bill/{year}/{month}/ppos")]
        [Tags("Pension: Regular Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoListResponseDTO>> GetAllPposForRegularBill(
            short year,
            short month
        )
        {
            _logger.LogInformation(
                "Received request to get PPOs for regular bill generation for year: {Year}, month: {Month}",
                year,
                month
            );
            JsonAPIResponse<PpoListResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO List for regular bill received sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoRegularBillService.GetPposForRegularBillGeneration<PpoListResponseDTO>(
                        year,
                        month,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPOs for regular bill generation for year: {Year}, month: {Month}",
                    year,
                    month
                );
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }

        [HttpPost("regular-bill")]
        [Tags("Pension: Regular Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoBillSaveResponseDTO>> SaveRegularPensionBill(
            PpoBillEntryDTO ppoBillEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to save regular pension bill with data: {PpoBillEntryDTO}",
                ppoBillEntryDTO
            );
            JsonAPIResponse<PpoBillSaveResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Regular Pension Bill saved sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoRegularBillService.SaveRegularPensionBill<PpoBillSaveResponseDTO>(
                        ppoBillEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while saving regular pension bill with data: {Data}",
                    ppoBillEntryDTO
                );
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }
            return response;
        }

        [Authorize("roles:Accountant,Admin|permissions:can-bill-check")]
        [HttpGet("regular-bill/{year}/{month}/bills")]
        [Tags("Pension: Regular Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<RegularBillListResponseDTO>> GetAllRegularPensionBills(
            short year,
            short month,
            long? categoryId = null,
            long? bankId = null,
            [FromQuery] long[]? id = null
        )
        {
            _logger.LogInformation(
                "Received request to get regular pension bills for year: {Year}, month: {Month}, categoryId: {CategoryId}, bankId: {BankId}, ids: {Ids}",
                year,
                month,
                categoryId,
                bankId,
                id
            );
            JsonAPIResponse<RegularBillListResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Regular Pension Bills received sucessfully!",
            };
            try
            {
                response.Result = await _ppoRegularBillService.GetRegularPensionBills(
                    year,
                    month,
                    GetCurrentFyYear(),
                    GetTreasuryCode(),
                    categoryId,
                    bankId,
                    id
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching regular pension bills for year: {Year}, month: {Month}, categoryId: {CategoryId}, bankId: {BankId}, ids: {Ids}",
                    year,
                    month,
                    categoryId,
                    bankId,
                    id
                );
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }
            return response;
        }
    }
}
