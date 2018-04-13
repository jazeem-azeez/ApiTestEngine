using System;
using ApiTestEngine.Shared.Services.Interfaces;

namespace ApiTestEngine.Shared.Services
{
    /// <summary></summary>
    /// <seealso cref="ApiTestEngine.Shared.Services.Interfaces.IBasicLogger" />
    public class ConsoleLogger : IBasicLogger
    {
        /// <summary> The write lock </summary>
        private object writeLock = new object();

        /// <summary> Logs the trace. </summary>
        /// <param name="message"> The message. </param>
        public void LogTrace(string message)
        {
            WriteLineLog(message, ConsoleColor.Cyan);
        }

        /// <summary> Writes the line log. </summary>
        /// <param name="message"> The message. </param>
        /// <param name="color">   The color. </param>
        public void WriteLineLog(string message, ConsoleColor color) => WriteLog($"{message}\r\n", color);

        /// <summary> Writes the log. </summary>
        /// <param name="message"> The message. </param>
        /// <param name="color">   The color. </param>
        public void WriteLog(string message, ConsoleColor color)
        {
            lock (writeLock)
            {
                Console.ForegroundColor = color;
                Console.Write(message);
                Console.ResetColor();
            }
        }
    }
}