using CTS_BE.BAL.Interfaces.stamp;
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
                            Name ="Raised By Treasury",
                            DataType = "string",
                            FieldName = "raisedByTreasury",
                            FilterField = "RaisedByTreasury",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Raised To Treasury",
                            DataType = "string",
                            FieldName = "raisedToTreasury",
                            FilterField = "RaisedToTreasury",
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

        
    }
}

