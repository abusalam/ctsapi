using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class PpoComponentRevisionController(
        IPpoComponentRevisionService ppoComponentRevisionService,
        IClaimService claimService
    ) : ApiBaseController(claimService)
    {
        private readonly IPpoComponentRevisionService _ppoComponentRevisionService =
            ppoComponentRevisionService;

        [HttpGet("ppo-component-revision/ppos")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PpoComponentRevisionPpoListItemDTO>>
        > GetAllPposForComponentRevisions()
        {
            JsonAPIResponse<TableResponseDTO<PpoComponentRevisionPpoListItemDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All PPOs for Component Revisions Received Successfully!",
            };

            try
            {
                TableResponseDTO<PpoComponentRevisionPpoListItemDTO>? tableResponse =
                    await _ppoComponentRevisionService.GetPposForComponentRevisions<
                        TableResponseDTO<PpoComponentRevisionPpoListItemDTO>
                    >(GetCurrentFyYear(), GetTreasuryCode());

                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "PPO ID", FieldName = "ppoId" },
                        new() { Name = "PPO No", FieldName = "ppoNo" },
                        new() { Name = "Pensioner Name", FieldName = "pensionerName" },
                        new() { Name = "Category Description", FieldName = "categoryDescription" },
                        new() { Name = "Bank", FieldName = "bankBranchName" },
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

        // Creates a new PPO component revision.
        //
        // Parameters:
        //   ppoComponentRevisionEntryDTO (PpoComponentRevisionEntryDTO): The PPO component rate entry DTO.
        //
        // Returns:
        //   JsonAPIResponse<PpoComponentRevisionResponseDTO>: A JSON API response containing a PPO component rate response DTO.
        [HttpPost("ppo/{ppoId}/component-revision")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<PpoComponentRevisionResponseDTO>
        > CreateSinglePpoComponentRevision(
            int ppoId,
            PpoComponentRevisionEntryDTO ppoComponentRevisionEntryDTO
        )
        {
            JsonAPIResponse<PpoComponentRevisionResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Component Revision saved sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoComponentRevisionService.CreateSinglePpoComponentRevision<PpoComponentRevisionResponseDTO>(
                        ppoId,
                        ppoComponentRevisionEntryDTO,
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

        // Retrieves a list of PPO component revisions for a given PPO ID.
        //
        // Parameters:
        //   ppoId (int): The ID of the PPO for which to retrieve component revisions.
        //
        // Returns:
        //   JsonAPIResponse<IEnumerable<PpoComponentRevisionResponseDTO>>: A JSON API response containing a list of PPO component rate response DTOs.
        [HttpGet("ppo/{ppoId}/component-revisions")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PpoComponentRevisionResponseDTO>>
        > GetPpoComponentRevisionsByPpoId(int ppoId)
        {
            JsonAPIResponse<TableResponseDTO<PpoComponentRevisionResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Component Revision Details Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "Revision ID", FieldName = "id" },
                        new() { Name = "Rate ID", FieldName = "rateId" },
                        new()
                        {
                            Name = "Component Description",
                            FieldName = "componentDescription",
                        },
                        new() { Name = "Amount/Month", FieldName = "amountPerMonth" },
                        new() { Name = "From", FieldName = "fromDate" },
                        new() { Name = "To", FieldName = "toDate" },
                    ],
                    Data =
                        await _ppoComponentRevisionService.GetPpoComponentRevisionsByPpoId<PpoComponentRevisionResponseDTO>(
                            ppoId,
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
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

        [HttpPut("ppo-component-revision/{revisionId}")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<PpoComponentRevisionResponseDTO>
        > UpdatePpoComponentRevisionById(
            long revisionId,
            PpoComponentRevisionUpdateDTO ppoComponentRevisionUpdateDTO
        )
        {
            JsonAPIResponse<PpoComponentRevisionResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Component Revision saved sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoComponentRevisionService.UpdatePpoComponentRevisionById<PpoComponentRevisionResponseDTO>(
                        revisionId,
                        ppoComponentRevisionUpdateDTO,
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

        [HttpDelete("ppo-component-revision/{revisionId}")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<PpoComponentRevisionResponseDTO>
        > DeletePpoComponentRevisionById(long revisionId)
        {
            JsonAPIResponse<PpoComponentRevisionResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Component Revision deleted sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoComponentRevisionService.DeletePpoComponentRevisionById<PpoComponentRevisionResponseDTO>(
                        revisionId,
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
    }
}
