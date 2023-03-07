using System.Configuration;
using System.IO;

namespace AccountManager.Configs
{
    public class FileConfig
    {
        public string SettingFolder { get; }
        public string SecretFile { get; }
        public string AppConfigFile { get; }

        public FileConfig()
        {
            SettingFolder = Path.GetDirectoryName(
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
            SecretFile = Path.Combine(SettingFolder, "secret.json");
            AppConfigFile = Path.Combine(SettingFolder, "config.json");
        }
    }
}