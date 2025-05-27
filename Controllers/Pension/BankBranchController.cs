using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class BankBranchController(
        IClaimService claimService,
        IBankBranchService bankBranchService,
        ILogger<BankBranchController> logger
    ) : ApiBaseController(claimService)
    {
        private readonly IBankBranchService _bankBranchService = bankBranchService;
        private readonly ILogger<BankBranchController> _logger = logger;

        [HttpGet("banks")]
        [Tags("Pension: Bank Branch")]
        [OpenApi]
        public async Task<JsonAPIResponse<BankListResponseDTO>> GetBanks()
        {
            _logger.LogInformation("Received request to get all banks.");

            JsonAPIResponse<BankListResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Banks Received Successfully!",
            };
            try
            {
                response.Result = await _bankBranchService.GetBanks(GetTreasuryCode());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching banks.");
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }

        [HttpGet("branches/{bankId}")]
        [Tags("Pension: Bank Branch")]
        [OpenApi]
        public async Task<JsonAPIResponse<BranchListResponseDTO>> GetBranchesByBankId(long bankId)
        {
            _logger.LogInformation(
                "Received request to get branches for bank ID: {BankId}",
                bankId
            );
            JsonAPIResponse<BranchListResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Branches Received Successfully!",
            };
            try
            {
                response.Result = await _bankBranchService.GetBranchesByBankId(
                    GetTreasuryCode(),
                    bankId
                );
                _logger.LogInformation(
                    "Branches for bank ID {BankId} retrieved successfully.",
                    bankId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching branches for bank ID: {BankId}",
                    bankId
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
