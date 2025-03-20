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

        [HttpPost("by-transfer-headmap")]
        [Tags("Pension: By Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> CreateByTransferHeadMap(
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
                response.Result =
                    await _byTransferHeadService.SaveByTransferHead<ByTransferHeadResponseDTO>(
                        byTransferHeadEntryDTO
                    );
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

        [HttpGet("by-transfer-headmap/{byTransferHeadId}")]
        [Tags("Pension: By Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> GetByTransferHeadById(
            long byTransferHeadId
        )
        {
            JsonAPIResponse<ByTransferHeadResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "By Transfer Head details received successfully!",
            };

            try
            {
                response.Result =
                    await _byTransferHeadService.GetByTransferHeadById<ByTransferHeadResponseDTO>(
                        byTransferHeadId
                    );
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

        [HttpGet("by-transfer-headmaps")]
        [Tags("Pension: By Transfer")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ByTransferHeadResponseDTO>>
        > GetAllByTransferHeads()
        {
            JsonAPIResponse<TableResponseDTO<ByTransferHeadResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "All By Transfer Head details retrieved successfully!",
            };

            try
            {
                var byTransferHeads =
                    await _byTransferHeadService.GetAllByTransferHeads<ByTransferHeadResponseDTO>();

                response.Result = new()
                {
                    Headers = new()
                    {
                        new() { Name = "ID", FieldName = "Id" },
                        new() { Name = "By Transfer Type", FieldName = "ByTransferType" },
                        new() { Name = "Account Head ID", FieldName = "AccountHeadId" },
                        new() { Name = "Description", FieldName = "ByTransferDescription" },
                        new() { Name = "AG By Transfer", FieldName = "AgBytransfer" },
                        new()
                        {
                            Name = "By Transfer Head Count",
                            FieldName = "ByTransferHeadCount",
                        },
                    },
                    Data = byTransferHeads,
                };
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
