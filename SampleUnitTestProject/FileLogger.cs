using System.IO;
using ApiTestEngine.Shared.Services.Interfaces;

namespace SampleUnitTestProject
{
    internal class FileLogger : IBasicLogger
    {
        private object filewritelock = new object();

        public void LogTrace(string message)
        {
            WriteLog(message);
        }

        public void WriteLog(string message)
        {
            lock (filewritelock)
            {
                File.AppendAllText(nameof(FileLogger), message);
            }
        }
    }
}