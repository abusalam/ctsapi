using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
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

            new PPOReceiptSequencesSeeder(context).Seed(count);

            var receiptData = new PpoReceiptFactory()
                .Make(count)
                .Select(
                    (receipt, i) =>
                    {
                        return new PpoReceipt
                        {
                            FinancialYear = _financialYear,
                            TreasuryCode = _treasuryCode,
                            TreasuryReceiptNo =
                                _manualPpoReceiptRepository.GenerateTreasuryReceiptNo(
                                    _financialYear,
                                    _treasuryCode
                                ),
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
                    }
                )
                .ToList();

            context.PpoReceipts.AddRange(receiptData);
            context.SaveChanges();
        }
    }
}
