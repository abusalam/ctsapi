using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper.Authentication;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;


namespace CTS_BE.BAL.Services.Pension
{
    public class PpoReceiptService : BaseService, IPpoReceiptService
    {
        private readonly IManualPpoReceiptRepository _manualPpoReceiptRepository;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;

        public PpoReceiptService (
            IManualPpoReceiptRepository manualPpoReceiptRepository,
            IClaimService claimService,
            IMapper mapper
            ) : base(claimService)
        {
            _manualPpoReceiptRepository = manualPpoReceiptRepository;
            _claimService = claimService;
            _mapper = mapper;
            _userId = _claimService.GetUserId();
            
        }

        public async Task<ManualPpoReceiptResponseDTO> GetPpoReceipt(string treasuryReceiptNo)
        {
            ManualPpoReceiptResponseDTO manualPpoReceiptResponseDTO;
            try
            {
                manualPpoReceiptResponseDTO = _mapper.Map<ManualPpoReceiptResponseDTO>(
                        await _manualPpoReceiptRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag
                        && entity.TreasuryReceiptNo == treasuryReceiptNo
                    )
                );
            }
            catch (DbUpdateException ex){
                ManualPpoReceiptResponseDTO errorResponse = _mapper.Map<ManualPpoReceiptResponseDTO>(null);
                errorResponse.FillDataSource(new PpoReceipt(), ex.InnerException?.Message ?? ex.Message);
                return errorResponse;
            }
            return manualPpoReceiptResponseDTO;
        }

