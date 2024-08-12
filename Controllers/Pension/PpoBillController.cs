using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.BAL.Interfaces.Pension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/ppo")]
    public class PpoBillController : ApiBaseController
    {
        private readonly IPensionerDetailsService _pensionerDetailsService;
        private readonly IPensionerBankAccountService _pensionerBankAccountService;
        private readonly IPensionBillService _pensionBillService;
        public PpoBillController(
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

        [HttpPost("pension-bill")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<InitiateFirstPensionBillResponseDTO>> GenerateFirstBill(
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