using System;
using AccountManager.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;

namespace AccountManager.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private readonly ILogger<SettingsViewModel> _logger;
        private readonly AppSetting _appSetting;
        private readonly AccountRepository _accountRepository;

        public DelegateCommand ExportCommand { get; }

        public DelegateCommand ImportCommand { get; }

        public SettingsViewModel(AccountRepository accountRepository, ILogger<SettingsViewModel> logger, AppSetting appSetting)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _appSetting = appSetting;

            ExportCommand = new DelegateCommand(Export);
            ImportCommand = new DelegateCommand(Import);
        }

        private void Export()
        {
            try
            {
                _logger.LogInformation("Export accounts to file");
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    _accountRepository.SaveToFile(dialog.FileName, _appSetting.Password, _appSetting.Accounts);
                }
            }
            catch (Exception ex)
            {
                //TODO Alert
                _logger.LogError(ex, "Failed to export accounts");
            }
        }

        private void Import()
        {
            try
            {
                _logger.LogInformation("Import accounts from file");
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    var accounts = _accountRepository.Load(dialog.FileName, _appSetting.Password);
                    _appSetting.Accounts = accounts;
                    _appSetting.Save();
                }
            }
            catch (Exception ex)
            {
                //TODO Alert
                _logger.LogError(ex, "Failed to import accounts");
            }
        }
    }
}