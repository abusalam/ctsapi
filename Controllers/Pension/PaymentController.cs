using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/payment")]
    public class PaymentController : ApiBaseController
    {
        private readonly IPensionerDetailsService _pensionerDetailsService;
        private readonly IPensionBillService _pensionBillService;
        private readonly IPpoBillService _ppoBillService;
        private readonly IPaymentService _paymentService;
        private readonly IMqService _mqService;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IClaimService _claimService;

        public PaymentController(
            IPensionerDetailsService pensionerDetailsService,
            IPensionBillService pensionBillService,
            IPpoBillService ppoBillService,
            IPaymentService paymentService,
            IMqService mqService,
            IClaimService claimService
        )
            : base(claimService)
        {
            _pensionBillService = pensionBillService;
            _ppoBillService = ppoBillService;
            _mqService = mqService;
            _pensionerDetailsService = pensionerDetailsService;
            _cancellationTokenSource = new CancellationTokenSource();
            _claimService = claimService;
            _paymentService = paymentService;
        }

        [HttpGet("pension-bill/payment")]
        [Tags("Pension: Payment")]
        [OpenApi]
        public async Task<JsonAPIResponse<RegularBillListResponseDTO>> GetPayment(
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
                response.Result = await _paymentService.GetPayment(
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
                FillErrorMesageFromDataSource(response);
            }
            return response;
        }
    }
}
