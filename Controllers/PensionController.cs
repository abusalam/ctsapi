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


        public PensionController(
                IClaimService claimService,
                IPensionService pensionService,
                IPensionStatusService pensionStatusService,
                IPensionerDetailsService pensionerDetailsService,
                IPensionerBankAccountService pensionerBankAccountService,
                IPensionCategoryService pensionCategoryService,
                IPensionBillService pensionBillService
            ) : base(claimService)
        {
            _pensionService = pensionService;
            _pensionStatusService = pensionStatusService;
            _pensionerDetailsService = pensionerDetailsService;
            _pensionerBankAccountService = pensionerBankAccountService;
            _pensionCategoryService = pensionCategoryService;
            _pensionBillService = pensionBillService;
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
                            DataCount = 10
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
        public async Task<APIResponse<PensionerResponseDTO>> ControlPensionerDetailsCreate(
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
                response.result = await _pensionerDetailsService.CreatePensioner(
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
                            DataCount = 10
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
        public async Task<APIResponse<PensionerBankAcDTO>> ControlPensionerBankAccountsCreate(
                int ppoId,
                PensionerBankAcDTO pensionerBankAcDTO
            )
        {
            APIResponse<PensionerBankAcDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Bank account details saved sucessfully!",
            };
            try {
                response.result = await _pensionerBankAccountService.CreatePensionerBankAccount(
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
                            DataCount = 10
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
                            DataCount = 10
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
                            DataCount = 10
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
 


        [HttpGet("ppo/{ppoId}/{toDate}/pension-bill")]
        [Produces("application/json")]
        [Tags("Pension", "Pension: First Bill")]
        public async Task<APIResponse<PensionerFirstBillDTO>> ControlFirstBillsRead(
                int ppoId,
                DateOnly toDate
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
    } 
}