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

        // Creates a new PPO component revision.
        // 
        // Parameters:
        //   ppoComponentRevisionEntryDTO (PpoComponentRevisionEntryDTO): The PPO component rate entry DTO.
        // 
        // Returns:
        //   JsonAPIResponse<PpoComponentRevisionResponseDTO>: A JSON API response containing a PPO component rate response DTO.
        [HttpPost("component-revision")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoComponentRevisionResponseDTO>> CreatePpoComponentRevision(
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
                    .CreatePpoComponentRevision<PpoComponentRevisionEntryDTO, PpoComponentRevisionResponseDTO>(
                        ppoComponentRevisionEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            finally {
                if(response.Result.DataSource != null) {
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
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                ApiResponseStatus = Enum.APIResponseStatus.Error,
                Result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            finally {
                if(response.Result?.DataSource != null) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Component Revision not saved!";
                }
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
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                ApiResponseStatus = Enum.APIResponseStatus.Error,
                Result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            finally {
                if(response.Result?.DataSource != null) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Component Revision not deleted!";
                }
            }
            return response;
        }
    }
}