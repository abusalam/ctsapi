using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.BAL.Services.stamp;
using CTS_BE.Common;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DTOs;
using CTS_BE.Enum;
using CTS_BE.Filters;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StampController : Controller
    {
        private readonly IStampService _stampService;

        public StampController(IStampService stampService)
        {
            _stampService = stampService;
        }

        // Stamp indent
        [HttpPatch("StampIndentListProcessed")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampIndentDTO>>>> StampIndentListProcessed(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp labels with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampIndentDTO>>> response = new();
            try
            {
                IEnumerable<StampIndentDTO> indentList = await _stampService.ListAllStampIndentsProcessed(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<StampIndentDTO>> result = new DynamicListResult<IEnumerable<StampIndentDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Stamp Indent Id",
                            DataType = "numeric",
                            FieldName = "stampIndentId",
                            FilterField = "stampIndentId",
                            IsFilterable = false,
                            IsSortable = false,
                        },
                        new ListHeader
                        {
                            Name ="Memo Number",
                            DataType = "text",
                            FieldName = "memoNumber",
                            FilterField = "MemoNumber",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Memo Date",
                            DataType = "datetime",
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
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Raised To Treasury Code",
                            DataType = "text",
                            FieldName = "raisedToTreasuryCode",
                            FilterField = "RaisedToTreasuryCodes",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Stamp Category",
                            DataType = "text",
                            FieldName = "stmapCategory",
                            FilterField = "StmapCategory",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Description",
                            DataType = "text",
                            FieldName = "description",
                            FilterField = "Description",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Denomination",
                            DataType = "decimal",
                            FieldName = "denomination",
                            FilterField = "Denomination",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Label Per Sheet",
                            DataType = "numeric",
                            FieldName = "labelPerSheet",
                            FilterField = "LabelPerSheet",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Sheet",
                            DataType = "numeric",
                            FieldName = "sheet",
                            FilterField = "Sheet",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Label",
                            DataType = "numeric",
                            FieldName = "label",
                            FilterField = "Label",
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
                        new ListHeader
                        {
                            Name ="Amount",
                            DataType = "decimal",
                            FieldName = "amount",
                            FilterField = "Amount",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created At",
                            DataType = "datetime",
                            FieldName = "createdAt",
                            FilterField = "CreatedAt",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Status",
                            DataType = "object",
                            ObjectTypeValueField="status",
                            FieldName = "status",
                            FilterField = "Status",
                            FilterEnums = new List<FilterEnum>
                            {
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ForwardedToSuperintendent,
                                    Label = "Forwarded To Superintendent",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ForwardedToTreasuryOfficer,
                                    Label = "Forwarded To Treasury Office",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ApproveBySuperintendent,
                                    Label = "Approve by Superintendent",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ApproveByTreasuryOfficer,
                                    Label = "Approve by Treasury Office",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.RejectBySuperintendent,
                                    Label = "Reject by Superintendent",
                                    StyleClass = "warning"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.RejectByTreasuryOfficer,
                                    Label = "Reject by Treasury Office",
                                    StyleClass = "warning"
                                },

                            },
                            IsFilterable = true,
                            IsSortable = true,
                        }

                    },

                    Data = indentList,
                    DataCount = 1
                };
                if (indentList.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                else
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = AppConstants.DataNotFound;
                }
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

        [HttpPatch("StampIndentListProcessing")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampIndentDTO>>>> StampIndentListProcessing(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp labels with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampIndentDTO>>> response = new();
            try
            {
                IEnumerable<StampIndentDTO> labelList = await _stampService.ListAllStampIndentsProcessing(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<StampIndentDTO>> result = new DynamicListResult<IEnumerable<StampIndentDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Stamp Indent Id",
                            DataType = "numeric",
                            FieldName = "stampIndentId",
                            FilterField = "stampIndentId",
                            IsFilterable = false,
                            IsSortable = false,
                        },
                        new ListHeader
                        {
                            Name ="Memo Number",
                            DataType = "text",
                            FieldName = "memoNumber",
                            FilterField = "MemoNumber",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Memo Date",
                            DataType = "datetime",
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
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Raised To Treasury Code",
                            DataType = "text",
                            FieldName = "raisedToTreasuryCode",
                            FilterField = "RaisedToTreasuryCodes",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Stamp Category",
                            DataType = "text",
                            FieldName = "stmapCategory",
                            FilterField = "StmapCategory",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Description",
                            DataType = "text",
                            FieldName = "description",
                            FilterField = "Description",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Denomination",
                            DataType = "decimal",
                            FieldName = "denomination",
                            FilterField = "Denomination",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Label Per Sheet",
                            DataType = "numeric",
                            FieldName = "labelPerSheet",
                            FilterField = "LabelPerSheet",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Sheet",
                            DataType = "numeric",
                            FieldName = "sheet",
                            FilterField = "Sheet",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Label",
                            DataType = "numeric",
                            FieldName = "label",
                            FilterField = "Label",
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
                        new ListHeader
                        {
                            Name ="Amount",
                            DataType = "decimal",
                            FieldName = "amount",
                            FilterField = "Amount",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created At",
                            DataType = "datetime",
                            FieldName = "createdAt",
                            FilterField = "CreatedAt",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Status",
                            DataType = "object",
                            ObjectTypeValueField="status",
                            FieldName = "status",
                            FilterField = "Status",
                            FilterEnums = new List<FilterEnum>
                            {
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ForwardedToSuperintendent,
                                    Label = "Forwarded To Superintendent",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ForwardedToTreasuryOfficer,
                                    Label = "Forwarded To Treasury Office",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ApproveBySuperintendent,
                                    Label = "Approve by Superintendent",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ApproveByTreasuryOfficer,
                                    Label = "Approve by Treasury Office",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.RejectBySuperintendent,
                                    Label = "Reject by Superintendent",
                                    StyleClass = "warning"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.RejectByTreasuryOfficer,
                                    Label = "Reject by Treasury Office",
                                    StyleClass = "warning"
                                },

                            },
                            IsFilterable = true,
                            IsSortable = true,
                        }

                    },

                    Data = labelList,
                    DataCount = 1
                };
                if (labelList.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                else
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = AppConstants.DataNotFound;
                }
                
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

        // Stamp indent
        [HttpPatch("StampIndentList")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampIndentDTO>>>> StampIndentList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp labels with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampIndentDTO>>> response = new();
            try
            {
                IEnumerable<StampIndentDTO> labelList = await _stampService.StampIndentList(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<StampIndentDTO>> result = new DynamicListResult<IEnumerable<StampIndentDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Stamp Indent Id",
                            DataType = "numeric",
                            FieldName = "stampIndentId",
                            FilterField = "stampIndentId",
                            IsFilterable = false,
                            IsSortable = false,
                        },
                        new ListHeader
                        {
                            Name ="Memo Number",
                            DataType = "text",
                            FieldName = "memoNumber",
                            FilterField = "MemoNumber",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Memo Date",
                            DataType = "datetime",
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
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Raised To Treasury Code",
                            DataType = "text",
                            FieldName = "raisedToTreasuryCode",
                            FilterField = "RaisedToTreasuryCodes",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Stamp Category",
                            DataType = "text",
                            FieldName = "stmapCategory",
                            FilterField = "StmapCategory",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Description",
                            DataType = "text",
                            FieldName = "description",
                            FilterField = "Description",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Denomination",
                            DataType = "decimal",
                            FieldName = "denomination",
                            FilterField = "Denomination",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Label Per Sheet",
                            DataType = "numeric",
                            FieldName = "labelPerSheet",
                            FilterField = "LabelPerSheet",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Sheet",
                            DataType = "numeric",
                            FieldName = "sheet",
                            FilterField = "Sheet",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Label",
                            DataType = "numeric",
                            FieldName = "label",
                            FilterField = "Label",
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
                        new ListHeader
                        {
                            Name ="Amount",
                            DataType = "decimal",
                            FieldName = "amount",
                            FilterField = "Amount",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created At",
                            DataType = "datetime",
                            FieldName = "createdAt",
                            FilterField = "CreatedAt",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Status",
                            DataType = "object",
                            ObjectTypeValueField="status",
                            FieldName = "status",
                            FilterField = "Status",
                            FilterEnums = new List<FilterEnum>
                            {
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ForwardedToSuperintendent,
                                    Label = "Forwarded To Superintendent",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ForwardedToTreasuryOfficer,
                                    Label = "Forwarded To Treasury Office",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ApproveBySuperintendent,
                                    Label = "Approve by Superintendent",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ApproveByTreasuryOfficer,
                                    Label = "Approve by Treasury Office",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.RejectBySuperintendent,
                                    Label = "Reject by Superintendent",
                                    StyleClass = "warning"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.RejectByTreasuryOfficer,
                                    Label = "Reject by Treasury Office",
                                    StyleClass = "warning"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.RecievedByTreasuryOfficer,
                                    Label = "Received by Treasury Office",
                                    StyleClass = "success"
                                },

                            },
                            IsFilterable = true,
                            IsSortable = true,
                        }

                    },

                    Data = labelList,
                    DataCount = 1
                };
                if (labelList.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                else
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = AppConstants.DataNotFound;
                }
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

        [HttpPost("CreateStampIndent")]
        public async Task<APIResponse<bool>> CreateStampIndent(StampIndentInsertDTO stampIndent)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampIndent != null)
                {
                    if(await _stampService.CreateNewStampIndent(stampIndent))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.DataAdded;
                        response.result = true;
                        return response;
                    }
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.MissingField;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        // approve

        [HttpGet("ApproveStampIndent")]
        public async Task<APIResponse<bool>> ApproveStampIndent(long stampIndentId)
        {
            APIResponse<bool> response = new();
            try
            {
                //if (stampIndentId != null)
                //{
                    if (await _stampService.ApproveStampIndent(stampIndentId))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.ApproveStatusDone;
                        response.result = true;
                        return response;
                    }
                //}
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.InsufficientBalance;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        // receive

        [HttpGet("ReceiveStampIndent")]
        public async Task<APIResponse<bool>> ReceiveStampIndent(long stampIndentId)
        {
            APIResponse<bool> response = new();
            try
            {
                //if (stampIndentId != null)
                //{
                if (await _stampService.ReceiveStampIndent(stampIndentId))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = AppConstants.ReceiveStatusDone;
                    response.result = true;
                    return response;
                }
                //}
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.ReceiveStatusFail;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }



        // reject stamp Indent
        [HttpGet("RejectStampIndent")]
        public async Task<APIResponse<bool>> RejectStampIndent(long stampIndentId)
        {
            APIResponse<bool> response = new();
            try
            {
                //if (stampIndentId != null)
                //{
                    if (await _stampService.RejectStampIndent(stampIndentId))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.RejectStatusDone;
                        response.result = true;
                        return response;
                    }
                //}
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.RejectStatusFail;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        // Stamp invoice
        [HttpPatch("StampInvoiceList")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampInvoiceDTO>>>> StampInvoiceList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp labels with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampInvoiceDTO>>> response = new();
            try
            {
                IEnumerable<StampInvoiceDTO> labelList = await _stampService.ListAllStampInvoices(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<StampInvoiceDTO>> result = new DynamicListResult<IEnumerable<StampInvoiceDTO>>
                {
                    Headers = new List<ListHeader>
                {
                    new ListHeader
                    {
                        Name = "Stamp Indent Id",
                        DataType = "numeric",
                        FieldName = "stampIndentId",
                        FilterField = "StampIndentId",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Memo Number",
                        DataType = "text",
                        FieldName = "memoNumber",
                        FilterField = "MemoNumber",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Memo Date",
                        DataType = "datetime",
                        FieldName = "memoDate",
                        FilterField = "MemoDate",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    //new ListHeader
                    //{
                    //    Name = "Remarks",
                    //    DataType = "text",
                    //    FieldName = "remarks",
                    //    FilterField = "Remarks",
                    //    IsFilterable = true,
                    //    IsSortable = true,
                    //},
                    new ListHeader
                    {
                        Name = "Stamp Invoice Id",
                        DataType = "numeric",
                        FieldName = "stampInvoiceId",
                        FilterField = "StampInvoiceId",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Invoice Number",
                        DataType = "text",
                        FieldName = "invoiceNumber",
                        FilterField = "InvoiceNumber",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Invoice Date",
                        DataType = "datetime",
                        FieldName = "invoiceDate",
                        FilterField = "InvoiceDate",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    //new ListHeader
                    //{
                    //    Name = "Raised To Treasury Code",
                    //    DataType = "text",
                    //    FieldName = "raisedToTreasuryCode",
                    //    FilterField = "RaisedToTreasuryCode",
                    //    IsFilterable = true,
                    //    IsSortable = true,
                    //},
                    new ListHeader
                    {
                        Name = "Stamp Category",
                        DataType = "text",
                        FieldName = "stmapCategory",
                        FilterField = "StmapCategory",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Description",
                        DataType = "text",
                        FieldName = "description",
                        FilterField = "Description",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Denomination",
                        DataType = "decimal",
                        FieldName = "denomination",
                        FilterField = "Denomination",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    //new ListHeader
                    //{
                    //    Name = "Label Per Sheet",
                    //    DataType = "numeric",
                    //    FieldName = "labelPerSheet",
                    //    FilterField = "LabelPerSheet",
                    //    IsFilterable = true,
                    //    IsSortable = true,
                    //},
                    //new ListHeader
                    //{
                    //    Name = "Indented Sheet",
                    //    DataType = "numeric",
                    //    FieldName = "indentedSheet",
                    //    FilterField = "IndentedSheet",
                    //    IsFilterable = true,
                    //    IsSortable = true,
                    //},
                    //new ListHeader
                    //{
                    //    Name = "Indented Label",
                    //    DataType = "numeric",
                    //    FieldName = "indentedLabel",
                    //    FilterField = "IndentedLabel",
                    //    IsFilterable = true,
                    //    IsSortable = true,
                    //},
                    new ListHeader
                    {
                        Name = "Sheet",
                        DataType = "numeric",
                        FieldName = "sheet",
                        FilterField = "Sheet",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Label",
                        DataType = "numeric",
                        FieldName = "label",
                        FilterField = "Label",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Quantity",
                        DataType = "numeric",
                        FieldName = "quantity",
                        FilterField = "Quantity",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Amount",
                        DataType = "decimal",
                        FieldName = "amount",
                        FilterField = "Amount",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                            Name ="Status",
                            DataType = "object",
                            ObjectTypeValueField="status",
                            FieldName = "status",
                            FilterField = "Status",
                            FilterEnums = new List<FilterEnum>
                            {
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ForwardedToSuperintendent,
                                    Label = "Forwarded To Superintendent",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ForwardedToTreasuryOfficer,
                                    Label = "Forwarded To Treasury Office",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ApproveBySuperintendent,
                                    Label = "Approve by Superintendent",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.ApproveByTreasuryOfficer,
                                    Label = "Approve by Treasury Office",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.RejectBySuperintendent,
                                    Label = "Reject by Superintendent",
                                    StyleClass = "warning"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampIndentStatusEnum.RejectByTreasuryOfficer,
                                    Label = "Reject by Treasury Office",
                                    StyleClass = "warning"
                                },

                            }
                    },
                    
                    //new ListHeader
                    //{
                    //    Name = "Created By",
                    //    DataType = "numeric",
                    //    FieldName = "createdBy",
                    //    FilterField = "CreatedBy",
                    //    IsFilterable = true,
                    //    IsSortable = true,
                    //},
                },
                    Data = labelList,
                    DataCount = 1
                };
                if (labelList.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                else
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = AppConstants.DataNotFound;
                }
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

        [HttpPost("CreateStampInvoice")]
        public async Task<APIResponse<bool>> CreateStampInvoice(StampInvoiceInsertDTO stampInvoice)
        {
            APIResponse<bool> response = new();
            try
            {
                Console.WriteLine(" " + stampInvoice.Amount, stampInvoice.Sheet, stampInvoice.Label, stampInvoice.InvoiceDate, stampInvoice.StampIndentId, stampInvoice.Quantity, stampInvoice.InvoiceNumber);

                if (stampInvoice != null)
                {
                    if (await _stampService.CreateNewStampInvoice(stampInvoice))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.DataAdded;
                        response.result = true;
                        return response;
                    } else
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Error;
                        response.result = false;
                        response.Message = AppConstants.InsufficientBalance;
                        return response;
                    }
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.MissingField;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }


        [HttpGet("IndentDetailsById")]
        public async Task<APIResponse<StampIndentDTO>> IndentDetailsById(long id)
        {
            APIResponse<StampIndentDTO> response = new();
            try
            {
                StampIndentDTO stampIndent = await _stampService.GetStampIndentById(id);
                if (stampIndent != null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = stampIndent;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = AppConstants.DataNotFound;
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

