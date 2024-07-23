using CTS_BE.DTOs;
using CTS_BE.DTOs.PensionDTO;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionService
    {
        public Task<ManualPpoReceiptResponseDTO> CreatePpoReceipt(
            ManualPpoReceiptEntryDTO manualPpoReceiptDTO,
            short financialYear,
            string treasuryCode
            );
        public Task<ManualPpoReceiptResponseDTO> GetPpoReceipt(string treasuryReceiptNo);
        public Task<IEnumerable<ListAllPpoReceiptsResponseDTO>> GetPpoReceipts(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
            );
        public Task<ManualPpoReceiptResponseDTO> UpdatePpoReceipt(string treasuryReceiptNo, ManualPpoReceiptEntryDTO manualPpoReceiptDTO);
    }
}