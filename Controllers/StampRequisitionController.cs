using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.BAL.Interfaces.stampRequisition;
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
    public class StampRequisitionController : Controller
    {
        private readonly IStampRequisitionService _stampRequisitionService;
        public StampRequisitionController(IStampRequisitionService stampRequisitionService)
        {
            _stampRequisitionService = stampRequisitionService;
        }

        // Stamp requisition
        [HttpPatch("GetAllStampRequisitionList")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>>> StampRequisitionList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp Processed Requisitions with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>> response = new();
            try
            {
                IEnumerable<StampRequisitionDTO> indentList = await _stampRequisitionService.ListAllStampRequisitions(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<StampRequisitionDTO>> result = new DynamicListResult<IEnumerable<StampRequisitionDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name = "Vendor Stamp Requisition Id",
                            DataType = "numeric",
                            FieldName = "vendorStampRequisitionId",
                            FilterField = "VendorStampRequisitionId",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name = "Vendor Id",
                            DataType = "numeric",
                            FieldName = "vendorId",
                            FilterField = "VendorId",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name = "Vendor Name",
                            DataType = "string",
                            FieldName = "vendorName",
                            FilterField = "VendorName",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name = "Vendor Type",
                            DataType = "string",
                            FieldName = "vendorType",
                            FilterField = "VendorType",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name = "License No",
                            DataType = "string",
                            FieldName = "licenseNo",
                            FilterField = "LicenseNo",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name = "Amount",
                            DataType = "numeric",
                            FieldName = "amount",
                            FilterField = "Amount",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name = "Requisition Date",
                            DataType = "date",
                            FieldName = "requisitionDate",
                            FilterField = "RequisitionDate",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name = "Raised To Treasury",
                            DataType = "string",
                            FieldName = "raisedToTreasury",
                            FilterField = "RaisedToTreasury",
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
                            Name = "Requisition No",
                            DataType = "string",
                            FieldName = "requisitionNo",
                            FilterField = "RequisitionNo",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                         new ListHeader
                         {
                             Name ="Status",
                            DataType = "object",
                            ObjectTypeValueField="statusId",
                            FieldName = "status",
                            FilterField = "Status",
                            FilterEnums = new List<FilterEnum>
                            {
                                /*
                                DeliveredToVendor = 37*/

                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.ForwardedToStampCleck,
                                    Label = "Forwarded to Stamp Clerk",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.ForwardedToTreasuryOfficer,
                                    Label = "Forwarded to Treasury Officer",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.RejectedByStampClerk,
                                    Label = "Rejected By Stamp Clerk",
                                    StyleClass = "warning"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForPayment,
                                    Label = "Waiting For Payment",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.RejectedByTreasuryOfficer,
                                    Label = "Rejected by Treasury Officer",
                                    StyleClass = "warning"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForTreasuryOfficerVerification,
                                    Label = "Waiting For Treasury Officer Verification",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.VerifiedByTreasuryOfficer,
                                    Label = "Verified By Treasury Officer",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.DeliveredToVendor,
                                    Label = "Delivered To Vendorr",
                                    StyleClass = "success"
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

        [HttpPost("CreateStampRequisition")]
        public async Task<APIResponse<bool>> CreateStampRequisition(StampRequisitionInsertDTO stampRequisition)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampRequisition != null)
                {
                    if (await _stampRequisitionService.CreateStampRequisition(stampRequisition))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.RequisitionRaised;
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

    }
}

