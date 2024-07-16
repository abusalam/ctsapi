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
            //Get all stamp Requisitions Requisitions with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>> response = new();
            try
            {
                IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionService.ListAllStampRequisitions(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
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
                            Name = "Treasury",
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
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForPaymentVerification,
                                    Label = "Waiting For Payment Verification",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForDelivery,
                                    Label = "Waiting For Delivery",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.DeliveredToVendor,
                                    Label = "Delivered To Vendor",
                                    StyleClass = "success"
                                },

                            },
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },

                    Data = stampRequisitionList,
                    DataCount = 1
                };
                if (stampRequisitionList.Count() > 0)
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

        [HttpPatch("GetAllStampRequisitionWaitingForPaymentVerificatonByTO")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>>> GetAllStampRequisitionWaitingForPaymentVerificatonByTO(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp Requisitions Requisitions with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>> response = new();
            try
            {
                IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionService.ListAllStampRequisitionsWaitingForPaymentVerification(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
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
                            Name = "Treasury",
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
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForPaymentVerification,
                                    Label = "Waiting For Payment Verification",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForDelivery,
                                    Label = "Waiting For Delivery",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.DeliveredToVendor,
                                    Label = "Delivered To Vendor",
                                    StyleClass = "success"
                                },

                            },
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },

                    Data = stampRequisitionList,
                    DataCount = 1
                };
                if (stampRequisitionList.Count() > 0)
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

        [HttpPost("StampRequisitionApprovedByStampClerk")]
        public async Task<APIResponse<bool>> StampRequisitionApprovedByStampClerk(StampRequisitionApprovedByClerkDTO stampRequisition)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampRequisition != null)
                {
                    if (await _stampRequisitionService.RequisitionApprovedByStampClerk(stampRequisition))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.ForwardedToTreasuryOfficer;
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

        [HttpGet("StampRequisitionRejectedByStampClerk")]
        public async Task<APIResponse<bool>> StampRequisitionRejectedByStampClerk(long stampRequisitionId)
        {
            APIResponse<bool> response = new();
            try
            {
                if (await _stampRequisitionService.RequisitionRejectedByStampClerk(stampRequisitionId))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = AppConstants.ForwardedToTreasuryOfficer;
                    response.result = true;
                    return response;
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

        [HttpPost("StampRequisitionApprovedByTreasuryOfficer")]
        public async Task<APIResponse<bool>> StampRequisitionApprovedByTreasuryOfficer(StampRequisitionApprovedByTODTO stampRequisition)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampRequisition != null)
                {
                    if (await _stampRequisitionService.RequisitionApprovedByTO(stampRequisition))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.PaymentRequired;
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

        [HttpGet("StampRequisitionRejectedByTreasuryOfficer")]
        public async Task<APIResponse<bool>> StampRequisitionRejectedByTreasuryOfficer(long stampRequisitionId)
        {
            APIResponse<bool> response = new();
            try
            {
                if (await _stampRequisitionService.RequisitionRejectedByTO(stampRequisitionId))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = AppConstants.ForwardedToTreasuryOfficer;
                    response.result = true;
                    return response;
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

        [HttpGet("StampRequisitionDeliveredToVendor")]
        public async Task<APIResponse<bool>> StampRequisitionDeliveredToVendor(long stampRequisitionId)
        {
            APIResponse<bool> response = new();
            try
            {
                if (await _stampRequisitionService.DeliveredByDEO(stampRequisitionId))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = AppConstants.ForwardedToTreasuryOfficer; // todo
                    response.result = true;
                    return response;
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

        [HttpGet("PaymentProcessByDEO")]
        public async Task<APIResponse<bool>> PaymentProcessByDEO(StampRequisitionPaymentDTO stampRequisition)
        {
            APIResponse<bool> response = new();
            try
            {
                if (await _stampRequisitionService.PaymentRegisterByDEO(stampRequisition))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Payment Details Entered."; // todo
                    response.result = true;
                    return response;
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

        [HttpPatch("GetAllStampRequisitionListWaitingForApprovalByTO")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>>> GetAllStampRequisitionListWaitingForApprovalByTO(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp Requisitions Requisitions with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>> response = new();
            try
            {
                IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionService.ListAllStampRequisitionsWaitingForApprovalByTO(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
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
                            Name = "Treasury",
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
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForPaymentVerification,
                                    Label = "Waiting For Payment Verification",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForDelivery,
                                    Label = "Waiting For Delivery",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.DeliveredToVendor,
                                    Label = "Delivered To Vendor",
                                    StyleClass = "success"
                                },

                            },
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },

                    Data = stampRequisitionList,
                    DataCount = 1
                };
                if (stampRequisitionList.Count() > 0)
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

        [HttpPatch("GetAllStampRequisitionListForClerk")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>>> GetAllStampRequisitionListForClerk(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp Requisitions Requisitions with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>> response = new();
            try
            {
                IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionService.ListAllStampRequisitionsForClerk(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
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
                            Name = "Treasury",
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
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForPaymentVerification,
                                    Label = "Waiting For Payment Verification",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForDelivery,
                                    Label = "Waiting For Delivery",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.DeliveredToVendor,
                                    Label = "Delivered To Vendor",
                                    StyleClass = "success"
                                },

                            },
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },

                    Data = stampRequisitionList,
                    DataCount = 1
                };
                if (stampRequisitionList.Count() > 0)
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

        [HttpPatch("GetAllStampRequisitionListForDelivery")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>>> GetAllStampRequisitionListForDelivery(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp Requisitions Requisitions with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>> response = new();
            try
            {
                IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionService.ListAllStampRequisitionsWaitingForDelivery(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
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
                            Name = "Treasury",
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
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForPaymentVerification,
                                    Label = "Waiting For Payment Verification",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForDelivery,
                                    Label = "Waiting For Delivery",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.DeliveredToVendor,
                                    Label = "Delivered To Vendor",
                                    StyleClass = "success"
                                },

                            },
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },

                    Data = stampRequisitionList,
                    DataCount = 1
                };
                if (stampRequisitionList.Count() > 0)
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

        [HttpPatch("GetAllStampRequisitionWaitingForPayment")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>>> GetAllStampRequisitionWaitingForPayment(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp Requisitions Requisitions with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampRequisitionDTO>>> response = new();
            try
            {
                IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionService.ListAllStampRequisitionsWaitingForPayment(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
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
                            Name = "Treasury",
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
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForPaymentVerification,
                                    Label = "Waiting For Payment Verification",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.WaitingForDelivery,
                                    Label = "Waiting For Delivery",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.StampRequisitionStatusEnum.DeliveredToVendor,
                                    Label = "Delivered To Vendor",
                                    StyleClass = "success"
                                },

                            },
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },

                    Data = stampRequisitionList,
                    DataCount = 1
                };
                if (stampRequisitionList.Count() > 0)
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
            
        [HttpGet("TrFromGenerationDataByRequisitionId")]
        public async Task<APIResponse<TRFormDataDTO>> TrFromGenerationDataByRequisitionId(long stampRequisitionId)
        {
            APIResponse<TRFormDataDTO> response = new();
            try
            {
                var data = await _stampRequisitionService.TrFromGenerationData(stampRequisitionId);
                if (data.Amount != 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = AppConstants.DataFound; 
                    response.result = data;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = null;
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

