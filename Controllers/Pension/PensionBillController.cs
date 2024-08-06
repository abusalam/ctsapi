using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.BAL.Interfaces.Pension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/ppo")]
    public class PensionBillController : ApiBaseController
    {
        private readonly IPensionerDetailsService _pensionerDetailsService;
        private readonly IPensionerBankAccountService _pensionerBankAccountService;
        private readonly IPensionBillService _pensionBillService;
        public PensionBillController(
                IPensionerDetailsService pensionerDetailsService,
                IPensionerBankAccountService pensionerBankAccountService,
                IPensionBillService pensionBillService,
                IClaimService claimService
            ) : base(claimService)
        {
            _pensionBillService = pensionBillService;
            _pensionerDetailsService = pensionerDetailsService;
            _pensionerBankAccountService = pensionerBankAccountService;
        }

        [HttpPost("{ppoId}/first-bill-general")]
        [Tags("Pension", "Pension: First Bill")]
        public async Task<JsonAPIResponse<PensionerFirstBillResponseDTO>> ControlFirstBillsCreate(
                int ppoId
            )
        {

            JsonAPIResponse<PensionerFirstBillResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Pensioner Details received sucessfully!",
                Result = new (){
                    Pensioner = new(){},
                    BankAccount = new(){}
                }
            };
            try {
                response.Result.BankAccount = await _pensionerBankAccountService.GetPensionerBankAccount(
                    ppoId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
                response.Result.Pensioner = await _pensionerDetailsService.GetPensioner<PensionerListItemDTO>(
                    ppoId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (DbUpdateException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"Bank Accounts Details not received! Error: {ex.Message}";
            }
            finally {
                if(response.Result?.DataSource != null) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"Bank Accounts Details not received!";
                }
            }

            return response;
        }


        [HttpPatch("pension-bill")]
        [Tags("Pension", "Pension: First Bill")]
        public async Task<JsonAPIResponse<InitiateFirstPensionBillResponseDTO>> ControlFirstBillsGeneration(
                InitiateFirstPensionBillDTO initiateFirstPensionBillDTO
            )
        {

            JsonAPIResponse<InitiateFirstPensionBillResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill generated sucessfully!",
                Result = new (){
                    Pensioner = new(){},
                    BankAccount = new(){}
                }
            };
            try {
                response.Result = await _pensionBillService.GenerateFirstPensionBills(
                    initiateFirstPensionBillDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch(InvalidCastException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"C-Error: {ex.Message}";
            }
            catch (DbUpdateException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"C-Error: Unable to generate first pension bill Error: {ex.Message}";
            }
            finally {
                if(response.Result?.DataSource != null) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Unable to generate first pension bill";
                }
            }

            return response;
        }
    
    }
}