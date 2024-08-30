using CTS_BE.DTOs;
using CTS_BE.Factories.Pension;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1")]
    public class FactoryController : ApiBaseController
    {
        private readonly IDictionary<string, BaseDTO> _factories;
        public FactoryController(
            IClaimService claimService
        ) : base(claimService)
        {
            _factories = new Dictionary<string, BaseDTO>()
            {
                {"ManualPpoReceiptEntryDTO", new PpoReceiptFactory().Create()},
                {"PensionerEntryDTO", new PensionerFactory().Create()},
                {"PensionerBankAcEntryDTO", new BankAccountFactory().Create()}
            };
        }

        [HttpGet("factory/{dtoName}")]
        [Tags("Pension: Factory")]
        [OpenApi]
        public async Task<JsonAPIResponse<object>> CreateFake(string dtoName)
        {
            JsonAPIResponse<object> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"{dtoName} received successfully",
                Result = null
            };
            
            if(!_factories.ContainsKey(dtoName))
            {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Result = _factories.Keys.ToList();
                response.Message = $"{dtoName} not found, please check result for valid options.";
                return await Task.FromResult(response);
            }

            response.Result = _factories[dtoName];

            return await Task.FromResult(response);
        }

    }
}