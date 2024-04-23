using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MasterController : ControllerBase
    {
        private readonly ITreasuryService _treasuryService;

        public MasterController(ITreasuryService treasuryService)
        {
            _treasuryService = treasuryService;
        }
        [HttpGet("get-treasuries")]
        public async Task<APIResponse<List<DropdownStringCodeDTO>>> Treasuries()
        {
            APIResponse<List<DropdownStringCodeDTO>> response = new();  
            try
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result =  await _treasuryService.GetTreasurys();
                response.Message = "";
                return response ;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }
    }
}