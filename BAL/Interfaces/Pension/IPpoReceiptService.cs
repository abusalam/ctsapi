using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoReceiptService : IBaseService
    {
        public Task<ManualPpoReceiptResponseDTO> CreatePpoReceipt(
                ManualPpoReceiptEntryDTO manualPpoReceiptDTO,
                short financialYear,
                string treasuryCode
            );
        public Task<ManualPpoReceiptResponseDTO> GetPpoReceipt(string treasuryReceiptNo);
        public Task<IEnumerable<ListAllPpoReceiptsResponseDTO>> GetAllPpoReceipts(
                short financialYear,
                string treasuryCode,
                DynamicListQueryParameters dynamicListQueryParameters
            );
        public Task<IEnumerable<ListAllPpoReceiptsResponseDTO>> GetAllUnusedPpoReceipts(
            short financialYear,
            string treasuryCode
        );
        public Task<ManualPpoReceiptResponseDTO> UpdatePpoReceipt(
                string treasuryReceiptNo,
                ManualPpoReceiptEntryDTO manualPpoReceiptDTO
            );
    }
}