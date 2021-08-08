using AccountManager.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Model
{
    internal class AppSetting
    {
        private static readonly string SETTING_FOLDER = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
        private static readonly string SETTING_FILE = Path.Combine(SETTING_FOLDER, "setting.json");

        private AppSetting()
        {
        }

        public static AppSetting Instance { get; private set; } = new AppSetting();

        [JsonIgnore]
        public string password_;
        [JsonIgnore]
        public string Password
        {
            get => AESTool.Decrypt(password_, "password_");
            set
            {
                password_ = AESTool.Encrypt(value, "password_");
            }
        }

        public List<Account> Accounts { get; set; } = new List<Account>();

        public void Save()
        {
            try
            {
                string data = AESTool.Encrypt(JsonConvert.SerializeObject(AppSetting.Instance), Password);

                if(!Directory.Exists(SETTING_FOLDER))
                {
                    Directory.CreateDirectory(SETTING_FOLDER);
                }
                File.WriteAllText(SETTING_FILE, data);
            }
            catch(Exception ex)
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
            Instance = JsonConvert.DeserializeObject<AppSetting>(AESTool.Decrypt(data, Password));
            Instance.Password = Password;
        }
    }
}