        public async Task<ManualPpoReceiptResponseDTO> GetPpoReceipt(long receiptId)
        {
            ManualPpoReceiptResponseDTO manualPpoReceiptResponseDTO;
            try
            {
                manualPpoReceiptResponseDTO = _mapper.Map<ManualPpoReceiptResponseDTO>(
                        await _manualPpoReceiptRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag
                        && entity.Id == receiptId
                    )
                );
            }
            catch (DbUpdateException ex){
                ManualPpoReceiptResponseDTO errorResponse = _mapper.Map<ManualPpoReceiptResponseDTO>(null);
                errorResponse.FillDataSource(new PpoReceipt(), ex.InnerException?.Message ?? ex.Message);
                return errorResponse;
            }
            return manualPpoReceiptResponseDTO;
        }

        public async Task<ManualPpoReceiptResponseDTO> CreatePpoReceipt(
            ManualPpoReceiptEntryDTO manualPpoReceiptDTO,
            short financialYear,
            string treasuryCode
        ) {
            PpoReceipt manualPpoReceiptEntity = _mapper.Map<PpoReceipt>(manualPpoReceiptDTO);
            ManualPpoReceiptResponseDTO manualPpoReceiptDTOResponse = _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptDTO);
            try
            {
                manualPpoReceiptEntity = _mapper.Map<PpoReceipt>(manualPpoReceiptDTO);
                manualPpoReceiptEntity.TreasuryCode = treasuryCode;
                manualPpoReceiptEntity.FinancialYear = financialYear;
                manualPpoReceiptEntity.PpoStatus = $"PPO Received";
                SetCreatedBy(manualPpoReceiptEntity);              
                manualPpoReceiptDTOResponse = await _manualPpoReceiptRepository
                    .CreatePpoReceiptWithTreasuryReceiptNo<ManualPpoReceiptResponseDTO>(
                        financialYear,
                        treasuryCode, 
                        manualPpoReceiptEntity
                    );
            }
            catch (Exception ex) {
                manualPpoReceiptDTOResponse.FillDataSource(
                    manualPpoReceiptEntity,
                    ex.Message
                );
                return manualPpoReceiptDTOResponse;
            }
            return manualPpoReceiptDTOResponse;
        }

        public async Task<IEnumerable<ListAllPpoReceiptsResponseDTO>> GetAllPpoReceipts(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        ) {
            _dataCount = _manualPpoReceiptRepository.Count();
            return await _manualPpoReceiptRepository
                .GetSelectedColumnByConditionAsync(
                    entity => entity.ActiveFlag && entity.FinancialYear == financialYear && entity.TreasuryCode == treasuryCode,
                    entity => _mapper.Map<ListAllPpoReceiptsResponseDTO>(entity),
                    dynamicListQueryParameters
                );
        }


        public async Task<IEnumerable<ListAllPpoReceiptsResponseDTO>> GetAllUnusedPpoReceipts(
            short financialYear,
            string treasuryCode
        )
        {
            var manualPpoReceipts = await _manualPpoReceiptRepository.GetAllUnusedPpoReceipts(
                financialYear,
                treasuryCode,
                entity => _mapper.Map<ListAllPpoReceiptsResponseDTO>(entity)
            );
            _dataCount = manualPpoReceipts.Count;
            return manualPpoReceipts;
        }


        public async Task<ManualPpoReceiptResponseDTO> UpdatePpoReceipt(string treasuryReceiptNo, ManualPpoReceiptEntryDTO manualPpoReceiptDTO)
        {

            PpoReceipt? manualPpoReceiptEntity = new ();

            ManualPpoReceiptResponseDTO manualPpoReceiptDTOResponse = _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
            try
            {
                manualPpoReceiptEntity = await _manualPpoReceiptRepository.GetSingleAysnc(
                        entity => entity.TreasuryReceiptNo == treasuryReceiptNo
                        );
                
                if(manualPpoReceiptEntity is null) {  
                    manualPpoReceiptDTOResponse.FillDataSource(manualPpoReceiptDTO, "Treasury Receipt No does not exist!");
                    return manualPpoReceiptDTOResponse;
                }                  
                manualPpoReceiptEntity.FillFrom(manualPpoReceiptDTO);
                SetUpdatedBy(manualPpoReceiptEntity);
                _manualPpoReceiptRepository.Update(manualPpoReceiptEntity);
                if(await _manualPpoReceiptRepository.SaveChangesManagedAsync()==0) {
                    manualPpoReceiptDTOResponse.FillDataSource(manualPpoReceiptEntity, "Update Failed!");
                    return manualPpoReceiptDTOResponse;
                }
                
            }
            catch (DbUpdateException ex){
                ManualPpoReceiptResponseDTO errorResponse = _mapper.Map<ManualPpoReceiptResponseDTO>(null);
                errorResponse.FillDataSource(new PpoReceipt(), ex.InnerException?.Message ?? ex.Message);
                return errorResponse;
            }
            return _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
        }

        public async Task<ManualPpoReceiptResponseDTO> UpdatePpoReceipt(long receiptId, ManualPpoReceiptEntryDTO manualPpoReceiptDTO)
        {

            PpoReceipt? manualPpoReceiptEntity = new ();

            ManualPpoReceiptResponseDTO manualPpoReceiptDTOResponse = _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
            try
            {
                manualPpoReceiptEntity = await _manualPpoReceiptRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag
                        && entity.Id == receiptId
                    );
                
                if(manualPpoReceiptEntity is null) {  
                    manualPpoReceiptDTOResponse.FillDataSource(manualPpoReceiptDTO, "Receipt does not exist! or has been deleted");
                    return manualPpoReceiptDTOResponse;
                }                  
                manualPpoReceiptEntity.FillFrom(manualPpoReceiptDTO);
                SetUpdatedBy(manualPpoReceiptEntity);
                _manualPpoReceiptRepository.Update(manualPpoReceiptEntity);
                if(await _manualPpoReceiptRepository.SaveChangesManagedAsync()==0) {
                    manualPpoReceiptDTOResponse.FillDataSource(manualPpoReceiptEntity, "Update Failed!");
                    return manualPpoReceiptDTOResponse;
                }
                
            }
            catch (DbUpdateException ex){
                ManualPpoReceiptResponseDTO errorResponse = _mapper.Map<ManualPpoReceiptResponseDTO>(null);
                errorResponse.FillDataSource(new PpoReceipt(), ex.InnerException?.Message ?? ex.Message);
                return errorResponse;
            }
            return _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
        }
    }
}