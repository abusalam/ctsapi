using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.BAL.Services.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class PpoBillSeeder(
        PensionDbContext context,
        IPpoBillRepository _ppoBillRepository,
        IMapper mapper,
        IManualPpoReceiptRepository _manualPpoReceiptRepository,
        IPpoIdSequenceRepository _ppoIdSequenceRepository,
        IPensionBillService _pensionBillService,
        IPpoBillService _ppoBillService
    ) : BaseSeeder, ISeeder
    {
        public async Task SeedAsync(int count = 1)
        {
            if (await context.PpoBills.AnyAsync())
            {
                return; // Exit if there are already PpoBills in the database
            }

            new AccountHeadSeeder(context).Seed(count);
            new BranchSeeder(context).Seed(count);
            var billSeeder = new BillSeeder(
                context,
                mapper,
                _ppoBillRepository,
                _manualPpoReceiptRepository,
                _ppoIdSequenceRepository
            );
            await billSeeder.SeedAsync(count);

            // Get the bill IDs
            var billIds = await context.Bills.Select(b => b.Id).ToListAsync();

            var pensionerSeeder = new PensionerSeeder(
                context,
                mapper,
                _manualPpoReceiptRepository,
                _ppoIdSequenceRepository
            );
            await pensionerSeeder.SeedAsync(count);

            var createdPensioners = await context.Pensioners.ToListAsync();
            var random = new Random();
            if (!await context.PpoStatusFlags.AnyAsync())
            {
                var ppoStatusFlagSeeder = new PpoStatusFlagSeeder(
                    context,
                    mapper,
                    _manualPpoReceiptRepository,
                    _ppoIdSequenceRepository
                );
                await ppoStatusFlagSeeder.SeedAsync(count);
            }

            for (int i = 0; i < count; i++)
            {
                try
                {
                    if (i >= createdPensioners.Count)
                    {
                        break; // Exit if there are no more pensioners to assign
                    }

                    var pensioner = createdPensioners[i];
                    Console.WriteLine($"Processing PPO {pensioner.PpoId} (Index: {i})");

                    var ppoStatusFlag = await context.PpoStatusFlags.FirstOrDefaultAsync(f =>
                        f.PensionerId == pensioner.Id
                        && f.StatusFlag == PensionStatusFlag.PpoApproved
                    );

                    if (ppoStatusFlag == null)
                    {
                        Console.WriteLine(
                            $"Creating missing PpoApproved status flag for PPO {pensioner.PpoId}"
                        );
                        ppoStatusFlag = new PpoStatusFlag
                        {
                            FinancialYear = _financialYear,
                            TreasuryCode = _treasuryCode,
                            PensionerId = pensioner.Id,
                            PpoId = pensioner.PpoId,
                            StatusWef = DateOnly.FromDateTime(DateTime.Today),
                            StatusFlag = PensionStatusFlag.PpoApproved,
                            CreatedBy = 1,
                            ActiveFlag = true,
                        };
                        context.Set<PpoStatusFlag>().Add(ppoStatusFlag);
                        await context.SaveChangesAsync();
                    }

                    var initiateFirstPensionBillDTO = new InitiateFirstPensionBillDTO
                    {
                        PpoId = pensioner.PpoId,
                        ToDate = DateOnly.FromDateTime(
                            DateTime.Now.AddDays(
                                random.Next(
                                    0,
                                    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)
                                        - DateTime.Now.Day
                                )
                            )
                        ),
                    };

                    PensionerFirstBillResponseDTO bill =
                        await _pensionBillService.SavePensionBill<PensionerFirstBillResponseDTO>(
                            initiateFirstPensionBillDTO,
                            BillType.FirstBill,
                            _financialYear,
                            _treasuryCode
                        );

                    var ppoBillResponse = await _ppoBillService.SavePpoBill<PpoBillSaveResponseDTO>(
                        bill,
                        _financialYear,
                        _treasuryCode
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing pensioner at index {i}: {ex.Message}");
                    Console.WriteLine($"Exception details: {ex}");
                }
            }
            bool hasPpoComponentRevisions = await context.Set<PpoComponentRevision>().AnyAsync();
            if (!hasPpoComponentRevisions)
            {
                throw new Exception(
                    "PpoComponentRevision table does not contain any data after seeding!"
                );
            }
            else
            {
                Console.WriteLine("✓ PpoComponentRevision data validation successful!");
            }
        }

        public void Seed(int count = 1)
        {
            SeedAsync(count).GetAwaiter().GetResult();
        }
    }
}
