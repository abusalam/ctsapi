using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.PensionEnum;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore.Diagnostics;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionCalculator
    {

        public static List<PpoPaymentListItemDTO> CalculatePpoPayments(
            ICollection<ComponentRate> componentRates,
            DateOnly commencementDate,
            DateOnly toDate,
            long basicPensionAmount
        )
        {
            List<PpoPaymentListItemDTO>? ppoPayments = new();
            DateOnly calculatedPeriodStartDate = toDate;
            long prevBreakupId = 0;
            componentRates.ToList().ForEach(componentRate => {
                $"{componentRate.Breakup.Id}, {componentRate.Breakup.ComponentName}".PrintOut();
                if(componentRate.Breakup.Id != prevBreakupId) {
                    calculatedPeriodStartDate = toDate;
                }
                prevBreakupId = componentRate.Breakup.Id;
                ppoPayments.Add(new PpoPaymentListItemDTO()
                {
                    RateId = componentRate.Id,
                    BreakupId = componentRate.Breakup.Id,
                    ComponentName = componentRate.Breakup.ComponentName,
                    ComponentType = componentRate.Breakup.ComponentType,

                    RateType = componentRate.RateType,
                    RateAmount = componentRate.RateAmount,
                    BasicPensionAmount = basicPensionAmount,
                    BreakupAmount = PensionCalculator.CalculatePerMonthBreakupAmount(
                        PensionCalculator.CalculateEffectiveRate(
                            componentRates.ToList(),
                            componentRate.Breakup.Id,
                            calculatedPeriodStartDate      
                        ),
                        basicPensionAmount
                        ),
                    FromDate = PensionCalculator.CalculatePeriodStartFromDate(
                            PensionCalculator.CalculateEffectiveRate(
                                    componentRates.ToList(),
                                    componentRate.Breakup.Id,
                                    calculatedPeriodStartDate.AddDays(-1)
                                ).EffectiveFromDate,
                            commencementDate
                        ),
                    ToDate = calculatedPeriodStartDate.AddDays(-1),
                });
                calculatedPeriodStartDate = componentRate.EffectiveFromDate;
            });
            ppoPayments.ForEach(ppoPayment => {
               ppoPayment.PeriodInMonths = PensionCalculator.CalculatePeriodInMonths(
                   ppoPayment.FromDate,
                   ppoPayment.ToDate
               );
               ppoPayment.DueAmount = ppoPayment.PeriodInMonths * ppoPayment.BreakupAmount;
            });
            return ppoPayments.OrderBy(entity => entity.ComponentName)
                .ThenBy(entity => entity.FromDate).ToList();
        }

        public static long CalculatePerMonthBreakupAmount(
            ComponentRate componentRate,
            long basicPensionAmount
        )
        {
            if(componentRate.RateType == BreakupRateType.Percentage) {
                return componentRate.RateAmount * basicPensionAmount / 100;
            }
            if (componentRate.RateAmount == 0) {
                return basicPensionAmount;
            }
            return componentRate.RateAmount;
        }

        public static long CalculateTotalPensionAmount(
            List<ComponentRate> componentRates,
            int basicPensionAmount,
            short forMonths
        )
        {
            return componentRates
                .Select(componentRate => CalculatePerMonthBreakupAmount(    
                        componentRate,
                        basicPensionAmount
                    ) * forMonths
                )
                .Sum();
        }

        /// <summary>
        /// Calculates the number of months between two given dates.
        /// </summary>
        /// <param name="fromDate">The start date.</param>
        /// <param name="toDate">The end date.</param>
        /// <returns>The number of months between the two dates, or 0 if the start date is after the end date.</returns>
        public static int CalculatePeriodInMonths(
            DateOnly fromDate,
            DateOnly toDate
        )
        {
            if (fromDate > toDate) {
                return 0;
            }

            DateTime startDate = fromDate.ToDateTime(new TimeOnly(0, 0, 0));
            DateTime endDate = toDate.ToDateTime(new TimeOnly(0, 0, 0));
            return (int)(endDate - startDate).TotalDays / 30;
        }

        public static List<long> CalculateBreakups(
            List<ComponentRate> componentRates
        )
        {
            List<long> componentBreakupIds = new ();
            componentRates
                .Select(componentRate => componentRate.BreakupId)
                .Distinct()
                .ToList()
                .ForEach(componentBreakupIds.Add);
            return componentBreakupIds;
        }

        public static ComponentRate CalculateEffectiveRate(
            List<ComponentRate> componentRates,
            long forBreakupId,
            DateOnly forDate
        )
        {
            ComponentRate prevRate = componentRates.Where(componentRate => componentRate.BreakupId == forBreakupId)
                .OrderBy(componentRate => componentRate.EffectiveFromDate)
                .ToList().First();

            componentRates.Where(componentRate => componentRate.BreakupId == forBreakupId)
                .OrderBy(componentRate => componentRate.EffectiveFromDate)
                .ToList()
                .ForEach(componentRate => {
                    if (componentRate.EffectiveFromDate >= forDate) {
                        $"For:{forDate}, Selected:{prevRate.EffectiveFromDate}, Breakup:{componentRate.EffectiveFromDate}".PrintOut();
                        return;
                    }
                    prevRate = componentRate;
                });

            return prevRate;
        }

        public static DateOnly CalculatePeriodStartFromDate(DateOnly fromDate, DateOnly commencementDate) {
            if(fromDate < commencementDate) {
                return commencementDate;
            } else {
                return fromDate;
            }
        }

        public static DateOnly CalculatePeriodStartDate(DateOnly fromDate) {
            return new DateOnly(fromDate.Year,fromDate.Month,1);
        }

        public static DateOnly CalculatePeriodEndDate(DateOnly fromDate) {
            return new DateOnly(
                    fromDate.Year,
                    fromDate.Month,
                    DateTime.DaysInMonth(
                        fromDate.Year,
                        fromDate.Month
                    )
                );
        }
    }
}