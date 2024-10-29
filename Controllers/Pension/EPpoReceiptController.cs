using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [ApiController]
    [Route("api/v1/e-ppo")]
    public class EPpoReceiptController : ApiBaseController
    {
        private readonly IEPpoReceiptService _ppoReceiptService;
        public EPpoReceiptController(
            IEPpoReceiptService ppoReceiptService,
            IClaimService claimService
        ) : base(claimService)
        {
            _ppoReceiptService = ppoReceiptService;
        }

        [HttpPost("receipt")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptEntryDTO>> SendEPpoReceipt(
            EPpoReceiptEntryDTO ePpoReceiptEntryDTO
        )
        {

            JsonAPIResponse<EPpoReceiptEntryDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Receipt saved sucessfully!"
            };
            try {
                response.Result = await _ppoReceiptService.CreateEPpoReceipt<EPpoReceiptEntryDTO>(
                    ePpoReceiptEntryDTO,
                    GetTreasuryCode(),
                    GetCurrentFyYear()
                );
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

        [HttpPost("revision")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptRevisionResponseDTO>> SendEPpoReceiptRevision(
            EPpoReceiptRevisionEntryDTO ePpoReceiptRevisionEntryDTO
        )
        {

            JsonAPIResponse<EPpoReceiptRevisionResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Revision saved sucessfully!"
            };
            try {
                response.Result = await _ppoReceiptService.CreateEPpoReceiptRevision<EPpoReceiptRevisionResponseDTO>(
                    ePpoReceiptRevisionEntryDTO,
                    GetTreasuryCode(),
                    GetCurrentFyYear()
                );
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

        [HttpGet("{ppoNo}/receipt")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptPpoIdResponseDTO>> GetPpoIdForEPpoReceipt(
            string ppoNo
        )
        {
            JsonAPIResponse<EPpoReceiptPpoIdResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Receipt Details received sucessfully!"
            };
            try {
                response.Result = await _ppoReceiptService.GetPpoIdByPpoNo<EPpoReceiptPpoIdResponseDTO>(
                    ppoNo,
                    GetTreasuryCode(),
                    GetCurrentFyYear()
                );
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

        [HttpPut("{applicationNo}/receipt")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptWithdrawlResponseDTO>> WithdrawEPpoReceipt(
            string applicationNo,
            EPpoReceiptWithdrawlEntryDTO ePpoReceiptWithdrawlEntryDTO
        )
        {

            JsonAPIResponse<EPpoReceiptWithdrawlResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Receipt withdrawn sucessfully!"
            };
            try {
                response.Result = await _ppoReceiptService.RegisterEPpoReceiptWithdrawal<EPpoReceiptWithdrawlResponseDTO>(
                    applicationNo,
                    ePpoReceiptWithdrawlEntryDTO,
                    GetTreasuryCode(),
                    GetCurrentFyYear()
                );
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