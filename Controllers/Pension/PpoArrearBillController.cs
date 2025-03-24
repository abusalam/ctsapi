using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class PpoArrearBillController(
        IPpoArrearBillService ppoArrearBillService,
        IClaimService claimService
    ) : ApiBaseController(claimService)
    {
        private readonly IPpoArrearBillService _ppoArrearBillService = ppoArrearBillService;

        [HttpGet("arrear-bill/ppos")]
        [Tags("Pension: Arrear Bill")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PpoArrearBillResponseDTO>>
        > GetAllPposForArrearBill()
        {
            JsonAPIResponse<TableResponseDTO<PpoArrearBillResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Ppo List For Arrear Bill Retrieved Sucessfully",
            };
            try
            {
                var ppoList =
                    await _ppoArrearBillService.GetPposForArrearBillGeneration<PpoArrearBillListResponseDTO>(
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
                        new() { Name = "PPO Sl No", FieldName = "id" },
                        new() { Name = "Bank Name", FieldName = "bankName" },
                        new() { Name = "Bank Account No", FieldName = "bankAcNo" },
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

        [HttpGet("arrear-bill/{ppoId}")]
        [Tags("Pension: Arrear Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoArrearBillResponseDTO>> GetArrearPensionBillByPpoId(
            int ppoId
        )
        {
            JsonAPIResponse<PpoArrearBillResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Arrear Pension Bill retrieved sucessfully!",
            };
            try
            {
                response.Result = await _ppoArrearBillService.GetArrearBillByPpoId(
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
