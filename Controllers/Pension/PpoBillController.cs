using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class PpoBillController(
        IPpoBillService ppoBillService,
        IMqService mqService,
        IClaimService claimService
    ) : ApiBaseController(claimService)
    {
        private readonly IPpoBillService _ppoBillService = ppoBillService;
        private readonly IMqService _mqService = mqService;

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
                    await _ppoBillService.GetPposForFirstBillGeneration<PpoListResponseDTO>(
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
                var ppoList = await _ppoBillService.GetPposForFirstBillPrint<PpoListResponseDTO>(
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
                    await _ppoBillService.GenerateFirstPensionBill<InitiateFirstPensionBillResponseDTO>(
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
                    await _ppoBillService.SaveFirstPensionBill<PpoBillSaveResponseDTO>(
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
                response.Result = await _ppoBillService.GetFirstBillByPpoId(
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

        [HttpGet("regular-bill/{year}/{month}/ppos")]
        [Tags("Pension: Regular Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoListResponseDTO>> GetAllPposForRegularBill(
            short year,
            short month
        )
        {
            JsonAPIResponse<PpoListResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO List for regular bill received sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoBillService.GetPposForBillGeneration<PpoListResponseDTO>(
                        year,
                        month,
                        BillType.RegularBill,
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

        [HttpPost("regular-bill")]
        [Tags("Pension: Regular Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoBillSaveResponseDTO>> SaveRegularPensionBill(
            PpoBillEntryDTO ppoBillEntryDTO
        )
        {
            JsonAPIResponse<PpoBillSaveResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Regular Pension Bill saved sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoBillService.SaveRegularPensionBill<PpoBillSaveResponseDTO>(
                        ppoBillEntryDTO,
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

        [HttpGet("regular-bill/{year}/{month}/bills")]
        [Tags("Pension: Regular Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<RegularBillListResponseDTO>> GetAllRegularPensionBills(
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
                response.Result = await _ppoBillService.GetRegularPensionBills(
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
                FillErrorMessageFromDataSource(response);
            }
            return response;
        }
    }
}
