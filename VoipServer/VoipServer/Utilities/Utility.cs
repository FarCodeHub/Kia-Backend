using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace VoipServer.Utilities
{
    public static class Utility
    {
        public enum LogType
        {
            Error,
            Information,
            Warning
        }
        public static void Log2(this ILogger logger, string message, LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:

                    logger.LogError(message);
                    for (int i = 0; i < 20; i++)
                    {
                        Console.Beep(1500, 500);
                        Thread.Sleep(300);
                    }
                    break;
                case LogType.Information:
                    logger.LogInformation(message);
                    break;
            }
        }

        public static string SimplifiyPhoneNumber(string inpute)
        {
            if (inpute.StartsWith("21"))
            {
                return inpute;
            }
            else if (inpute.StartsWith("09")) return inpute.Remove(0,1);
            else if (inpute.StartsWith("909"))
            {
                return inpute.Remove(0, 2);
            }
            else if (inpute.StartsWith("9") && inpute.Length == 10)
            {
                return inpute;
            }
            else if (inpute.StartsWith("009"))
            {
                return inpute.Remove(0,2);
            }
            else if (inpute.StartsWith("+989"))
            {
                return inpute.Remove(0, 3);
            }
            else if (inpute.StartsWith("989"))
            {
                return inpute.Remove(0, 2);
            }
            else
            {
                return inpute;
            }
        }
    }
}