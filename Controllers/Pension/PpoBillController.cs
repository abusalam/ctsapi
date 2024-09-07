using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.BAL.Interfaces.Pension;
using Microsoft.AspNetCore.Mvc;
using CTS_BE.BAL.Interfaces;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/ppo")]
    public class PpoBillController : ApiBaseController
    {
        private readonly IPensionerDetailsService _pensionerDetailsService;
        private readonly IPensionerBankAccountService _pensionerBankAccountService;
        private readonly IPensionBillService _pensionBillService;
        private readonly IPpoBillService _ppoBillService;
        private readonly IMqService _mqService;
        private readonly CancellationTokenSource _cancellationTokenSource;
        static bool IsQueueWorkerRunning = false;
        public PpoBillController(
                IPensionerDetailsService pensionerDetailsService,
                IPensionerBankAccountService pensionerBankAccountService,
                IPensionBillService pensionBillService,
                IPpoBillService ppoBillService,
                IMqService mqService,
                IClaimService claimService
            ) : base(claimService)
        {
            _pensionBillService = pensionBillService;
            _ppoBillService = ppoBillService;
            _mqService = mqService;
            _pensionerDetailsService = pensionerDetailsService;
            _pensionerBankAccountService = pensionerBankAccountService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [HttpPost("first-bill-generate")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<InitiateFirstPensionBillResponseDTO>> GenerateFirstPensionBill(
                InitiateFirstPensionBillDTO initiateFirstPensionBillDTO
            )
        {

            JsonAPIResponse<InitiateFirstPensionBillResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill generated sucessfully!",
                Result = new (){
                    // Pensioner = new(){},
                    // BankAccount = new(){}
                }
            };
            try {
                response.Result = await _pensionBillService.GenerateFirstPensionBill<InitiateFirstPensionBillResponseDTO>(
                    initiateFirstPensionBillDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
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
    
        [HttpPost("first-bill")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoBillResponseDTO>> SaveFirstPensionBill(
                PpoBillEntryDTO ppoBillEntryDTO
            )
        {

            JsonAPIResponse<PpoBillResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill saved sucessfully!",
                Result = new () {
                    DataSource = new()
                }
            };
            try {
                PensionerFirstBillResponseDTO firstBill = await _pensionBillService.GenerateFirstPensionBill<PensionerFirstBillResponseDTO>(
                    new InitiateFirstPensionBillDTO
                    {
                        PpoId = ppoBillEntryDTO.PpoId,
                        ToDate = ppoBillEntryDTO.ToDate
                    },
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
                response.Result = await _ppoBillService.SaveFirstBill<PpoBillResponseDTO>(
                    firstBill,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
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

        [HttpGet("first-bill/{ppoId}")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoBillResponseDTO>> GetFirstPensionBillByPpoId(
                int ppoId
            )
        {

            JsonAPIResponse<PpoBillResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill retrieved sucessfully!",
            };
            try {
                response.Result = await _ppoBillService.GetFirstBillByPpoId(
                    ppoId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
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

        [HttpPost("bill")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public Task<string> SendFirstPensionBill(string message)
        {

            string response = "";
            try {
                int messagesToSend = 0;
                try{
                    messagesToSend = Int32.Parse(message);
                }
                catch(FormatException) {
                    Console.WriteLine("Trying to send single messsage");
                }
                if(messagesToSend > 0) {
                    for(int i = 0; i < messagesToSend; i++) {
                         _mqService.Despatch(GetTreasuryCode(), $"Bill {i+1}");
                    }
                    response = $"Dispatched {messagesToSend} messages";
                } else {
                    response = _mqService.Despatch(GetTreasuryCode(), message);
                }
            }
            finally {

            }
            return Task.FromResult(response);
        }
    }
}