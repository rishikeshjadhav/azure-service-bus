
namespace Nitor.OSS.Logger
{
    using System;
    using System.Configuration;
    using System.Globalization;

    public static class Logger
    {
        private static bool ConsoleLogging = Convert.ToBoolean(ConfigurationManager.AppSettings["ConsoleLogging"], CultureInfo.InvariantCulture);

        public static void LogMessage(string message)
        {
            if (ConsoleLogging)
            {
                Console.WriteLine(message);
            }
        }

        public static void LogError(string error)
        {
            if (ConsoleLogging)
            {
                Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                Console.WriteLine("Error Occurred: " + error);
                Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            }
        }

        public static void LogException(Exception exception)
        {
            if (null != exception)
            {
                if (ConsoleLogging)
                {
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                    Console.WriteLine("Exception Occurred: " + exception.Message);
                    Console.WriteLine("Stack Trace: " + exception.StackTrace);
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                }
            }
        }
    }
}
