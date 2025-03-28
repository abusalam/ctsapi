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
        IClaimService claimService
    ) : ApiBaseController(claimService)
    {
        private readonly IPpoRegularBillService _ppoRegularBillService = ppoRegularBillService;

        [Authorize("roles:Accountant,Admin|permissions:can-bill-check")]
        [HttpGet("regular-bill/{year}/{month}/ppos")]
        [Tags("Pension: Regular Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoListResponseDTO>> GetAllPposForRegularBill(
            short year,
            short month
        )
        {
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
