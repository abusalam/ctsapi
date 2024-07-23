using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs.PensionDTO;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;


namespace CTS_BE.BAL.Services.Pension
{
    public class PensionService : IPensionService
    {
        private readonly IManualPpoReceiptRepository _manualPpoReceiptRepository;
        private readonly IReceiptSequenceRepository _receiptSequenceRepository;
        private readonly IMapper _mapper;

        public PensionService (
            IManualPpoReceiptRepository manualPpoReceiptRepository,
            IReceiptSequenceRepository receiptSequenceRepository,
            IMapper mapper
            )
        {
            _manualPpoReceiptRepository = manualPpoReceiptRepository;
            _receiptSequenceRepository = receiptSequenceRepository;
            _mapper = mapper;
            
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
            )
        {

            ;
            PpoReceipt manualPpoReceiptEntity;
            try
            {
                manualPpoReceiptEntity = _mapper.Map<PpoReceipt>(manualPpoReceiptDTO);
                manualPpoReceiptEntity.TreasuryCode = treasuryCode;
                manualPpoReceiptEntity.TreasuryReceiptNo = await _receiptSequenceRepository.GenerateTreasuryReceiptNo(
                    financialYear,
                    treasuryCode
                );
                manualPpoReceiptEntity.FinancialYear = financialYear;
                manualPpoReceiptEntity.PpoStatus = $"PPO Received";
                manualPpoReceiptEntity.ActiveFlag = true;
                
                _manualPpoReceiptRepository.Add(manualPpoReceiptEntity);

                await _manualPpoReceiptRepository.SaveChangesManagedAsync();
            }
            finally {


            }
            return _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
        }

        public async Task<IEnumerable<ListAllPpoReceiptsResponseDTO>> GetPpoReceipts(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        ) {
            return await _manualPpoReceiptRepository
                .GetSelectedColumnByConditionAsync(
                entity => entity.ActiveFlag && entity.FinancialYear == financialYear && entity.TreasuryCode == treasuryCode,
                entity => new ListAllPpoReceiptsResponseDTO() {
                    TreasuryReceiptNo = entity.TreasuryReceiptNo,
                    PpoNo = entity.PpoNo,
                    PensionerName = entity.PensionerName,
                    ReceiptDate = entity.ReceiptDate
                    },
                    dynamicListQueryParameters
                );
        }


        public async Task<ManualPpoReceiptResponseDTO> UpdatePpoReceipt(string treasuryReceiptNo, ManualPpoReceiptEntryDTO manualPpoReceiptDTO)
        {

            PpoReceipt? manualPpoReceiptEntity = new ();
            try
            {
                manualPpoReceiptEntity = await _manualPpoReceiptRepository.GetSingleAysnc(
                        entity => entity.TreasuryReceiptNo == treasuryReceiptNo
                        );
                if(manualPpoReceiptEntity is not null) {                    
                    manualPpoReceiptEntity.PpoNo = manualPpoReceiptDTO.PpoNo;
                    manualPpoReceiptEntity.PpoType = manualPpoReceiptDTO.PpoType;
                    manualPpoReceiptEntity.PsaCode = manualPpoReceiptDTO.PsaCode;
                    manualPpoReceiptEntity.PensionerName = manualPpoReceiptDTO.PensionerName;
                    manualPpoReceiptEntity.MobileNumber = manualPpoReceiptDTO.MobileNumber;
                    manualPpoReceiptEntity.ReceiptDate = manualPpoReceiptDTO.ReceiptDate;
                    manualPpoReceiptEntity.DateOfCommencement = manualPpoReceiptDTO.DateOfCommencement;

                    if(_manualPpoReceiptRepository.Update(manualPpoReceiptEntity)) {
                        await _manualPpoReceiptRepository.SaveChangesManagedAsync();
                    }
                } else {
                    manualPpoReceiptEntity = _mapper.Map<PpoReceipt>(manualPpoReceiptDTO);
                }
            }
            finally {


            }
            return _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
        }
    }
}