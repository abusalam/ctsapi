using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Factories.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class PpoReceiptSeeder(
        PensionDbContext context,
        IManualPpoReceiptRepository _manualPpoReceiptRepository
    ) : BaseSeeder, ISeeder
    {
        public void Seed(int count = 10)
        {
            if (context.PpoReceipts.Any())
            {
                return; // Exit if there are already PPO receipts in the database
            }

            var receiptData = new PpoReceiptFactory().Make(count).ToList();

            for (int i = 0; i < receiptData.Count; i++)
            {
                var receipt = receiptData[i];
                var ppoReceiptEntity = new PpoReceipt
                {
                    FinancialYear = _financialYear,
                    TreasuryCode = _treasuryCode,
                    PpoNo = receipt.PpoNo,
                    PensionerName = receipt.PensionerName,
                    DateOfCommencement = receipt.DateOfCommencement,
                    MobileNumber = receipt.MobileNumber,
                    ReceiptDate = receipt.ReceiptDate,
                    PsaCode = receipt.PsaCode,
                    PpoType = receipt.PpoType,
                    PpoStatus = "PPO Received",
                    CreatedBy = 1,
                    ActiveFlag = true,
                };

                _manualPpoReceiptRepository.CreatePpoReceiptWithTreasuryReceiptNo<ManualPpoReceiptResponseDTO>(
                    _financialYear,
                    _treasuryCode,
                    ppoReceiptEntity
                );
                new PPOReceiptSequencesSeeder(context).Seed(i + 1);
            }
            new PPOReceiptSequencesSeeder(context).Seed(count + 1);
        }
    }
}
