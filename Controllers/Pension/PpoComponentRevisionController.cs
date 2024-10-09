using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/ppo")]
    public class PpoComponentRevisionController : ApiBaseController
    {
        private readonly IPpoComponentRevisionService _ppoComponentRevisionService;
        // Initializes a new instance of the PpoComponentRevisionController class.
        // 
        // Parameters:
        //   ppoComponentRevisionService (IPpoComponentRevisionService): The PPO component rate service.
        //   claimService (IClaimService): The claim service.
        // 
        // Returns:
        //   PpoComponentRevisionController: A new instance of the PpoComponentRevisionController class.
        public PpoComponentRevisionController(
                IPpoComponentRevisionService ppoComponentRevisionService,
                IClaimService claimService
            ) : base(claimService)
        {
            _ppoComponentRevisionService = ppoComponentRevisionService;
        }


        [HttpGet("component-revision/ppos")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<JsonAPIResponse<TableResponseDTO<PpoComponentRevisionPpoListItemDTO>>> GetAllPposForComponentRevisions()
        {
            JsonAPIResponse<TableResponseDTO<PpoComponentRevisionPpoListItemDTO>> response= new() {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Message = $"All PPOs for Component Revisions Received Successfully!"
                };

            try {

                response.Result = new TableResponseDTO<PpoComponentRevisionPpoListItemDTO>() {
                    Headers = new () {
                        new() {
                            Name = "PPO ID",
                            FieldName = "ppoId"
                        },
                        new() {
                            Name = "PPO No",
                            FieldName = "ppoNo"
                        },
                        new() {
                            Name = "Pensioner Name",
                            FieldName = "pensionerName"
                        },
                        new() {
                            Name = "Category Description",
                            FieldName = "categoryDescription"
                        },
                        new() {
                            Name = "Bank",
                            FieldName = "bankBranchName"
                        },
                    },
                    Data = await _ppoComponentRevisionService.GetPposForComponentRevisions<PpoComponentRevisionPpoListItemDTO>(
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    )
                };
                response.Result.Data.ForEach(
                    item => {
                        item.BankBranchName = item.Branch?.Bank?.BankName + "-" + item.Branch?.BranchName;
                        item.CategoryDescription = item.Category?.CategoryName ?? "";
                        item.Branch = null;
                        item.Category = null;
                    }
                );
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                ApiResponseStatus = Enum.APIResponseStatus.Error,
                Result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
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
        [HttpPost("{ppoId}/component-revision")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoComponentRevisionResponseDTO>> CreateSinglePpoComponentRevision(
            int ppoId,
            PpoComponentRevisionEntryDTO ppoComponentRevisionEntryDTO
        )
        {

            JsonAPIResponse<PpoComponentRevisionResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Component Revision saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _ppoComponentRevisionService
                    .CreateSinglePpoComponentRevision<PpoComponentRevisionEntryDTO, PpoComponentRevisionResponseDTO>(
                        ppoId,
                        ppoComponentRevisionEntryDTO,
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

        [HttpPost("{ppoId}/component-revisions")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        [Obsolete("Use CreateSinglePpoComponentRevision instead")]
        public async Task<JsonAPIResponse<List<PpoComponentRevisionResponseDTO>>> CreatePpoComponentRevisions(
            int ppoId,
            List<PpoComponentRevisionEntryDTO> ppoComponentRevisionEntryDTOs
        )
        {

            JsonAPIResponse<List<PpoComponentRevisionResponseDTO>> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Component Revision saved sucessfully!",
                Result = new()
            };
            try {
                response.Result = await _ppoComponentRevisionService
                    .CreatePpoComponentRevisions<PpoComponentRevisionEntryDTO, PpoComponentRevisionResponseDTO>(
                        ppoId,
                        ppoComponentRevisionEntryDTOs,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            finally {
                if(response.Result.Count == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Component Revision not saved!";
                }
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
        [HttpGet("{ppoId}/component-revision")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<JsonAPIResponse<IEnumerable<PpoComponentRevisionResponseDTO>>> GetPpoComponentRevisionsByPpoId(
            int ppoId
        )
        {
            JsonAPIResponse<IEnumerable<PpoComponentRevisionResponseDTO>> response;
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result =  await _ppoComponentRevisionService.GetPpoComponentRevisionsByPpoId<PpoComponentRevisionResponseDTO>(
                                ppoId,
                                GetCurrentFyYear(),
                                GetTreasuryCode()
                            ),
                    Message = $"All Component Revision Details Received Successfully!"

                };
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                ApiResponseStatus = Enum.APIResponseStatus.Error,
                Result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }

        [HttpPut("{revisionId}/component-revision")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoComponentRevisionResponseDTO>> UpdatePpoComponentRevisionById(
            long revisionId,
            PpoComponentRevisionUpdateDTO ppoComponentRevisionUpdateDTO
        )
        {
            JsonAPIResponse<PpoComponentRevisionResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Component Revision saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result =  await _ppoComponentRevisionService.UpdatePpoComponentRevisionById<PpoComponentRevisionUpdateDTO, PpoComponentRevisionResponseDTO>(
                        revisionId,
                        ppoComponentRevisionUpdateDTO,
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


        [HttpDelete("{revisionId}/component-revision")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoComponentRevisionResponseDTO>> DeletePpoComponentRevisionById(
            long revisionId
        )
        {
            JsonAPIResponse<PpoComponentRevisionResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Component Revision deleted sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result =  await _ppoComponentRevisionService.DeletePpoComponentRevisionById<PpoComponentRevisionResponseDTO>(
                                revisionId,
                                GetCurrentFyYear(),
                                GetTreasuryCode()
                            ),
                    Message = $"Component Revision deleted successfully!"

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
    }
}