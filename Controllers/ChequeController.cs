using CTS_BE.BAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ChequeController : Controller
    {
        private readonly IChequeEntryService _chequeEntryService;
        private readonly IChequeIndentService _chequeIndentService;
        private readonly IChequeInvoiceService _chequeInvoiceService;
        private readonly IClaimService _claimService;

        public ChequeController(IChequeEntryService chequeEntryService, IChequeIndentService chequeIndentService, IChequeInvoiceService chequeInvoiceService, IClaimService claimService)
        {
            _chequeEntryService = chequeEntryService;
            _chequeIndentService = chequeIndentService;
            _chequeInvoiceService = chequeInvoiceService;
            _claimService = claimService;
        }
        [HttpPost("new-cheque-entry")]
        public async Task<APIResponse<string>> ChequeEntry(ChequeEntryDTO chequeEntryDTO)
        {
            APIResponse<string> response = new();
            try
            {
               short quantity = (short)(chequeEntryDTO.End - chequeEntryDTO.Start);
               if(await _chequeEntryService.Insert(chequeEntryDTO.Series, chequeEntryDTO.Start, chequeEntryDTO.End, quantity))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Cheque entry successfully.";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Cheque entry faild!";
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }
        [HttpPatch("all-cheques")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<ChequeListDTO>>>> ChequeList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            APIResponse<DynamicListResult<IEnumerable<ChequeListDTO>>> response = new();
            try
            {
                IEnumerable<ChequeListDTO> chequeList = await _chequeEntryService.List(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<ChequeListDTO>> result = new DynamicListResult<IEnumerable<ChequeListDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Series",
                            DataType = "text",
                            FieldName = "series",
                            FilterField = "Series",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Start",
                            DataType = "numeric",
                            FieldName = "start",
                            FilterField = "Start",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="End",
                            DataType = "numeric",
                            FieldName = "end",
                            FilterField = "End",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Quantity",
                            DataType = "numeric",
                            FieldName = "quantity",
                            FilterField = "Quantity",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                    },
                    Data = chequeList,
                    DataCount = 1
                };
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = "";
                response.result = result;
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }
        [HttpPost("cheque-indent")]
        public async Task<APIResponse<string>> ChequeIndent(ChequeIndentDTO chequeIndentDTO)
        {
            APIResponse<string> response = new();
            try
            {
                if (await _chequeIndentService.Insert(chequeIndentDTO))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "Cheque indent successfully!";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Cheque indent faild!";
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }
        [HttpPut("cheque-indent-approve")]
        public async Task<APIResponse<string>> ChequeIndentApprove(long indentId)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();
                if (await _chequeIndentService.ApproveRejectIndent(userId,(short)Enum.IndentStatus.ApproveByTO))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "Cheque indent approve successfully!";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Cheque indent approve faild!";
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }
        [HttpPut("cheque-indent-reject")]
        public async Task<APIResponse<string>> ChequeIndentReject(long indentId)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();
                if (await _chequeIndentService.ApproveRejectIndent(userId, (short)Enum.IndentStatus.RejectByTO))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "Cheque indent reject successfully!";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Cheque indent reject faild!";
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }
        [HttpPost("cheque-indent-invoice")]
        public async Task<APIResponse<string>> ChequeIndentApprove(ChequeInvoiceDTO  chequeInvoiceDTO)
        {
            APIResponse<string> response = new();
            try
            {
                if (await _chequeInvoiceService.InsertIndentInvoice(chequeInvoiceDTO))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "Cheque invoice successfully!";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Cheque invoice faild!";
                return response;
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
