using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp.ColorSpaces.Companding;

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