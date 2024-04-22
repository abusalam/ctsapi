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
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
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
        [HttpPatch("cheque-indent-list")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<ChequeIndentListDTO>>>> ListOfChequeIndent(DynamicListQueryParameters dynamicListQueryParameters)
        {
            APIResponse < DynamicListResult < IEnumerable <ChequeIndentListDTO>>> response = new();
            try
            {

                DynamicListResult<IEnumerable<ChequeIndentListDTO>> result = new DynamicListResult<IEnumerable<ChequeIndentListDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Indent Id",
                            DataType = "numeric",
                            FieldName = "indentId",
                            FilterField = "IndentId",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Indent Date",
                            DataType = "date",
                            FieldName = "indentDate",
                            FilterField = "IndentDate",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Memo Number",
                            DataType = "text",
                            FieldName = "memoNo",
                            FilterField = "MemoNo",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="MemoDate",
                            DataType = "date",
                            FieldName = "memoDate",
                            FilterField = "MemoDate",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Remarks",
                            DataType = "text",
                            FieldName = "remarks",
                            FilterField = "Remarks",
                            IsFilterable = true,
                            IsSortable = false,
                        },
                        new ListHeader
                        {
                            Name ="Status",
                            DataType="object",
                            ObjectTypeValueField="currentStatusId",
                            FieldName ="currentStatus",
                            FilterField ="Status",
                            FilterEnums = new List<FilterEnum>
                            {
                                new FilterEnum
                                {
                                    Value = (int) Enum.IndentStatus.NewIndent,
                                    Label = "New Indent",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.IndentStatus.ApproveByTO,
                                    Label = "Approve by TO",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.IndentStatus.RejectByTO,
                                    Label = "Reject by TO",
                                    StyleClass = "warning"
                                },

                            },
                            IsFilterable=true,
                            IsSortable=false,
                            },

                    },
                    Data = await _chequeIndentService.ChequeIndentList(dynamicListQueryParameters),
                    DataCount = 1
                };
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message ="";
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
        [HttpPatch("cheque-invoice-list")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<ChequeIndentListDTO>>>> ListOfChequeInvoice(DynamicListQueryParameters dynamicListQueryParameters)
        {
            APIResponse<DynamicListResult<IEnumerable<ChequeIndentListDTO>>> response = new();
            try
            {

                DynamicListResult<IEnumerable<ChequeIndentListDTO>> result = new DynamicListResult<IEnumerable<ChequeIndentListDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Indent Id",
                            DataType = "numeric",
                            FieldName = "indentId",
                            FilterField = "IndentId",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Indent Date",
                            DataType = "date",
                            FieldName = "indentDate",
                            FilterField = "IndentDate",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Memo Number",
                            DataType = "text",
                            FieldName = "memoNo",
                            FilterField = "MemoNo",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="MemoDate",
                            DataType = "date",
                            FieldName = "memoDate",
                            FilterField = "MemoDate",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Remarks",
                            DataType = "text",
                            FieldName = "remarks",
                            FilterField = "Remarks",
                            IsFilterable = true,
                            IsSortable = false,
                        },
                        new ListHeader
                        {
                            Name ="Status",
                            DataType="object",
                            ObjectTypeValueField="currentStatusId",
                            FieldName ="currentStatus",
                            FilterField ="Status",
                            FilterEnums = new List<FilterEnum>
                            {
                                new FilterEnum
                                {
                                    Value = (int) Enum.IndentStatus.NewIndent,
                                    Label = "New Indent",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.IndentStatus.ApproveByTO,
                                    Label = "Approve by TO",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.IndentStatus.RejectByTO,
                                    Label = "Reject by TO",
                                    StyleClass = "warning"
                                },

                            },
                            IsFilterable=true,
                            IsSortable=false,
                            },

                    },
                    Data = await _chequeIndentService.ChequeIndentList(dynamicListQueryParameters, (int)Enum.IndentStatus.ApproveByTO),
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
        [HttpPut("cheque-indent-approve")]
        public async Task<APIResponse<string>> ChequeIndentApprove(IndentApproveRjectDTO indentApproveRjectDTO)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();

                var indentDetails = await _chequeIndentService.ChequeIndentByIdStatus(indentApproveRjectDTO.IndentId, (int)Enum.IndentStatus.NewIndent);
                if (indentDetails == null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Warning;
                    response.Message = "Indent not found!";
                    return response;
                }
                if (await _chequeIndentService.ApproveRejectIndent(indentDetails,userId,(short)Enum.IndentStatus.ApproveByTO))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
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
        public async Task<APIResponse<string>> ChequeIndentReject(IndentApproveRjectDTO indentApproveRjectDTO)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();
                var indentDetails = await _chequeIndentService.ChequeIndentByIdStatus(indentApproveRjectDTO.IndentId, (short)Enum.IndentStatus.NewIndent);
                if (indentDetails == null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Warning;
                    response.Message = "Indent not found!";
                    return response;
                }
                if (await _chequeIndentService.ApproveRejectIndent( indentDetails,userId, (short)Enum.IndentStatus.RejectByTO))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
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
        [HttpGet("cheque-indent/{id}")]
        public async Task<APIResponse<List<ChequeIndentDTO>>> ChequeIndent(long id)
        {
            APIResponse<List<ChequeIndentDTO>> response = new();
            try
            {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = await _chequeIndentService.ChequeIndentDetailsByIdStatus(id, (int)Enum.IndentStatus.ApproveByTO);
                    response.Message = "";
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
