using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class PpoFirstBillController(
        IPpoFirstBillService ppoFirstBillService,
        IClaimService claimService
    ) : ApiBaseController(claimService)
    {
        private readonly IPpoFirstBillService _ppoFirstBillService = ppoFirstBillService;

        [HttpGet("first-bill/ppos")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>>
        > GetAllPposForFirstBill()
        {
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO List for first bill received sucessfully!",
            };
            try
            {
                var ppoList =
                    await _ppoFirstBillService.GetPposForFirstBillGeneration<PpoListResponseDTO>(
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "PPO ID", FieldName = "ppoId" },
                        new() { Name = "PPO Number", FieldName = "ppoNo" },
                        new() { Name = "Pensioner Name", FieldName = "pensionerName" },
                        new() { Name = "Mobile", FieldName = "mobileNumber" },
                        new() { Name = "Date of Birth", FieldName = "dateOfBirth" },
                        new() { Name = "Date of Commencement", FieldName = "dateOfCommencement" },
                        new() { Name = "Date of Retirement", FieldName = "dateOfRetirement" },
                        new() { Name = "Date of Commencement", FieldName = "dateOfCommencement" },
                    ],
                    Data = ppoList.PpoList,
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

        [HttpGet("first-bill-print/ppos")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>>
        > GetPposForFirstBillPrint()
        {
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO List for first bill received sucessfully!",
            };
            try
            {
                var ppoList =
                    await _ppoFirstBillService.GetPposForFirstBillPrint<PpoListResponseDTO>(
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "PPO ID", FieldName = "ppoId" },
                        new() { Name = "PPO Number", FieldName = "ppoNo" },
                        new() { Name = "Pensioner Name", FieldName = "pensionerName" },
                        new() { Name = "Mobile", FieldName = "mobileNumber" },
                        new() { Name = "Date of Birth", FieldName = "dateOfBirth" },
                        new() { Name = "Date of Commencement", FieldName = "dateOfCommencement" },
                        new() { Name = "Date of Retirement", FieldName = "dateOfRetirement" },
                        new() { Name = "Date of Commencement", FieldName = "dateOfCommencement" },
                    ],
                    Data = ppoList.PpoList,
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

        [HttpPost("first-bill-generate")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<InitiateFirstPensionBillResponseDTO>
        > GenerateFirstPensionBill(InitiateFirstPensionBillEntryDTO initiateFirstPensionBillDTO)
        {
            JsonAPIResponse<InitiateFirstPensionBillResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill generated sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoFirstBillService.GenerateFirstPensionBill<InitiateFirstPensionBillResponseDTO>(
                        initiateFirstPensionBillDTO,
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

        [HttpPost("first-bill")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoBillSaveResponseDTO>> SaveFirstPensionBill(
            InitiateFirstPensionBillEntryDTO initiateFirstPensionBillEntryDTO
        )
        {
            JsonAPIResponse<PpoBillSaveResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill saved sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoFirstBillService.SaveFirstPensionBill<PpoBillSaveResponseDTO>(
                        initiateFirstPensionBillEntryDTO,
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

        [HttpGet("first-bill/{ppoId}")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoBillResponseDTO>> GetFirstPensionBillByPpoId(int ppoId)
        {
            JsonAPIResponse<PpoBillResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill retrieved sucessfully!",
            };
            try
            {
                response.Result = await _ppoFirstBillService.GetFirstBillByPpoId(
                    ppoId,
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
