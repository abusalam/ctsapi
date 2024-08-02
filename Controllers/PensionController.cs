using System.Runtime;
using System.Data.Common;
using System.Diagnostics;
using System.Text.Json.Nodes;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.PensionEnum;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using NPOI.DDF;
using System.Dynamic;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Tags("Pension")]
    [Route("api/v1")]
    public class PensionController : ApiBaseController
    {
        private readonly IPensionService _pensionService;
        private readonly IPensionStatusService _pensionStatusService;
        private readonly IPensionerDetailsService _pensionerDetailsService;
        private readonly IPensionerBankAccountService _pensionerBankAccountService;
        private readonly IPensionCategoryService _pensionCategoryService;
        private readonly IPensionBillService _pensionBillService;
        private readonly IPensionBreakupService _pensionBreakupService;
        private readonly IPensionRateService _pensionRateService;

        public PensionController(
                IClaimService claimService,
                IPensionService pensionService,
                IPensionStatusService pensionStatusService,
                IPensionerDetailsService pensionerDetailsService,
                IPensionerBankAccountService pensionerBankAccountService,
                IPensionCategoryService pensionCategoryService,
                IPensionBillService pensionBillService,
                IPensionBreakupService pensionBreakupService,
                IPensionRateService pensionRateService
            ) : base(claimService)
        {
            _pensionService = pensionService;
            _pensionStatusService = pensionStatusService;
            _pensionerDetailsService = pensionerDetailsService;
            _pensionerBankAccountService = pensionerBankAccountService;
            _pensionCategoryService = pensionCategoryService;
            _pensionBillService = pensionBillService;
            _pensionBreakupService = pensionBreakupService;
            _pensionRateService = pensionRateService;
        }

        [HttpPost("manual-ppo/receipts")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Manual PPO Receipt")]
        public async Task<APIResponse<ManualPpoReceiptResponseDTO>> ControlManualPpoReceiptsCreate(
                ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO
            )
        {
            APIResponse<ManualPpoReceiptResponseDTO> response;

            try {
                response = new() {
                apiResponseStatus = Enum.APIResponseStatus.Success,
                result = await _pensionService
                        .CreatePpoReceipt(
                            manualPpoReceiptEntryDTO,
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                            ),
                Message = $"PPO Received Successfully!"
                };
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new StackFrame(1, true);
                response = new () {
                apiResponseStatus = Enum.APIResponseStatus.Error,
                result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }

        [HttpGet("manual-ppo/receipts/{treasuryReceiptNo}")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Manual PPO Receipt")]
        public async Task<APIResponse<ManualPpoReceiptResponseDTO>> ControlManualPpoReceiptsRead(string treasuryReceiptNo)
        {
            APIResponse<ManualPpoReceiptResponseDTO> response;

            try {
                response = new() {
                apiResponseStatus = Enum.APIResponseStatus.Success,
                result = await _pensionService
                        .GetPpoReceipt(treasuryReceiptNo),
                Message = $"PPO Received Successfully!"

                };
            } catch(DbException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                    apiResponseStatus = Enum.APIResponseStatus.Error,
                    result = null,
                    Message = e.ToString()
                    //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }

        [HttpPatch("manual-ppo/receipts")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Manual PPO Receipt")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<ListAllPpoReceiptsResponseDTO>>>> ControlManualPpoReceiptsList(DynamicListQueryParameters dynamicListQueryParameters) {
            APIResponse<DynamicListResult<IEnumerable<ListAllPpoReceiptsResponseDTO>>> response;
            try {

                response = new() {

                    apiResponseStatus = Enum.APIResponseStatus.Success,
                    result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Treasury Receipt No",
                                    DataType = "text",
                                    FieldName = "treasuryReceiptNo",
                                    FilterField = "treasuryReceiptNo",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "PPO No",
                                    DataType = "text",
                                    FieldName = "ppoNo",
                                    FilterField = "ppoNo",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Name of Pensioner",
                                    DataType = "text",
                                    FieldName = "pensionerName",
                                    FilterField = "pensionerName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Date of Receipt",
                                    DataType = "text",
                                    FieldName = "receiptDate",
                                    FilterField = "receiptDate",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionService.GetAllPpoReceipts(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionService.DataCount()
                        },
                    Message = $"All PPO Receipts Received Successfully!"

                };
            } catch(DbException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                apiResponseStatus = Enum.APIResponseStatus.Error,
                result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }

        [HttpPut("manual-ppo/receipts/{treasuryReceiptNo}")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Manual PPO Receipt")]
        public async Task<APIResponse<ManualPpoReceiptResponseDTO>> ControlManualPpoReceiptsUpdate(string treasuryReceiptNo, ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO)
        {
            APIResponse<ManualPpoReceiptResponseDTO> response;

            try {
                response = new() {
                apiResponseStatus = Enum.APIResponseStatus.Success,
                result = await _pensionService
                        .UpdatePpoReceipt(treasuryReceiptNo, manualPpoReceiptEntryDTO),
                Message = $"PPO Receipt Updated Successfully!"
                };
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new StackFrame(1, true);
                response = new () {
                apiResponseStatus = Enum.APIResponseStatus.Error,
                result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }



        [HttpPost("ppo/status")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: PPO Status")]
        public async Task<APIResponse<PensionStatusEntryDTO>> ControlStatusFlagCreate(PensionStatusEntryDTO pensionStatusEntryDTO) {

            APIResponse<PensionStatusEntryDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "|",
                result = await _pensionStatusService.SetPensionStatusFlag(
                    pensionStatusEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                )
            };

            if(System.Enum.TryParse<PensionStatusFlag>(
                    $"{pensionStatusEntryDTO.StatusFlag}",
                    out PensionStatusFlag pensionStatus
                )
            ) {
                if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionBillGenerated))
                {
                    response.Message += " First Pension Generated |";
                }
                if(pensionStatus.HasFlag(PensionStatusFlag.PpoApproved))
                {
                    response.Message += " PPO Approved |";
                }
                if(pensionStatus.HasFlag(PensionStatusFlag.PpoRunning))
                {
                    response.Message += " PPO Running |";
                }
                if(pensionStatus.HasFlag(PensionStatusFlag.PpoSuspended))
                {
                    response.Message += " PPO Suspended |";
                }
            }

            return response;
        }

        [HttpDelete("ppo/{ppoId}/status/{statusFlag}")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: PPO Status")]
        public async Task<APIResponse<PensionStatusDTO>> ControlStatusFlagDelete(int ppoId, int statusFlag) {

            APIResponse<PensionStatusDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Reset: |",
                result = new(){
                    StatusFlag = 0
                }
            };
            try {
                response.result = await _pensionStatusService.ClearPensionStatusFlag(
                    ppoId,
                    statusFlag,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
                if(System.Enum.TryParse<PensionStatusFlag>(
                        $"{response.result.StatusFlag}",
                        out PensionStatusFlag pensionStatus
                    )
                ) {
                    if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionBillGenerated))
                    {
                        response.Message += " First Pension Generated Flag |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoApproved))
                    {
                        response.Message += " PPO Approved Flag |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoRunning))
                    {
                        response.Message += " PPO Running Flag |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoSuspended))
                    {
                        response.Message += " PPO Suspended Flag |";
                    }
                }
            }
            finally {
                if(response.result.StatusFlag==0){
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"PPO status flag not found!";
                }
            }

            return response;
        }

        [HttpGet("ppo/{ppoId}/status/{statusFlag}")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: PPO Status")]
        public async Task<APIResponse<PensionStatusDTO>> ControlStatusFlagRead(int ppoId, int statusFlag) {
            APIResponse<PensionStatusDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "|",
                result = new(){
                    StatusFlag = 0
                }
            };
            try {
                response.result = await _pensionStatusService.CheckPensionStatusFlag(
                        ppoId,
                        statusFlag,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );

                if(System.Enum.TryParse<PensionStatusFlag>(
                        $"{response.result.StatusFlag}",
                        out PensionStatusFlag pensionStatus
                    )
                ) {
                    if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionBillGenerated))
                    {
                        response.Message += " First Pension Generated |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoApproved))
                    {
                        response.Message += " PPO Approved |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoRunning))
                    {
                        response.Message += " PPO Running |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoSuspended))
                    {
                        response.Message += " PPO Suspended |";
                    }
                }
            }
            finally {
                if(response.result.StatusFlag==0){
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"PPO status flag not found!";
                }
            }
            return response;
        }
    

    
        [HttpPost("ppo/details")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: PPO Details")]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> ControlPensionerDetailsCreate(
                PensionerEntryDTO pensionerEntryDTO
            )
        {

            JsonAPIResponse<PensionerResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Details saved sucessfully!",
                Result = new(){
                    PpoId = 0
                }
            };
            try {
                response.Result = await _pensionerDetailsService.CreatePensioner(
                    pensionerEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (DbUpdateException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"PPO Details not saved! Error: {ex.Message}";
            }
            finally {
                if(response.Result.PpoId == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"Error: PPO Details not saved!";
                }
            }

            return response;
        }

        [HttpPut("ppo/{ppoId}/details")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: PPO Details")]
        public async Task<APIResponse<PensionerResponseDTO>> ControlPensionerDetailsUpdate(
                int ppoId,
                PensionerEntryDTO pensionerEntryDTO
            )
        {

            APIResponse<PensionerResponseDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Details saved sucessfully!",
                result = new(){
                    PpoId = 0
                }
            };
            try {
                response.result = await _pensionerDetailsService.UpdatePensioner(
                    ppoId,
                    pensionerEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (DbUpdateException ex) {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"PPO Details not saved! Error: {ex.Message}";
            }
            finally {
                if(response.result.PpoId == 0) {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"Error: PPO Details not saved!";
                }
            }

            return response;
        }

        [HttpGet("ppo/{ppoId}/details")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: PPO Details")]
        public async Task<APIResponse<PensionerResponseDTO>> ControlPensionerDetailsRead(
                int ppoId
            )
        {

            APIResponse<PensionerResponseDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Details received sucessfully!",
                result = new(){
                    PpoId = 0
                }
            };
            try {
                response.result = await _pensionerDetailsService.GetPensioner<PensionerResponseDTO>(
                    ppoId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (DbUpdateException ex) {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"PPO Details not received! Error: {ex.Message}";
            }
            finally {
                if(response.result.PpoId == 0) {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"PPO Details not received!";
                }
            }

            return response;
        }

        [HttpPatch("ppo/details")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: PPO Details")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<PensionerListDTO>>>> ControlPensionerDetailsList(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            APIResponse<DynamicListResult<IEnumerable<PensionerListDTO>>> response;
            try {

                response = new() {

                    apiResponseStatus = Enum.APIResponseStatus.Success,
                    result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "PPO ID",
                                    DataType = "text",
                                    FieldName = "ppoId",
                                    FilterField = "ppoId",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "Name of Pensioner",
                                    DataType = "text",
                                    FieldName = "pensionerName",
                                    FilterField = "pensionerName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Mobile",
                                    DataType = "text",
                                    FieldName = "mobileNumber",
                                    FilterField = "mobileNumber",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Date of Birth",
                                    DataType = "text",
                                    FieldName = "dateOfBirth",
                                    FilterField = "dateOfBirth",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Date of Retirement",
                                    DataType = "text",
                                    FieldName = "dateOfRetirement",
                                    FilterField = "dateOfRetirement",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "PPO No",
                                    DataType = "text",
                                    FieldName = "ppoNo",
                                    FilterField = "ppoNo",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionerDetailsService.GetAllPensioners(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionerDetailsService.DataCount()
                        },
                    Message = $"All PPO Details Received Successfully!"

                };
            } catch(DbException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                apiResponseStatus = Enum.APIResponseStatus.Error,
                result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }
    



        [HttpPost("ppo/{ppoId}/bank-accounts")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Bank Accounts")]
        public async Task<JsonAPIResponse<PensionerBankAcDTO>> ControlPensionerBankAccountsCreate(
                int ppoId,
                PensionerBankAcDTO pensionerBankAcDTO
            )
        {
            JsonAPIResponse<PensionerBankAcDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Bank account details saved sucessfully!",
            };
            try {
                PensionerResponseDTO? pensionerResponseDTO = await _pensionerDetailsService.GetPensioner<PensionerResponseDTO>(
                        ppoId,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
                response.Result = await _pensionerBankAccountService.CreatePensionerBankAccount(
                    ppoId,
                    pensionerResponseDTO.Id,
                    pensionerBankAcDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (DbUpdateException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"ServiceError: {ex.InnerException?.Message ?? ex.Message} {ex.StackTrace}";
            }
            finally {
                if(response.Result?.DataSource != null) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"Error: Bank account details not saved! Check DataSource for more details!";
                }
            }

            return response;
        }

        [HttpPut("ppo/{ppoId}/bank-accounts")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Bank Accounts")]
        public async Task<APIResponse<PensionerBankAcDTO>> ControlPensionerBankAccountsUpdate(
                int ppoId,
                PensionerBankAcDTO pensionerBankAcDTO
            )
        {
            APIResponse<PensionerBankAcDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Bank account details saved sucessfully!",
            };
            try {
                response.result = await _pensionerBankAccountService.UpdatePensionerBankAccount(
                    ppoId,
                    pensionerBankAcDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (DbUpdateException ex) {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"Bank account details not saved! Error: {ex.Message}";
            }
            finally {
                if(response.result?.DataSource != null) {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"Error: Bank account details not saved!";
                }
            }

            return response;
        }

        [HttpGet("ppo/{ppoId}/bank-accounts")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Bank Accounts")]
        public async Task<APIResponse<PensionerBankAcDTO>> ControlPensionerBankAccountsRead(
                int ppoId
            )
        {

            APIResponse<PensionerBankAcDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Bank Accounts Details received sucessfully!",
            };
            try {
                response.result = await _pensionerBankAccountService.GetPensionerBankAccount(
                    ppoId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (DbUpdateException ex) {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"Bank Accounts Details not received! Error: {ex.Message}";
            }
            finally {
                if(response.result?.DataSource != null) {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"Bank Accounts Details not received!";
                }
            }

            return response;
        }
    
        [HttpGet("ppo/{ppoId}/first-bill-general")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: First Bill")]
        public async Task<APIResponse<PensionerFirstBillDTO>> ControlFirstBillsPensionerInfoRead(
                int ppoId
            )
        {

            APIResponse<PensionerFirstBillDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Pensioner Details received sucessfully!",
                result = new (){
                    Pensioner = new(){},
                    BankAccount = new(){}
                }
            };
            try {
                response.result.BankAccount = await _pensionerBankAccountService.GetPensionerBankAccount(
                    ppoId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
                response.result.Pensioner = await _pensionerDetailsService.GetPensioner<PensionerListDTO>(
                    ppoId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (DbUpdateException ex) {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"Bank Accounts Details not received! Error: {ex.Message}";
            }
            finally {
                if(response.result?.DataSource != null) {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"Bank Accounts Details not received!";
                }
            }

            return response;
        }


        [HttpPost("pension/primary-category")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Category Master")]
        public async Task<JsonAPIResponse<PensionPrimaryCategoryResponseDTO>> ControlPensionPrimaryCategoryCreate(
                PensionPrimaryCategoryEntryDTO pensionPrimaryCategoryEntryDTO
            )
        {

            JsonAPIResponse<PensionPrimaryCategoryResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PrimaryCategory saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionCategoryService
                    .CreatePensionPrimaryCategory<PensionPrimaryCategoryEntryDTO, PensionPrimaryCategoryResponseDTO>(
                        pensionPrimaryCategoryEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            finally {
                if(response.Result.Id == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: PrimaryCategory not saved!";
                }
            }

            return response;
        }

        [HttpPatch("pension/primary-category")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Category Master")]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionPrimaryCategoryResponseDTO>>>> ControlPensionPrimaryCategoryList(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionPrimaryCategoryResponseDTO>>> response;
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Primary Category ID",
                                    DataType = "text",
                                    FieldName = "primaryCategoryId",
                                    FilterField = "primaryCategoryId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Head of Account",
                                    DataType = "text",
                                    FieldName = "hoaId",
                                    FilterField = "hoaId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Primary Category Name",
                                    DataType = "text",
                                    FieldName = "categoryName",
                                    FilterField = "categoryName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionCategoryService.ListPrimaryCategory<PensionPrimaryCategoryResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionCategoryService.DataCount()
                        },
                    Message = $"All Primary Category Details Received Successfully!"

                };
            } catch(DbException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                ApiResponseStatus = Enum.APIResponseStatus.Error,
                Result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }

        [HttpPost("pension/sub-category")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Category Master")]
        public async Task<JsonAPIResponse<PensionSubCategoryResponseDTO>> ControlPensionSubCategoryCreate(
                PensionSubCategoryEntryDTO pensionSubCategoryEntryDTO
            )
        {

            JsonAPIResponse<PensionSubCategoryResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"SubCategory saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionCategoryService
                    .CreatePensionSubCategory<PensionSubCategoryEntryDTO, PensionSubCategoryResponseDTO>(
                        pensionSubCategoryEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            finally {
                if(response.Result.Id == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: SubCategory not saved!";
                }
            }

            return response;
        }

        [HttpPatch("pension/sub-category")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Category Master")]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionSubCategoryResponseDTO>>>> ControlPensionSubCategoryList(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionSubCategoryResponseDTO>>> response;
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Sub Category ID",
                                    DataType = "text",
                                    FieldName = "primaryCategoryId",
                                    FilterField = "primaryCategoryId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Sub Category Name",
                                    DataType = "text",
                                    FieldName = "categoryName",
                                    FilterField = "categoryName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionCategoryService.ListSubCategory<PensionSubCategoryResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionCategoryService.DataCount()
                        },
                    Message = $"All Sub Category Details Received Successfully!"

                };
            } catch(DbException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                ApiResponseStatus = Enum.APIResponseStatus.Error,
                Result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }

        [HttpPost("pension/category")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Category Master")]
        public async Task<JsonAPIResponse<PensionCategoryResponseDTO>> ControlPensionCategoryCreate(
                PensionCategoryEntryDTO pensionCategoryEntryDTO
            )
        {

            JsonAPIResponse<PensionCategoryResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Category saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionCategoryService
                    .CreatePensionCategory<PensionCategoryEntryDTO, PensionCategoryResponseDTO>(
                        pensionCategoryEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            finally {
                if(response.Result.Id == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Category not saved!";
                }
            }

            return response;
        }

        [HttpPatch("pension/category")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Category Master")]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionCategoryListDTO>>>> ControlPensionCategoryList(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionCategoryListDTO>>> response;
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Category ID",
                                    DataType = "text",
                                    FieldName = "id",
                                    FilterField = "id",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "Primary Category ID",
                                    DataType = "text",
                                    FieldName = "primaryCategoryId",
                                    FilterField = "primaryCategoryId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Sub Category ID",
                                    DataType = "text",
                                    FieldName = "subCategoryId",
                                    FilterField = "subCategoryId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Category Name",
                                    DataType = "text",
                                    FieldName = "categoryName",
                                    FilterField = "categoryName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionCategoryService.ListPensionCategory<PensionCategoryListDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionCategoryService.DataCount()
                        },
                    Message = $"All PPO Details Received Successfully!"

                };
            } catch(DbException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                ApiResponseStatus = Enum.APIResponseStatus.Error,
                Result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }
 
        [HttpPost("pension/bill-component")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Bill Component")]
        public async Task<JsonAPIResponse<PensionBreakupResponseDTO>> ControlPensionBillComponentCreate(
                PensionBreakupEntryDTO pensionBreakupEntryDTO
            )
        {

            JsonAPIResponse<PensionBreakupResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Bill Component saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionBreakupService
                    .CreatePensionBreakup<PensionBreakupEntryDTO, PensionBreakupResponseDTO>(
                        pensionBreakupEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch(InvalidCastException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"C-Error: {ex.Message}";
            }
            finally {
                if(response.Result.Id == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Bill Component not saved!";
                }
            }

            return response;
        }

        [HttpPatch("pension/bill-component")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Bill Component")]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionBreakupResponseDTO>>>> ControlPensionBillComponentList(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionBreakupResponseDTO>>> response;
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Bill Component ID",
                                    DataType = "text",
                                    FieldName = "id",
                                    FilterField = "id",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "Component Name",
                                    DataType = "text",
                                    FieldName = "componentName",
                                    FilterField = "componentName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Component Type",
                                    DataType = "text",
                                    FieldName = "componentType",
                                    FilterField = "componentType",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Relief Allowed",
                                    DataType = "text",
                                    FieldName = "reliefFlag",
                                    FilterField = "reliefFlag",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionBreakupService.ListBreakup<PensionBreakupResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionBreakupService.DataCount()
                        },
                    Message = $"All Bill Breakups Received Successfully!"

                };
            } catch(DbException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                ApiResponseStatus = Enum.APIResponseStatus.Error,
                Result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }
 
        [HttpPost("pension/component-rate")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Component Rate")]
        public async Task<JsonAPIResponse<PensionRatesResponseDTO>> ControlPensionComponentRateCreate(
                PensionRatesEntryDTO pensionRatesEntryDTO
            )
        {

            JsonAPIResponse<PensionRatesResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Component Rate saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionRateService
                    .CreatePensionRates<PensionRatesEntryDTO, PensionRatesResponseDTO>(                    
                            pensionRatesEntryDTO,
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        );
            }
            catch(InvalidCastException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"C-Error: {ex.Message}";
            }
            finally {
                if(response.Result.Id == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Component Rate not saved!";
                }
            }

            return response;
        }

        [HttpPatch("pension/component-rate")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: Component Rate")]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionRatesResponseDTO>>>> ControlPensionComponentRateList(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionRatesResponseDTO>>> response = new();
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Component Rate ID",
                                    DataType = "text",
                                    FieldName = "id",
                                    FilterField = "id",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "Category ID",
                                    DataType = "text",
                                    FieldName = "categoryId",
                                    FilterField = "categoryId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Bill Breakup ID",
                                    DataType = "text",
                                    FieldName = "breakupId",
                                    FilterField = "breakupId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Effective From Date",
                                    DataType = "text",
                                    FieldName = "effectiveFromDate",
                                    FilterField = "effectiveFromDate",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Rate Amount",
                                    DataType = "text",
                                    FieldName = "rateAmount",
                                    FilterField = "rateAmount",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Rate Type",
                                    DataType = "text",
                                    FieldName = "rateType",
                                    FilterField = "rateType",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionRateService.ListRates<PensionRatesResponseDTO>(
                                    GetCurrentFyYear(),
                                    GetTreasuryCode(),
                                    dynamicListQueryParameters
                                ),
                            DataCount = _pensionRateService.DataCount()
                        },
                    Message = $"All Bill Breakups Received Successfully!"

                };
            }
            finally {

            }
            return response;
        }
 


        [HttpPatch("ppo/pension-bill")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: First Bill")]
        public async Task<JsonAPIResponse<PensionerFirstBillDTO>> ControlFirstBillsGeneration(
                InitiateFirstPensionBillDTO initiateFirstPensionBillDTO
            )
        {

            JsonAPIResponse<PensionerFirstBillDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill generated sucessfully!",
                Result = new (){
                    Pensioner = new(){},
                    BankAccount = new(){}
                }
            };
            try {
                response.Result = await _pensionBillService.GenerateFirstPensionBills<PensionerFirstBillDTO>(
                    initiateFirstPensionBillDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch(InvalidCastException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"C-Error: {ex.Message}";
            }
            catch (DbUpdateException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"C-Error: Unable to generate first pension bill Error: {ex.Message}";
            }
            finally {
                if(response.Result?.DataSource != null) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Unable to generate first pension bill";
                }
            }

            return response;
        }
    } 
}