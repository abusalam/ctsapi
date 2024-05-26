﻿using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.BAL.Services.stamp;
using CTS_BE.Common;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DTOs;
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
        [HttpPatch("StampIndentList")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampIndentDTO>>>> StampIndentList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp labels with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampIndentDTO>>> response = new();
            try
            {
                IEnumerable<StampIndentDTO> labelList = await _stampService.ListAllStampIndents(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
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
                            DataType = "string",
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
                            DataType = "string",
                            FieldName = "remarks",
                            FilterField = "Remarks",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Raised To Treasury Code",
                            DataType = "string",
                            FieldName = "raisedToTreasuryCode",
                            FilterField = "RaisedToTreasuryCodes",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Stamp Category",
                            DataType = "string",
                            FieldName = "stmapCategory",
                            FilterField = "StmapCategory",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Description",
                            DataType = "string",
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
                            DataType = "numeric",
                            FieldName = "status",
                            FilterField = "Status",
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
                        DataType = "string",
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
                    new ListHeader
                    {
                        Name = "Remarks",
                        DataType = "string",
                        FieldName = "remarks",
                        FilterField = "Remarks",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Raised To Treasury Code",
                        DataType = "string",
                        FieldName = "raisedToTreasuryCode",
                        FilterField = "RaisedToTreasuryCode",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Stamp Category",
                        DataType = "string",
                        FieldName = "stmapCategory",
                        FilterField = "StmapCategory",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Description",
                        DataType = "string",
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
                    new ListHeader
                    {
                        Name = "Label Per Sheet",
                        DataType = "numeric",
                        FieldName = "labelPerSheet",
                        FilterField = "LabelPerSheet",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Indented Sheet",
                        DataType = "numeric",
                        FieldName = "indentedSheet",
                        FilterField = "IndentedSheet",
                        IsFilterable = true,
                        IsSortable = true,
                    },
                    new ListHeader
                    {
                        Name = "Indented Label",
                        DataType = "numeric",
                        FieldName = "indentedLabel",
                        FilterField = "IndentedLabel",
                        IsFilterable = true,
                        IsSortable = true,
                    },
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
                        Name = "Status",
                        DataType = "numeric",
                        FieldName = "status",
                        FilterField = "Status",
                        IsFilterable = true,
                        IsSortable = true,
                    },
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
                        DataType = "string",
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
                    new ListHeader
                    {
                        Name = "Created By",
                        DataType = "numeric",
                        FieldName = "createdBy",
                        FilterField = "CreatedBy",
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

    }
}

