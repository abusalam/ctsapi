using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp.ColorSpaces.Companding;

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