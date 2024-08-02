using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/ppo")]
    public class PpoBankAccount : ApiBase
    {
        private readonly IPensionerDetailsService _pensionerDetailsService;
        private readonly IPensionerBankAccountService _pensionerBankAccountService;

        public PpoBankAccount(
                IPensionerDetailsService pensionerDetailsService,
                IPensionerBankAccountService pensionerBankAccountService,
                IClaimService claimService
            ) : base(claimService)
        {
            _pensionerDetailsService = pensionerDetailsService;
            _pensionerBankAccountService = pensionerBankAccountService;
        }

        [HttpPost("{ppoId}/bank-accounts")]
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

        [HttpPut("{ppoId}/bank-accounts")]
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

        [HttpGet("{ppoId}/bank-accounts")]
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
    
    }
}