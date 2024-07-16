using CTS_BE.DTOs.PensionDTO;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionService
    {
        public Task<string> CreateManualPpoReceipt(ManualPpoReceiptDTO manualPpoReceiptDTO);
        public Task<ManualPpoReceiptDTO> GetManualPpoReceipt(string treasuryReceiptNo);
    }
}