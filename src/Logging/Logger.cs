using System;
using System.IO;

namespace SecureInventory.Logging
{
    public static class Logger
    {
        private static readonly string logFile = "data/audit.log";

        static Logger()
        {
            Directory.CreateDirectory("data");
        }

        public static void Log(int userId, string role, string action, string entity, int entityId, string status)
        {
            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | User:{userId} | Role:{role} | Action:{action} | Entity:{entity} | EntityId:{entityId} | Status:{status}";
            File.AppendAllText(logFile, line + Environment.NewLine);
        }
    }
}
