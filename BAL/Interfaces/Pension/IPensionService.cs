using CTS_BE.DTOs.PensionDTO;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionService
    {
        public Task<ManualPpoReceiptResponseDTO> CreatePpoReceipt(ManualPpoReceiptEntryDTO manualPpoReceiptDTO);
        public Task<ManualPpoReceiptResponseDTO> GetPpoReceipt(string treasuryReceiptNo);
        public Task<ICollection<ListAllPpoReceiptsResponseDTO>> GetPpoReceipts();
        public Task<ManualPpoReceiptResponseDTO> UpdatePpoReceipt(string treasuryReceiptNo, ManualPpoReceiptEntryDTO manualPpoReceiptDTO);
    }
}