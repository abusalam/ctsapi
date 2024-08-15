using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.PensionEnum;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore.Diagnostics;
using CTS_BE.DTOs;
using System.ComponentModel;

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
                            
            componentRates.OrderBy(entity => entity.BreakupId)
                .ThenByDescending(entity => entity.EffectiveFromDate)
                .ToList().ForEach(componentRate => {
                $"{componentRate.Breakup.Id}, {componentRate.Breakup.ComponentName}".PrintOut();


                // Reset the period start date for the next breakup
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
                    AmountPerMonth = PensionCalculator.CalculatePerMonthBreakupAmount(
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
               ppoPayment.DueAmount = ppoPayment.PeriodInMonths * ppoPayment.AmountPerMonth;
               ppoPayment.NetAmount = ppoPayment.DueAmount - ppoPayment.DrawnAmount;
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

        /// <summary>
        /// Calculates the number of months between two given dates.
        /// </summary>
        /// <param name="fromDate">The start date.</param>
        /// <param name="toDate">The end date.</param>
        /// <returns>The number of whole months between the two dates.</returns>
        public static int CalculatePeriodInMonths(
            DateOnly fromDate,
            DateOnly toDate
        )
        {
            if (fromDate > toDate) {
                return CalculatePeriodInMonths(toDate, fromDate);
            }

            DateTime startDate = CalculatePeriodStartDate(fromDate).ToDateTime(new TimeOnly(0, 0, 0));
            DateTime endDate = CalculatePeriodEndDate(toDate).ToDateTime(new TimeOnly(0, 0, 0));
            var roughMonths = (int)(endDate - startDate).TotalDays / 30;

            if(startDate.AddMonths(roughMonths) > endDate) {
                $"fromDate:{fromDate}, toDate:{toDate}, roughMonths(-1):{roughMonths}".PrintOut();
                return roughMonths - 1;
            } else {
                $"fromDate:{fromDate}, toDate:{toDate}, roughMonths:{roughMonths}".PrintOut();
                return roughMonths;
            }
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


        /// <summary>
        /// Calculates the period start date based on the given from date and commencement date.
        /// </summary>
        /// <param name="fromDate">The date from which the period starts.</param>
        /// <param name="commencementDate">The date when the pension is commencing.</param>
        /// <returns>The fromDate or commencementDate whichever is the later.</returns>
        public static DateOnly CalculatePeriodStartFromDate(DateOnly fromDate, DateOnly commencementDate) {
            if(fromDate < commencementDate) {
                return commencementDate;
            } else {
                return fromDate;
            }
        }


        /// <summary>
        /// Calculates the period start date based on the given date.
        /// </summary>
        /// <param name="fromDate">The date from which the period starts.</param>
        /// <returns>The period start date, which is the first day of the month of the given date.</returns>
        public static DateOnly CalculatePeriodStartDate(DateOnly fromDate) {
            return new DateOnly(fromDate.Year,fromDate.Month,1);
        }


        /// <summary>
        /// Calculates the period end date based on the given date.
        /// </summary>
        /// <param name="fromDate">The start date of the period.</param>
        /// <returns>The end date of the month of the given date.</returns>
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