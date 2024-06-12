using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Services;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.Model;
using CTS_BE.Model.Cheque;
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
        private readonly IChequeCountService _chequeCountService;
        private readonly IChequeReceivedService _chequeReceivedService;
        private readonly IChequeDistributionService _chequeDistributionService;

        public ChequeController(IChequeCountService chequeCountService, IChequeEntryService chequeEntryService, IChequeIndentService chequeIndentService, IChequeInvoiceService chequeInvoiceService, IClaimService claimService, IChequeReceivedService chequeReceivedService, IChequeDistributionService chequeDistributionService)
        {
            _chequeEntryService = chequeEntryService;
            _chequeIndentService = chequeIndentService;
            _chequeInvoiceService = chequeInvoiceService;
            _claimService = claimService;
            _chequeCountService = chequeCountService;
            _chequeReceivedService = chequeReceivedService;
            _chequeDistributionService = chequeDistributionService;
        }
        [HttpPost("new-cheque-entry")]
        public async Task<APIResponse<string>> ChequeEntry(ChequeEntryDTO chequeEntryDTO)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();
                short quantity = 0;
                quantity = (short)((chequeEntryDTO.End - chequeEntryDTO.Start) + 1);
                ChequeEntryModel chequeEntry = new ChequeEntryModel
                {
                    FinancialYearId = 1, //TODO:: Change this to active FY ID
                    TreasurieCode = chequeEntryDTO.TreasurieCode,
                    MicrCode = chequeEntryDTO.MicrCode,
                    SeriesNo = chequeEntryDTO.Series,
                    Start = chequeEntryDTO.Start,
                    End = chequeEntryDTO.End,
                    Quantity = quantity,
                    AvailableQuantity = quantity,
                    CreatedBy = userId,
                };
                if (await _chequeEntryService.Insert(chequeEntry))
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
        [HttpGet("series")]
        public async Task<APIResponse<IEnumerable<DropdownDTO>>> ChequeSeries()
        {
            APIResponse<IEnumerable<DropdownDTO>> response = new();
            try
            {

                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = await _chequeEntryService.AllSeries();
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
        [HttpGet("available-cheque-micr")]
        public async Task<APIResponse<IEnumerable<DropdownStringCodeDTO>>> TreasuryChequeMICR([FromQuery] string treasuryCode)
        {
            APIResponse<IEnumerable<DropdownStringCodeDTO>> response = new();
            try
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = await _chequeCountService.GetAvailableChequeMICRByTreasuryCode(treasuryCode);
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
        [HttpGet("series-details")]
        public async Task<APIResponse<ChequeSeriesDetailDTO>> ChequeSeriesDetils([FromQuery] long Id)
        {
            APIResponse<ChequeSeriesDetailDTO> response = new();
            try
            {

                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = await _chequeEntryService.SeriesDetailsById(Id);
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
        [HttpGet("micr-series-details")]
        public async Task<APIResponse<List<ChequeSeriesDetailDTO>>> MicrSeriesDetils([FromQuery] string MicrCode)
        {
            APIResponse<List<ChequeSeriesDetailDTO>> response = new();
            try
            {

                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = await _chequeEntryService.SeriesDetailsByMICRCode(MicrCode);
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
                            Name ="Treasurie",
                            DataType = "text",
                            FieldName = "treasurieCode",
                            FilterField = "TreasurieCode",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="MICR",
                            DataType = "text",
                            FieldName = "micrCode",
                            FilterField = "MicrCode",
                            IsFilterable = true,
                            IsSortable = true,
                        },
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
                string role = _claimService.GetRole();
                //TODO:: LOG OF INDENT NEED TO IMPLEMENT IN DATABASE
                ChequeIndentModel chequeIndentModel = new()
                {
                    IndentDate = chequeIndentDTO.IndentDate,
                    TreasurieCode = (role == "DTA") ? chequeIndentDTO.TreasurieCode : _claimService.GetScope(),
                    MemoDate = chequeIndentDTO.MemoDate,
                    MemoNumber = chequeIndentDTO.MemoNumber,
                    Remarks = chequeIndentDTO.Remarks,
                    Status = ChequeStatusManagerHelper.getStatus(role),
                    UserId = _claimService.GetUserId(),
                    ChequeIndentDeatils = chequeIndentDTO.ChequeIndentDeatils.Select(iDetils => new ChequeIndentDeatilsModel
                    {
                        ChequeType = iDetils.ChequeType,
                        MicrCode = iDetils.MicrCode,
                        Quantity = iDetils.Quantity,
                    }).ToList()
                };
                if (await _chequeIndentService.Insert(chequeIndentModel))
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
            APIResponse<DynamicListResult<IEnumerable<ChequeIndentListDTO>>> response = new();
            try
            {
                List<int> statusIds = ChequeStatusManagerHelper.getStatus(_claimService.GetPermissions());
                DynamicListResult<IEnumerable<ChequeIndentListDTO>> result = new DynamicListResult<IEnumerable<ChequeIndentListDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        // new ListHeader
                        // {
                        //     Name ="Indent Id",
                        //     DataType = "numeric",
                        //     FieldName = "indentId",
                        //     FilterField = "IndentId",
                        //     IsFilterable = true,
                        //     IsSortable = true,
                        // },
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
                            Name ="Memo Date",
                            DataType = "date",
                            FieldName = "memoDate",
                            FilterField = "MemoDate",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                                                new ListHeader
                        {
                            Name ="Cheque Type",
                            DataType = "object",
                            FieldName = "chequeType",
                            FilterField = "ChequeIndentDetails.ChequeType",
                                                        FilterEnums = new List<FilterEnum>
                            {
                                new FilterEnum
                                {
                                    Value = (int) Enum.ChequeType.TreasuryCheque,
                                    Label = "Treasury Cheque",
                                    StyleClass = "status-Global"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.ChequeType.Others,
                                    Label = "Others",
                                    StyleClass = "success"
                                },
                            },
                            IsFilterable = true,
                            IsSortable = true,
                        },
                                                new ListHeader
                        {
                            Name ="Micr Code",
                            DataType = "text",
                            FieldName = "micrCode",
                            FilterField = "ChequeIndentDetails.MicrCode",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                                                new ListHeader
                        {
                            Name ="Quantity",
                            DataType = "numeric",
                            FieldName = "quantity",
                            FilterField = "ChequeIndentDetails.Quantity",
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
                                    Value = (int) Enum.IndentStatus.ApproveByTreasuryOfficer,
                                    Label = "Approve by TO",
                                    StyleClass = "success"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.IndentStatus.RejectByTreasuryOfficer,
                                    Label = "Reject by TO",
                                    StyleClass = "warning"
                                },

                            },
                            IsFilterable=false,
                            IsSortable=false,
                            },

                    },
                    Data = await _chequeIndentService.ChequeIndentList(dynamicListQueryParameters, statusIds),
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
        [HttpPatch("cheque-invoice-list")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<ChequeInvoiceListDTO>>>> ListOfChequeInvoice(DynamicListQueryParameters dynamicListQueryParameters)
        {
            APIResponse<DynamicListResult<IEnumerable<ChequeInvoiceListDTO>>> response = new();
            try
            {
                string role = _claimService.GetRole();
                List<int> statusIds = new List<int>();
                if (role == "DTA")
                {
                    statusIds = new List<int> { (int)Enum.InvoiceStatus.NewInvoice, (int)Enum.InvoiceStatus.FrowardToTreasuryOfficer };
                }
                else
                {
                    statusIds = new List<int> { (int)Enum.InvoiceStatus.FrowardToTreasuryOfficer };
                }
                DynamicListResult<IEnumerable<ChequeInvoiceListDTO>> result = new DynamicListResult<IEnumerable<ChequeInvoiceListDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Invoice Date",
                            DataType = "date",
                            FieldName = "invoiceDate",
                            FilterField = "InvoiceDate",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Invoice Number",
                            DataType = "text",
                            FieldName = "invoiceNumber",
                            FilterField = "InvoiceNo",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Memo Number",
                            DataType = "text",
                            FieldName = "memoNumber",
                            FilterField = "ChequeIndent.MemoNo",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Quantity",
                            DataType = "numeric",
                            FieldName = "quantity",
                            FilterField = "TotalApprovedQuantity",
                            IsFilterable = true,
                            IsSortable = true,
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
                                    Value = (int) Enum.InvoiceStatus.NewInvoice,
                                    Label = "New Invoice",
                                    StyleClass = "primary"
                                },
                                new FilterEnum
                                {
                                    Value = (int) Enum.InvoiceStatus.FrowardToTreasuryOfficer,
                                    Label = "Froward to Treasury Officer",
                                    StyleClass = "success"
                                },

                            },
                            IsFilterable=true,
                            IsSortable=false,
                            },

                    },
                    Data = await _chequeInvoiceService.ChequeInvoiceList(dynamicListQueryParameters, statusIds),
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
        public async Task<APIResponse<string>> ChequeIndentApprove(IndentFrowardApproveRjectDTO indentApproveRjectDTO)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();

                var indentDetails = await _chequeIndentService.ChequeIndentByIdStatus(indentApproveRjectDTO.IndentId, (int)Enum.IndentStatus.FrowardToTreasuryOfficer);
                if (indentDetails == null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Warning;
                    response.Message = "Indent not found!";
                    return response;
                }
                if (await _chequeIndentService.ApproveRejectIndent(indentDetails, userId, (short)Enum.IndentStatus.ApproveByTreasuryOfficer))
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
        public async Task<APIResponse<string>> ChequeIndentReject(IndentFrowardApproveRjectDTO indentApproveRjectDTO)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();
                var indentDetails = await _chequeIndentService.ChequeIndentByIdStatus(indentApproveRjectDTO.IndentId, (short)Enum.IndentStatus.FrowardToTreasuryOfficer);
                if (indentDetails == null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Warning;
                    response.Message = "Indent not found!";
                    return response;
                }
                if (await _chequeIndentService.ApproveRejectIndent(indentDetails, userId, (short)Enum.IndentStatus.RejectByTreasuryOfficer))
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
        [HttpPut("cheque-indent-froward")]
        public async Task<APIResponse<string>> ChequeIndentFroward(IndentFrowardApproveRjectDTO indentApproveRjectDTO)
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
                if (await _chequeIndentService.ApproveRejectIndent(indentDetails, userId, (short)Enum.IndentStatus.FrowardToTreasuryOfficer))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Cheque indent froward successfully!";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Cheque indent froward faild!";
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }
        [HttpPost("new-cheque-invoice")]
        public async Task<APIResponse<string>> NewInvoice(ChequeInvoiceDTO chequeInvoiceDTO)
        {
            APIResponse<string> response = new();
            try
            {
                if (await _chequeInvoiceService.InsertIndentInvoice(chequeInvoiceDTO))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
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
        [HttpGet("cheque-indent")]
        public async Task<APIResponse<ChequeIndentDTO>> ChequeIndent([FromQuery] long Id)
        {
            APIResponse<ChequeIndentDTO> response = new();
            try
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = await _chequeIndentService.ChequeIndentDetailsByIdStatus(Id, (int)Enum.IndentStatus.ApproveByTreasuryOfficer);
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
        [HttpPut("cheque-invoice-froward")]
        public async Task<APIResponse<string>> ChequeInvoiceFroward(InvoiceFrowardDTO invoiceFrowardDTO)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();
                var invoiceDetails = await _chequeInvoiceService.ChequeInvoiceById(invoiceFrowardDTO.InvoiceId, (short)Enum.InvoiceStatus.NewInvoice);
                if (invoiceDetails == null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Warning;
                    response.Message = "Invoice not found!";
                    return response;
                }
                if (await _chequeInvoiceService.UpdateInvoiceStatus(invoiceDetails, (int)Enum.InvoiceStatus.FrowardToTreasuryOfficer))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Cheque invoice froward successfully!";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Cheque invoice froward faild!";
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }

        [HttpGet("getChequeInvoiceDetails")]
        public async Task<APIResponse<ChequeInvoiceDetailsByIdDTO>> getChequeInvoiceDetails([FromQuery] long invoiceId)
        {
            APIResponse<ChequeInvoiceDetailsByIdDTO> response = new();
            try
            {
                ChequeInvoiceDetailsByIdDTO invoiceDetails = await _chequeInvoiceService.ChequeInvoiceAndInvoiceDetailsById(invoiceId);
                if (invoiceDetails != null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = invoiceDetails;
                    return response;
                }
                else
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Warning;
                    response.Message = "Invoice not found!";
                    return response;
                }

            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }

        [HttpPost("cheque-received")]
        public async Task<APIResponse<string>> ChequeReceived(ChequeReceivedDTO chequeReceivedDTO)
        {
            APIResponse<string> response = new();
            try
            {
                var result = _chequeReceivedService.ChequeReceived(chequeReceivedDTO);
                if (result != null)
                {
                    await result;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Cheque received successfully!";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Cheque received failed!";
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }

        [HttpPatch("cheque-received-list")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<ChequeReceivedListDTO>>>> ListOfChequeReceived(DynamicListQueryParameters dynamicListQueryParameters)
        {
            APIResponse<DynamicListResult<IEnumerable<ChequeReceivedListDTO>>> response = new();
            try
            {
                // List<int> statusIds = ChequeStatusManagerHelper.getStatus(_claimService.GetPermissions());
                List<int> statusIds = new List<int> { 20 };
                DynamicListResult<IEnumerable<ChequeReceivedListDTO>> result = new DynamicListResult<IEnumerable<ChequeReceivedListDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name = "Invoice Details Id",
                            DataType = "numeric",
                            FieldName = "InvoiceDeatilsId",
                            FilterField = "cheque_invoice_details_id",
                            IsFilterable = true,
                            IsSortable = true,

                        },

                        new ListHeader
                        {
                            Name = "Quantity",
                            DataType = "numeric",
                            FieldName = "quantity",
                            FilterField = "quantity",
                            IsFilterable = true,
                            IsSortable = true,
                        }

                    },
                    Data = await _chequeReceivedService.ChequeReceivedList(dynamicListQueryParameters, statusIds),
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
        [HttpGet("user-list")]
        public async Task<APIResponse<IEnumerable<UserListDTO>>> UserList()
        {
            APIResponse<IEnumerable<UserListDTO>> response = new();

            try
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = await _chequeDistributionService.UserList();
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("getChequeReceivedDetails")]
        public async Task<APIResponse<IEnumerable<ChequeReceivedDataWithMICRDTO>>> getChequeReceivedDetails()
        {
            APIResponse<IEnumerable<ChequeReceivedDataWithMICRDTO>> response = new();
            try
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = await _chequeReceivedService.AllChequeReceivedList();
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("save-Cheque-Distribution")]
        public async Task<APIResponse<bool>>savechequedistribution(ChequeDistributionDTO chequeDistributionDTO)
        {
            APIResponse<bool> response = new();
            try
            {
                var result = await _chequeDistributionService.saveChequeDistribution(chequeDistributionDTO);
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = result;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
            }
                return response;
        }
    
    }
}



