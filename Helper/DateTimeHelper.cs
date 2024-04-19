namespace CTS_BE.Helper
{
    public static class DateTimeHelper
    {
        public static double GetJulianDate(DateTime dateTime)
        {
            const double JulianDateOffset = 2440587.5; // Offset to start from January 1, 4713 BCE (Julian calendar)

            // Calculate Julian Date
            double julianDate = JulianDateOffset + dateTime.ToOADate();

            return julianDate;
        }
    }
}
