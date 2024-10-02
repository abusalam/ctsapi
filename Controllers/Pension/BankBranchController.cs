using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [ApiController]
    [Route("api/v1")]
    public class BankBranchController : ApiBaseController
    {
        private readonly IBankBranchService _bankBranchService;
        public BankBranchController(
            IClaimService claimService,
            IBankBranchService bankBranchService
        ) : base(claimService)
        {
            _bankBranchService = bankBranchService;
        }

        [HttpGet("banks")]
        [Tags("Pension: Bank Branch")]
        [OpenApi]
        public async Task<JsonAPIResponse<BankListResponseDTO>> GetBanks()
        {
            JsonAPIResponse<BankListResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Banks Received Successfully!"
            };
            try {
                response.Result = await _bankBranchService
                    .GetBanks(GetTreasuryCode());
            }
            catch(Exception ex) {
                FillException(response, ex);
                return response;
            }
            finally {
                FillErrorMesageFromDataSource(response);
            }

            return response;
        }

        [HttpGet("branches/{bankId}")]
        [Tags("Pension: Bank Branch")]
        [OpenApi]
        public async Task<JsonAPIResponse<BranchListResponseDTO>> GetBranchesByBankId(long bankId)
        {
            JsonAPIResponse<BranchListResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Branches Received Successfully!"
            };
            try {
                response.Result = await _bankBranchService
                    .GetBranchesByBankId(GetTreasuryCode(), bankId);
            }
            catch(Exception ex) {
                FillException(response, ex);
                return response;
            }
            finally {
                FillErrorMesageFromDataSource(response);
            }

            return response;
        }    
    
    }
}