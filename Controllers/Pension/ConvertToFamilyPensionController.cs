using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class ConvertToFamilyPensionController(
        IConvertToFamilyPensionService convertToFamilyPensionService,
        IClaimService claimService
    ) : ApiBaseController(claimService)
    {
        private readonly IConvertToFamilyPensionService _convertToFamilyPensionService =
            convertToFamilyPensionService;

        [HttpGet("convert-to-family-pension-list")]
        [Tags("Pension: Convert To Family Pension")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>>
        > GetPensionersToConvert()
        {
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers =
                        [
                            new() { Name = "PPO ID", FieldName = "ppoId" },
                            new() { Name = "Name of Pensioner", FieldName = "pensionerName" },
                            new() { Name = "Mobile", FieldName = "mobileNumber" },
                            new() { Name = "Date of Birth", FieldName = "dateOfBirth" },
                            new() { Name = "Date of Retirement", FieldName = "dateOfRetirement" },
                            new() { Name = "PPO No", FieldName = "ppoNo" },
                        ],
                        Data =
                            await _convertToFamilyPensionService.GetPensioners<PensionerListItemDTO>(
                                GetTreasuryCode()
                            ),
                    },
                    Message = $"All PPO Details Received Successfully!",
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
