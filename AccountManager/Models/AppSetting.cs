using System;
using System.Collections.Generic;
using System.IO;
using AccountManager.Configs;
using AccountManager.Models.Accounts;
using AccountManager.Utils;
using Microsoft.Extensions.Logging;

namespace AccountManager.Models
{
    public class AppSetting
    {
        private readonly ILogger _logger;
        private readonly AccountRepository _accountRepository;
        private readonly FileConfig _fileConfig;

        public AppSetting(ILogger<AppSetting> logger, AccountRepository accountRepository, FileConfig fileConfig)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _fileConfig = fileConfig;
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

                if (!Directory.Exists(_fileConfig.SettingFolder))
                {
                    Directory.CreateDirectory(_fileConfig.SettingFolder);
                }
                _accountRepository.SaveToFile(_fileConfig.SecretFile, Password, Accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save settings to file");
            }
        }

        public void Load()
        {
            Accounts = _accountRepository.Load(_fileConfig.SecretFile, Password);
        }
    }
}
