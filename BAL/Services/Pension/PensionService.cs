using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs.PensionDTO;
using CTS_BE.DAL.Entities.Pension;


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

        public async Task<ManualPpoReceiptDTO> GetManualPpoReceipt(string treasuryReceiptNo)
        {
            ManualPpoReceiptDTO manualPpoReceiptDTO;
            try
            {
                manualPpoReceiptDTO = _mapper.Map<ManualPpoReceiptDTO>(
                    await _manualPpoReceiptRepository.GetSingleAysnc(
                        entity => entity.TreasuryReceiptNo == treasuryReceiptNo
                        )
                    );
            }
            finally {

            }
            return manualPpoReceiptDTO;
        }

        public async Task<string> CreateManualPpoReceipt(ManualPpoReceiptDTO manualPpoReceiptDTO)
        {

            PpoReceipt manualPpoReceiptEntity;
            try
            {
                manualPpoReceiptEntity = _mapper.Map<PpoReceipt>(manualPpoReceiptDTO);

                manualPpoReceiptEntity.TreasuryCode = await _receiptSequenceRepository.GetUserTreasuryCode();
                manualPpoReceiptEntity.TreasuryReceiptNo = await _receiptSequenceRepository.GenerateTreasuryReceiptNo(
                    await _receiptSequenceRepository.GetUserFinYear(),
                    await _receiptSequenceRepository.GetUserTreasuryCode()
                );
                manualPpoReceiptEntity.PpoStatus = $"PPO Received";
                
                _manualPpoReceiptRepository.Add(manualPpoReceiptEntity);

                await _manualPpoReceiptRepository.SaveChangesManagedAsync();
            }
            finally {


            }
            return manualPpoReceiptEntity.TreasuryReceiptNo;
        }

    }
}