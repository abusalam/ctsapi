using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.PensionEnum;

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionCalculator
    {

        /// <summary>
        /// Calculates pension payments for a given period from a list of component rates.
        /// </summary>
        /// <param name="componentRates">List of component rates.</param>
        /// <param name="commencementDate">Date when the pension is commencing.</param>
        /// <param name="toDate">Date till which the pension payment is to be calculated.</param>
        /// <param name="basicPensionAmount">The basic pension amount.</param>
        /// <returns>List of pension payments.</returns>
        public static List<PpoPaymentListItemDTO> CalculatePpoPayments(
            ICollection<ComponentRate> componentRates,
            DateOnly commencementDate,
            DateOnly toDate,
            int basicPensionAmount,
            int commutedPensionAmount
        )
        {
            List<PpoPaymentListItemDTO>? ppoPayments = new();
            DateOnly calculatedPeriodStartDate = toDate;
            long prevBreakupId = 0;
                            
            componentRates.OrderBy(entity => entity.BreakupId)
                .ThenByDescending(entity => entity.EffectiveFromDate)
                .ToList().ForEach(componentRate => {
                    // $"{componentRate.Breakup.Id}, {componentRate.Breakup.ComponentName}".PrintOut();


                    // Reset the period start date for the next breakup
                    if(componentRate.Breakup.Id != prevBreakupId) {
                        calculatedPeriodStartDate = toDate;
                    }
                    if(commencementDate > calculatedPeriodStartDate) {
                        return;
                    }
                    prevBreakupId = componentRate.Breakup.Id;

                    int baseAmount = CalculateBaseAmount(
                        componentRate.Breakup.ComponentName,
                        componentRate.RateType,
                        basicPensionAmount,
                        commutedPensionAmount
                    );

                    ppoPayments.Add(new PpoPaymentListItemDTO()
                    {
                        RateId = componentRate.Id,
                        BreakupId = componentRate.Breakup.Id,
                        ComponentName = componentRate.Breakup.ComponentName,
                        ComponentType = componentRate.Breakup.ComponentType,

                        RateType = componentRate.RateType,
                        RateAmount = componentRate.RateAmount,
                        BasicPensionAmount = basicPensionAmount,
                        BaseAmount = baseAmount,
                        AmountPerMonth = CalculatePerMonthBreakupAmount(
                                CalculateEffectiveRate(
                                        componentRates.ToList(),
                                        componentRate.Breakup.Id,
                                        calculatedPeriodStartDate      
                                    ),
                                baseAmount
                            ),
                        FromDate = CalculatePeriodStartFromDate(
                                CalculateEffectiveRate(
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
                ppoPayment.PeriodInMonths = CalculateMonthsAndDays(
                        ppoPayment.FromDate,
                        ppoPayment.ToDate,
                        out int days
                    );
                ppoPayment.PeriodInDays = days;
                // $"{ppoPayment.FromDate}-{ppoPayment.ToDate}, {ppoPayment.PeriodInMonths}-{days} days".PrintOut();

                ppoPayment.DueAmount = CalculateDueAmount(
                        ppoPayment.ComponentType,
                        ppoPayment.AmountPerMonth,
                        days,
                        ppoPayment.PeriodInMonths,
                        ppoPayment.FromDate.Year,
                        ppoPayment.FromDate.Month
                    );
                ppoPayment.NetAmount = ppoPayment.DueAmount - ppoPayment.DrawnAmount;
            });
            return ppoPayments.OrderBy(entity => entity.ComponentName)
                .ThenBy(entity => entity.FromDate).ToList();
        }


        /// <summary>
        /// Calculates the base amount for a given component, based on its name. 
        /// If the component name is "AMOUNT COMMUTED", the base amount is the commuted pension amount, otherwise it is the basic pension amount.
        /// </summary>
        /// <param name="componentName">The name of the component.</param>
        /// <param name="basicPensionAmount">The basic pension amount.</param>
        /// <param name="commutedPensionAmount">The commuted pension amount.</param>
        /// <returns>The base amount for the component.</returns>
        public static int CalculateBaseAmount(
            string componentName,
            char rateType,
            long basicPensionAmount,
            long commutedPensionAmount
        )
        {
            return rateType switch {
                BreakupRateType.Percentage => (int) basicPensionAmount,
                BreakupRateType.Amount => componentName switch {
                    "AMOUNT COMMUTED" => (int) commutedPensionAmount * -1,
                    _ => (int) basicPensionAmount
                },
                _ => (int) basicPensionAmount
            };
        }


        /// <summary>
        /// Calculates the due amount for a given breakup component type (Payment or Deduction).
        /// </summary>
        /// <param name="breakupComponentType">The type of the breakup component.</param>
        /// <param name="amountPerMonth">The amount per month for the breakup component.</param>
        /// <param name="periodInDays">The period in days for which the amount is to be calculated.</param>
        /// <param name="periodInMonths">The period in months for which the amount is to be calculated.</param>
        /// <param name="forYear">The year for which the amount is to be calculated.</param>
        /// <param name="forMonth">The month for which the amount is to be calculated.</param>
        /// <returns>The due amount for the given breakup component.</returns>
        public static int CalculateDueAmount(
            char breakupComponentType,
            int amountPerMonth,
            int periodInDays,
            int periodInMonths,
            int forYear,
            int forMonth
        )
        {
            return breakupComponentType switch
            {
                BreakupComponentType.Payment => 1,
                BreakupComponentType.Deduction => -1,
                _ => 0
            } * (
                (periodInMonths * amountPerMonth)
                + amountPerMonth / DateTime.DaysInMonth(forYear, forMonth) * periodInDays
            );
        }


        /// <summary>
        /// Calculates the monthly amount for a given breakup rate.
        /// </summary>
        /// <param name="componentRate">The component rate.</param>
        /// <param name="basicPensionAmount">The basic pension amount.</param>
        /// <returns>The monthly breakup amount.</returns>
        /// <remarks>
        /// If the <paramref name="componentRate"/> is a percentage, the returned value is the percentage of the <paramref name="basicPensionAmount"/>.</br>
        /// If the <paramref name="componentRate"/> is a fixed amount and the rate amount is 0, the returned value is the <paramref name="basicPensionAmount"/>.</br>
        /// If the <paramref name="componentRate"/> is a fixed amount and the rate amount is not 0, the returned value is the rate amount.
        /// </remarks>
        public static int CalculatePerMonthBreakupAmount(
            ComponentRate componentRate,
            int baseAmount
        )
        {
            if(componentRate.RateType == BreakupRateType.Percentage) {
                return componentRate.RateAmount * baseAmount / 100;
            }
            if (componentRate.RateAmount == 0) {
                return baseAmount;
            }
            return componentRate.RateAmount;
        }


        /// <summary>
        /// Calculates the number of months between two given dates.
        /// </summary>
        /// <param name="fromDate">The start date.</param>
        /// <param name="toDate">The end date.</param>
        /// <returns>The number of whole months between the two dates.</returns>
        public static int CalculateMonthsAndDays(
            DateOnly fromDate,
            DateOnly toDate,
            out int days
        )
        {
            days = 0;
            int months = 0;
            if (fromDate > toDate) {
                return CalculateMonthsAndDays(toDate, fromDate, out days);
            }

            DateTime startDate = fromDate.ToDateTime(new TimeOnly(0, 0, 0));
            DateTime endDate = toDate.ToDateTime(new TimeOnly(0, 0, 0));

            for( var i = 1; ; ++i )
            {
                if( startDate.AddMonths( i ) > endDate )
                {
                    months = i - 1;

                    break;
                }
            }

            for( var i = 1; ; ++i )
            {
                if( startDate.AddMonths( months ).AddDays( i ) > endDate )
                {
                    days = i;

                    break;
                }
            }
            if(endDate.Day == days && DateTime.DaysInMonth(endDate.Year, endDate.Month) == days) {
                days = 0;
                months++;
            }
            return months;
        }


        /// <summary>
        /// Calculates distinct breakups from the given component rates.
        /// </summary>
        /// <param name="componentRates">List of component rates.</param>
        /// <returns>List of distinct breakups.</returns>
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


        /// <summary>
        /// Calculate effective rate for a given date and breakup
        /// </summary>
        /// <param name="componentRates">List of component rates</param>
        /// <param name="forBreakupId">Breakup id for which the effective rate is to be calculated</param>
        /// <param name="forDate">Date for which the effective rate is to be calculated</param>
        /// <returns>The effective rate for the given date and breakup</returns>
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
        public static DateOnly CalculatePeriodStartFromDate(
            DateOnly fromDate,
            DateOnly commencementDate
        )
        {
            if(fromDate < commencementDate) {
                return commencementDate;
            } else {
                return fromDate;
            }
        }


        /// <summary>
        /// Calculates the period start date based on the given date i.e the starting date of the month of the given date.
        /// </summary>
        /// <param name="fromDate">The date from which the period starts.</param>
        /// <returns>The period start date, which is the first day of the month of the given date.</returns>
        public static DateOnly CalculatePeriodStartDate(DateOnly fromDate) {
            return new DateOnly(fromDate.Year,fromDate.Month,1);
        }


        /// <summary>
        /// Calculates the period end date based on the given date i.e the last day of the month of the given date.
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

        public static string InWords(long number)   
        {  
            if (number == 0) return "ZERO";  
            if (number < 0) return "minus " + InWords(Math.Abs(number));  
            string words = "";  
            if ((number / 1000000) > 0)   
            {  
                words += InWords(number / 100000) + " LAKH ";  
                number %= 1000000;  
            }  
            if ((number / 1000) > 0)   
            {  
                words += InWords(number / 1000) + " THOUSAND ";  
                number %= 1000;  
            }  
            if ((number / 100) > 0)   
            {  
                words += InWords(number / 100) + " HUNDRED ";  
                number %= 100;  
            }  
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)   
            {  
                if (words != "") words += "AND ";  
                var unitsMap = new[]   
                {  
                    "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"  
                };  
                var tensMap = new[]   
                {  
                    "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"  
                };  
                if (number < 20) words += unitsMap[number];  
                else   
                {  
                    words += tensMap[number / 10];  
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];  
                }  
            }  
            return words;  
        }
    }
}