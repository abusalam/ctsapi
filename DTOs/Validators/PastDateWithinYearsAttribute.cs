using System.ComponentModel.DataAnnotations;

namespace CTS_BE.DTOs.Validators
{
    public class PastDateWithinYearsAttribute : RangeAttribute
    {
        /// <summary>
        /// Validate that the date is in the past and within specified number of years.
        /// </summary>
        public PastDateWithinYearsAttribute(short years) : base(
            typeof(DateOnly),
            DateTime.Now.AddYears(-years).ToString("yyyy-MM-dd"),
            DateTime.Now.ToString("yyyy-MM-dd")
        )
        {
        }
    }
}