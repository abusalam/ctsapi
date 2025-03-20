using System.Dynamic;

namespace CTS_BE.Helper
{
    public static class PensionExtensions
    {
        public static void FillErrorInDataSource<TResponse, TEntity>(
            this TResponse response,
            TEntity entity,
            string message,
            Exception? exception = null
        )
        {
            dynamic dataSource = new ExpandoObject() { };
            dataSource.Message =
                $"{message} {exception?.InnerException?.Message ?? exception?.Message} {exception?.StackTrace}";
            dataSource.Entity = entity;
            response?.GetType().GetProperty("DataSource")?.SetValue(response, dataSource, null);
        }

        public static void PrintOut(this string textToWriteOnConsole)
        {
            // Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Out.WriteLine(textToWriteOnConsole);
            Console.ResetColor();
        }
    }
}
