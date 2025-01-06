using System.ComponentModel.DataAnnotations;

namespace CTS_BE.DTOs.Validators
{
    public class CurrentOrFutureDateUptoYearsAttribute : RangeAttribute
    {
        /// <summary>
        /// Validate that the date is in the future and upto specified number of years.
        /// </summary>
        public CurrentOrFutureDateUptoYearsAttribute(short years) : base(
            typeof(DateOnly),
            DateTime.Now.ToString("yyyy-MM-dd"),
            DateTime.Now.AddYears(years).ToString("yyyy-MM-dd")
        )
        {
        }
    }
}