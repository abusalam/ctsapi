using System.Data.Common;
using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.BAL.Services;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    // [Route("api/[controller]")]
    //[ApiController]


    public class ByTransferController : ApiBaseController
    {
        private readonly IClaimService _claimService;
        private readonly IMqService _mqService;
        private readonly IByTransferService _byTransferHeadService;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ByTransferController(
            IClaimService claimService,
            IMqService mqService,
            IByTransferService byTransferHeadService
        )
            : base(claimService)
        {
            _claimService = claimService;
            _mqService = mqService;
            _byTransferHeadService = byTransferHeadService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

 



        [HttpPost("by-transfer")]
        [Tags("Pension: ByTransfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> SaveByTransferHead(
            ByTransferHeadEntryDTO byTransferHeadEntryDTO
        )
        {
            JsonAPIResponse<ByTransferHeadResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "ByTransfer Head saved successfully!",
            };

            try
            {
                // Call the service method to save the ByTransferHead
                response.Result =
                    await _byTransferHeadService.SaveByTransferHead<ByTransferHeadResponseDTO>(
                        byTransferHeadEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                // Handle exceptions and populate error response
                FillException(response, ex);
                return response;
            }
            finally
            {
                // Attach error messages from DataSource if any
                FillErrorMesageFromDataSource(response);
            }

            return response;
        }
    }
}
