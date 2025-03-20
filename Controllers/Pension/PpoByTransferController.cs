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
    public class PpoByTransferController : ApiBaseController
    {
        private readonly IClaimService _claimService;
        private readonly IMqService _mqService;
        private readonly IPpoByTransferService _ppobyTransferHeadService;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public PpoByTransferController(
            IClaimService claimService,
            IMqService mqService,
            IPpoByTransferService ppobyTransferHeadService
        )
            : base(claimService)
        {
            _claimService = claimService;
            _mqService = mqService;
            _ppobyTransferHeadService = ppobyTransferHeadService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [HttpPost("ppo-by-transfer-headmap")]
        [Tags("Pension:Ppo By Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoByTransferHeadResponseDTO>> CreatePPoByTransferHeadMap(
            PpoByTransferEntryDTO ppobyTransferHeadEntryDTO
        )
        {
            JsonAPIResponse<PpoByTransferHeadResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Ppo ByTransfer Head saved successfully!",
                Result = new() { DataSource = null },
            };

            try
            {
                response.Result =
                    await _ppobyTransferHeadService.SavePpoByTransferHead<PpoByTransferHeadResponseDTO>(
                        ppobyTransferHeadEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );

                if (response.Result.DataSource != null)
                {
                    return response;
                }
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
