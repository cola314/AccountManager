using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Config;

namespace AccountManager.Utils.Logging
{
    public class ApplicationLogConfiguration : LoggingConfiguration
    {
        public ApplicationLogConfiguration()
        {
            // Targets where to log to: File and Console
            var logFolder = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = logFolder + "/Logs/${longdate}.log" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            this.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            this.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);
        }
    }
}
