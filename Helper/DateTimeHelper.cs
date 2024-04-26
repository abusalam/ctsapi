namespace CTS_BE.Helper
{
    public static class DateTimeHelper
    {
        public static double GetJulianDay(DateTime dateTime)
        {
            // Convert the date to UTC
            dateTime = dateTime.ToUniversalTime();

            // Calculate Julian date
            int year = dateTime.Year;
            int month = dateTime.Month;
            int day = dateTime.Day;
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            int second = dateTime.Second;

            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }

            double a = Math.Floor(year / 100.0);
            double b = 2 - a + Math.Floor(a / 4.0);

            double julianDay = Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + b - 1524.5;
            double julianFraction = (hour + (minute + second / 60.0) / 60.0) / 24.0;

            return julianDay + julianFraction;
        }
        public static string GetJulianDate(DateTime dateTime){
            DateTime firstDay = new DateTime(dateTime.Year, 1, 1);
            int numberOfDays = (int) (dateTime-firstDay).TotalDays + 1;
            return dateTime.ToString("yy")+numberOfDays.ToString();
        }
    }
}
