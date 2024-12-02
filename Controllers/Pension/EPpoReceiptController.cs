using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
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
            };
            try {
                response = new(){
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = await _ppoReceiptService.CreateEPpoReceipt<EPpoReceiptEntryDTO>(
                        ePpoReceiptEntryDTO,
                        GetTreasuryCode(),
                        GetCurrentFyYear()
                    ),
                    Message = $"ePPO Receipt saved sucessfully!"
                };
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
        public async Task<JsonAPIResponse<EPpoReceiptResponseDTO>> GetPpoIdForEPpoReceipt(
            string ppoNo
        )
        {
            JsonAPIResponse<EPpoReceiptResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Receipt Details received sucessfully!"
            };
            try {
                response.Result = await _ppoReceiptService.GetPpoIdByPpoNo<EPpoReceiptResponseDTO>(
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

        [HttpPut("{pension_appln_no}/update")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptWithdrawlResponseDTO>> UpdateEPpoReceipt(
            EPpoReceiptWithdrawlEntryDTO ePpoReceiptWithdrawlEntryDTO
        )
        {
            JsonAPIResponse<EPpoReceiptWithdrawlResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Receipt updated successfully!"
            };

            // Validate the flag value
            if (ePpoReceiptWithdrawlEntryDTO.Flag != "F" && 
                ePpoReceiptWithdrawlEntryDTO.Flag != "P" && 
                ePpoReceiptWithdrawlEntryDTO.Flag != "R")
            {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Invalid flag value. It must be either 'F', 'P', or 'R'.";
                return response;
            }

            try {
            response.Result = await _ppoReceiptService.RegisterEPpoReceiptWithdrawal<EPpoReceiptWithdrawlResponseDTO>(
                ePpoReceiptWithdrawlEntryDTO.ApplicationNo,
                ePpoReceiptWithdrawlEntryDTO,
                GetTreasuryCode(),
                GetCurrentFyYear());
            }
            catch (Exception ex)
            {
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMesageFromDataSource(response);
            }

            return response;
        }
    }
}