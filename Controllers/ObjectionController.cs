using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Interfaces.master;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ObjectionController : Controller
    {
        protected readonly IGobalObjectionService _gobalObjectionService;
        protected readonly ILocalObjectionService _localObjectionService;
        protected readonly ITokenHasObjectionService _tokenHasObjectionService;

        public ObjectionController(IGobalObjectionService gobalObjectionService, ILocalObjectionService localObjectionService, ITokenHasObjectionService tokenHasObjectionService)
        {
            _gobalObjectionService = gobalObjectionService;
            _localObjectionService = localObjectionService;
            _tokenHasObjectionService = tokenHasObjectionService;
            _tokenHasObjectionService = tokenHasObjectionService;
        }
        [HttpGet("GetGobalObjections")]
        public async Task<APIResponse<List<ObjectionDto>>> GobalObjections()
        {
            APIResponse<List<ObjectionDto>> response = new();
            try
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = await _gobalObjectionService.AllObjections();
                response.Message = "Data Collect Successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpGet("GetLocalObjections")]
        public async Task<APIResponse<List<ObjectionDto>>> LocalObjections()
        {
            APIResponse<List<ObjectionDto>> response = new();
            try
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = await _localObjectionService.AllObjections();
                response.Message = "Data Collect Successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpGet("GetTokenObjections/{tokenId}")]
        public async Task<APIResponse<List<TokenWithObjectionDto>>> TokenObjections(long tokenId)
        {
            APIResponse<List<TokenWithObjectionDto>> response = new();
            try
            {
                List<TokenWithObjectionDto> tokenWithObjections = await _tokenHasObjectionService.ObjectionByTokenId(tokenId);
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = tokenWithObjections;
                response.Message = "Data Collect Successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpPut("NewLocalObjection")]
        public async Task<APIResponse<string>> NewLocalObjection(NewObjectionDto newObjectionDto)
        {
            APIResponse<string> response = new();
            try
            {
                if (await _localObjectionService.Insert(newObjectionDto.description))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Objection added successfully";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Objection added failed";
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
