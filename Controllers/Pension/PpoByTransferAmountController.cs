using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class PpoByTransferAmountController : ApiBaseController
    {
        private readonly IPpoByTransferAmountService _ppobyTransferHeadService;

        public PpoByTransferAmountController(
            IClaimService claimService,
            IPpoByTransferAmountService ppobyTransferHeadService
        )
            : base(claimService)
        {
            _ppobyTransferHeadService = ppobyTransferHeadService;
        }

        [HttpPost("ppo/{ppoId}/by-transfer")]
        [Tags("Pension: PPO By-Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoByTransferAmountResponseDTO>> CreatePpoByTransfer(
            int ppoId,
            PpoByTransferAmountEntryDTO ppoByTransferHeadEntryDTO
        )
        {
            JsonAPIResponse<PpoByTransferAmountResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Ppo By Transfer Head saved successfully!",
            };

            try
            {
                response.Result =
                    await _ppobyTransferHeadService.CreatePpoByTransfer<PpoByTransferAmountResponseDTO>(
                        ppoId,
                        ppoByTransferHeadEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
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

        [HttpPut("ppo-by-transfer/{ppoByTransferId}")]
        [Tags("Pension: PPO By-Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoByTransferAmountResponseDTO>> UpdatePpoByTransfer(
            long ppoByTransferId,
            PpoByTransferAmountUpdateDTO ppoByTransferUpdateDTO
        )
        {
            JsonAPIResponse<PpoByTransferAmountResponseDTO> response =
                new JsonAPIResponse<PpoByTransferAmountResponseDTO>()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Message = "Ppo By Transfer updated successfully!",
                };

            try
            {
                response.Result =
                    await _ppobyTransferHeadService.UpdatePpoByTransfer<PpoByTransferAmountResponseDTO>(
                        ppoByTransferId,
                        ppoByTransferUpdateDTO
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

        [HttpDelete("ppo-by-transfer/{ppoByTransferId}")]
        [Tags("Pension: PPO By-Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<BaseDTO>> DeletePpoByTransferById(long ppoByTransferId)
        {
            JsonAPIResponse<BaseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Ppo By Transfer deleted successfully!",
            };

            try
            {
                response.Result = await _ppobyTransferHeadService.DeletePpoByTransferById<BaseDTO>(
                    ppoByTransferId
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

        [HttpGet("ppo/{ppoId}/by-transfers")]
        [Tags("Pension: PPO By-Transfer")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PpoByTransferAmountResponseListDTO>>
        > GetByTransfersByPpoId(int ppoId)
        {
            JsonAPIResponse<TableResponseDTO<PpoByTransferAmountResponseListDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "All Ppo By Transfer records received successfully!",
            };

            try
            {
                TableResponseDTO<PpoByTransferAmountResponseListDTO>? tableResponse =
                    await _ppobyTransferHeadService.GetByTransfersByPpoId<
                        TableResponseDTO<PpoByTransferAmountResponseListDTO>
                    >(ppoId, GetCurrentFyYear(), GetTreasuryCode());

                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "ID", FieldName = "id" },
                        new() { Name = "PPO No", FieldName = "ppoNo" },
                        new() { Name = "Pensioner Name", FieldName = "pensionerName" },
                        new() { Name = "From Date", FieldName = "fromDate" },
                        new() { Name = "To Date", FieldName = "toDate" },
                        new() { Name = "BT No", FieldName = "bytransferHeadId" },
                        new() { Name = "By Transfer Amount", FieldName = "bytransferAmount" },
                        new() { Name = "Remarks", FieldName = "remarks" },
                    ],
                    Data = tableResponse.Data,
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
