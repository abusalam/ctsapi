using System.Data.Common;
using System.Diagnostics;
using System.Text.Json.Nodes;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs.PensionDTO;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace CTS_BE.Controllers
{
  [ApiController]
  [Tags("Pension")]
  [Route("api/v1/[controller]")]
  public class PensionController : ControllerBase
  {
    private readonly IPensionService _pensionService;
    private readonly IReceiptSequenceService _receiptSequenceService;
    public PensionController(IPensionService pensionService)
    {
      _pensionService = pensionService;
    }

    [HttpPost("manual-ppo-receipt")]
    [Produces("application/json")]
    [Tags("Pension", "Manual PPO Receipt")]
    public async Task<APIResponse<string>> CreateManualPpoReceipt(ManualPpoReceiptDTO manualPpoReceiptDTO)
    {
      APIResponse<string> response;

      try {
        response = new() {
          apiResponseStatus = Enum.APIResponseStatus.Success,
          result = await _pensionService
                .CreateManualPpoReceipt(manualPpoReceiptDTO),
          Message = $"PPO Received Successfully!"

        };
      } catch(DbUpdateException e) {
        StackFrame CallStack = new StackFrame(1, true);
        response = new APIResponse<string>() {
          apiResponseStatus = Enum.APIResponseStatus.Error,
          result = null,
          Message = e.ToString()
        //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
        };
      }
      return response;
    }

    [HttpGet("manual-ppo-receipt")]
    [Produces("application/json")]
    [Tags("Pension", "Manual PPO Receipt")]
    public async Task<APIResponse<ManualPpoReceiptDTO>> GetManualPpoReceipt(string treasuryReceiptNo)
    {
      APIResponse<ManualPpoReceiptDTO> response;

      try {
        response = new() {
          apiResponseStatus = Enum.APIResponseStatus.Success,
          result = await _pensionService
                .GetManualPpoReceipt(treasuryReceiptNo),
          Message = $"PPO Received Successfully!"

        };
      } catch(DbException e) {
        StackFrame CallStack = new StackFrame(1, true);
        response = new APIResponse<ManualPpoReceiptDTO>() {
          apiResponseStatus = Enum.APIResponseStatus.Error,
          result = null,
          Message = e.ToString()
        //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
        };
      }
      return response;
    }

    [HttpPost("echo")]
    [Produces("application/json")]
    [Tags("Pension", "Response Echo Request")]
    public async Task<APIResponse<Object>> EchoRequestInResponse(Object req)
    {
      APIResponse<Object> response = new()
      {
          apiResponseStatus = Enum.APIResponseStatus.Success,
          Message = "Echoing Request",
          result = req
      };
      return await Task.FromResult(response);
    }

    [HttpPost("testDateOnly")]
    public async Task<APIResponse<JsonResult>> TestSetDateOnly(DateOnly dateOnly)
    {
      APIResponse<JsonResult> response = new()
      {
          apiResponseStatus = Enum.APIResponseStatus.Success,
          Message = "Writing DateOnly",
          result = new JsonResult(dateOnly)
      };
      return await Task.FromResult(response);
    }

    [HttpGet("testDateOnly")]
    public async Task<APIResponse<DateOnly>> TestGetDateOnly()
    {
      DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
      APIResponse<DateOnly> response = new()
      {
          apiResponseStatus = Enum.APIResponseStatus.Success,
          Message = "Reading DateOnly",
          result = dateOnly
      };
      return await Task.FromResult(response);
    }
  }
}