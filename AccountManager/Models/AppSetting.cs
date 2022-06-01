using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using AccountManager.Utils;
using Newtonsoft.Json;

namespace AccountManager.Models
{
    public class AppSetting
    {
        private static readonly string SETTING_FOLDER = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
        private static readonly string SETTING_FILE = Path.Combine(SETTING_FOLDER, "setting.json");

        private AppSetting()
        {
        }

        public static AppSetting Instance { get; private set; } = new AppSetting();
        
        private string _password;
        [JsonIgnore]
        public string Password
        {
            get => AesTool.Decrypt(_password, "_password");
            set => _password = AesTool.Encrypt(value, "_password");
        }

        public List<Account> Accounts { get; set; } = new List<Account>();

        public void Save()
        {
            try
            {
                string data = AesTool.Encrypt(JsonConvert.SerializeObject(AppSetting.Instance), Password);

                if (!Directory.Exists(SETTING_FOLDER))
                {
                    Directory.CreateDirectory(SETTING_FOLDER);
                }
                File.WriteAllText(SETTING_FILE, data);
            }
            catch (Exception)
            {
                //TODO logging
            }
        }

        public void Load()
        {
            if (!File.Exists(SETTING_FILE))
            {
                return;
            }
            string data = File.ReadAllText(SETTING_FILE);
            Instance = JsonConvert.DeserializeObject<AppSetting>(AesTool.Decrypt(data, Password));
            Instance.Password = Password;
        }
    }
}
