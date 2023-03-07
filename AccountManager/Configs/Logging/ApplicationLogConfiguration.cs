using System;
using System.Configuration;
using System.IO;
using NLog.Config;

namespace AccountManager.Configs.Logging
{
    public class ApplicationLogConfiguration : LoggingConfiguration
    {
        public ApplicationLogConfiguration()
        {
            string logFileName = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var logFolder = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
            var fileLogger = new NLog.Targets.FileTarget("log-file") { FileName = $"{logFolder}/Logs/{logFileName}.log" };
            var consoleLogger = new NLog.Targets.ConsoleTarget("log-console");
     
            this.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, consoleLogger);
            this.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, fileLogger);
        }
    }
}
