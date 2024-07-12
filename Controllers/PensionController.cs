using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs.PensionDTO;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NuGet.Protocol;

namespace CTS_BE.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class PensionController : ControllerBase
  {
    private readonly IPensionService _pensionService;
    public PensionController(IPensionService pensionService)
    {
      _pensionService = pensionService;
    }

    [HttpPost("pensioner")]
    public async Task<IActionResult> CreatePensionerDetails(PensionerDetailsDTO pensionerDetailsDTO)
    {
      return Ok(await _pensionService.CreatePensionerDetails(pensionerDetailsDTO));
    }

    [HttpPost("testDateOnly")]
    public async Task<APIResponse<JsonResult>> TestDateOnly(DateOnly dateOnly)
    {
      dateOnly = DateOnly.FromDateTime(DateTime.Now);
      APIResponse<JsonResult> response = new();
      response.apiResponseStatus=Enum.APIResponseStatus.Success;
      response.Message = "Testing DateOnly";
      response.result = new JsonResult(dateOnly);
      return await Task.FromResult(response);
    }
  }
}