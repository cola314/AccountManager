using System;
using System.Linq;
using AccountManager.Models;
using AccountManager.Models.Accounts;
using AccountManager.Models.Csv;
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
        private readonly CsvExporter _csvExporter;
        private readonly CsvImporter _csvImporter;

        public DelegateCommand ExportCommand { get; }
        public DelegateCommand ImportCommand { get; }
        public DelegateCommand CsvImportCommand { get; }
        public DelegateCommand CsvExportCommand { get; }

        public SettingsViewModel(AccountRepository accountRepository, ILogger<SettingsViewModel> logger, AppSetting appSetting, CsvExporter csvExporter, CsvImporter csvImporter)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _appSetting = appSetting;
            _csvExporter = csvExporter;
            _csvImporter = csvImporter;

            ExportCommand = new DelegateCommand(Export);
            ImportCommand = new DelegateCommand(Import);
            CsvExportCommand = new DelegateCommand(CsvExport);
            CsvImportCommand = new DelegateCommand(CsvImport);
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


        private void CsvExport()
        {
            try
            {
                _logger.LogInformation("Export accounts to csv");
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    _csvExporter.Export(_appSetting.Accounts, dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                //TODO Alert
                _logger.LogError(ex, "Failed to export accounts to csv");
            }
        }

        private void CsvImport()
        {
            try
            {
                _logger.LogInformation("Import accounts from csv file");
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    var accounts = _csvImporter.Import(dialog.FileName);
                    _appSetting.Accounts = accounts.ToList();
                    _appSetting.Save();
                }
            }
            catch (Exception ex)
            {
                //TODO Alert
                _logger.LogError(ex, "Failed to import accounts from csv");
            }
        }
    }
}