using CTS_BE.DTOs;
using CTS_BE.Factories.Pension;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public enum FactoryEntityEnum
    {
        ByTransferHeadEntryDTO,
        ComponentRateEntryDTO,
        ManualPpoReceiptEntryDTO,
        PensionerEntryDTO,
        PpoByTransferEntryDTO,
        PpoSanctionDetailsEntryDTO,
        PensionPrimaryCategoryEntryDTO,
        PensionSubCategoryEntryDTO,
        PensionBreakupEntryDTO,
        NomineeEntryDTO,
    }

    [Route("api/v1/factory")]
    public class FactoryController : ApiBaseController
    {
        private readonly IDictionary<FactoryEntityEnum, BaseDTO> _factories;

        public FactoryController(IClaimService claimService)
            : base(claimService)
        {
            _factories = new Dictionary<FactoryEntityEnum, BaseDTO>()
            {
                { FactoryEntityEnum.ByTransferHeadEntryDTO, new ByTransferHeadFactory().Create() },
                { FactoryEntityEnum.ComponentRateEntryDTO, new ComponentRateFactory().Create() },
                { FactoryEntityEnum.ManualPpoReceiptEntryDTO, new PpoReceiptFactory().Create() },
                { FactoryEntityEnum.PensionerEntryDTO, new PensionerFactory().Create() },
                { FactoryEntityEnum.PpoByTransferEntryDTO, new PpoByTransferFactory().Create() },
                {
                    FactoryEntityEnum.PpoSanctionDetailsEntryDTO,
                    new PpoSanctionDetailsFactory().Create()
                },
                {
                    FactoryEntityEnum.PensionPrimaryCategoryEntryDTO,
                    new PrimaryCategoryFactory().Create()
                },
                { FactoryEntityEnum.PensionSubCategoryEntryDTO, new SubCategoryFactory().Create() },
                { FactoryEntityEnum.PensionBreakupEntryDTO, new ComponentFactory().Create() },
                { FactoryEntityEnum.NomineeEntryDTO, new NomineeFactory().Create() },
            };
        }

        [HttpGet("{dtoName}")]
        [Tags("Pension: Factory")]
        [OpenApi]
        public async Task<JsonAPIResponse<object>> CreateFake(FactoryEntityEnum dtoName)
        {
            JsonAPIResponse<object> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"{dtoName} received successfully",
                Result = null,
            };

            if (!_factories.ContainsKey(dtoName))
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
