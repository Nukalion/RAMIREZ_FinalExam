using System;
using System.IO;

namespace RAMIREZ_FinalExam_AppsDev.Services
{
    public static class AuditLogger
    {
        private static readonly string logFile = "Data/audit.log";

        public static void Log(string action, string details)
        {
            string log = $"{DateTime.Now} | {action} | {details}";
            File.AppendAllText(logFile, log + Environment.NewLine);
        }
    }
}