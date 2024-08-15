using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper.Authentication;
using CTS_BE.Helper;


namespace CTS_BE.BAL.Services.Pension
{
    public class PpoReceiptService : BaseService, IPpoReceiptService
    {
        private readonly IManualPpoReceiptRepository _manualPpoReceiptRepository;
        private readonly IReceiptSequenceRepository _receiptSequenceRepository;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;

        public PpoReceiptService (
            IManualPpoReceiptRepository manualPpoReceiptRepository,
            IReceiptSequenceRepository receiptSequenceRepository,
            IClaimService claimService,
            IMapper mapper
            ) : base(claimService)
        {
            _manualPpoReceiptRepository = manualPpoReceiptRepository;
            _receiptSequenceRepository = receiptSequenceRepository;
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
                        entity => entity.TreasuryReceiptNo == treasuryReceiptNo
                    )
                );
            }
            finally {

            }
            return manualPpoReceiptResponseDTO;
        }

        public async Task<ManualPpoReceiptResponseDTO> CreatePpoReceipt(
            ManualPpoReceiptEntryDTO manualPpoReceiptDTO,
            short financialYear,
            string treasuryCode
        ) {
            PpoReceipt manualPpoReceiptEntity;
            ManualPpoReceiptResponseDTO manualPpoReceiptDTOResponse = _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptDTO);
            try
            {
                manualPpoReceiptEntity = _mapper.Map<PpoReceipt>(manualPpoReceiptDTO);
                manualPpoReceiptEntity.TreasuryCode = treasuryCode;
                manualPpoReceiptEntity.FinancialYear = financialYear;
                manualPpoReceiptEntity.TreasuryReceiptNo = await _receiptSequenceRepository.GenerateTreasuryReceiptNo(
                    financialYear,
                    treasuryCode
                );
                manualPpoReceiptEntity.PpoStatus = $"PPO Received";
                SetCreatedBy(manualPpoReceiptEntity);                
                _manualPpoReceiptRepository.Add(manualPpoReceiptEntity);
                if(await _manualPpoReceiptRepository.SaveChangesManagedAsync()==0) {
                    manualPpoReceiptDTOResponse.FillDataSource(manualPpoReceiptEntity, "Failed to create PPO Receipt");
                    return manualPpoReceiptDTOResponse;
                }
            }
            finally {


            }
            return _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
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
            
                manualPpoReceiptEntity = _mapper.Map<PpoReceipt>(manualPpoReceiptDTO);
                
            }
            finally {


            }
            return _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
        }
    }
}