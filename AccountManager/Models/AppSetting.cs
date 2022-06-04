using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using AccountManager.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccountManager.Models
{
    public class AppSetting
    {
        private static readonly string SETTING_FOLDER = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
        private static readonly string SETTING_FILE = Path.Combine(SETTING_FOLDER, "setting.json");

        private readonly ILogger<AppSetting> _logger;
        private readonly AccountRepository _accountRepository;

        public AppSetting(ILogger<AppSetting> logger, AccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        private string _password;
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
                _logger.LogInformation("Save settings");

                if (!Directory.Exists(SETTING_FOLDER))
                {
                    Directory.CreateDirectory(SETTING_FOLDER);
                }
                _accountRepository.SaveToFile(SETTING_FILE, Password, Accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save settings to file");
            }
        }

        public void Load()
        {
            Accounts = _accountRepository.Load(SETTING_FILE, Password);
        }
    }
}
