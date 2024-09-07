using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Helper
{
    public static class PensionExtensions
    {
        public static void FillDataSource<TResponse,TEntity>(this TResponse response, TEntity entity, string message) {
            dynamic dataSource = new ExpandoObject(){};
            dataSource.Message = $"{message}";
            dataSource.Entity = entity;
            response?.GetType()
                .GetProperty("DataSource")?.SetValue(
                    response,
                    dataSource,
                    null
                );
        }

        public static void PrintOut(this string textToWriteOnConsole) {
            // Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Out.WriteLine(textToWriteOnConsole);
            Console.ResetColor();
        }
    }
}