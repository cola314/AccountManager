using AccountManager.Models;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;

namespace AccountManager.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private readonly AccountRepository _accountRepository;

        public DelegateCommand ExportCommand { get; }

        public DelegateCommand ImportCommand { get; }

        public SettingsViewModel(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;

            ExportCommand = new DelegateCommand(Export);
            ImportCommand = new DelegateCommand(Import);
        }

        private void Export()
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    _accountRepository.SaveToFile(dialog.FileName, AppSetting.Instance.Password,
                        AppSetting.Instance.Accounts);
                }
            }
            catch
            {
                //TODO logging and alert
            }
        }

        private void Import()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    var accounts = _accountRepository.Load(dialog.FileName, AppSetting.Instance.Password);
                    AppSetting.Instance.Accounts = accounts;
                    AppSetting.Instance.Save();
                }
            }
            catch
            {
                //TODO logging and alert
            }
        }
    }
}