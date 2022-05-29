using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AccountManager.Model;
using AccountManager.ViewModels.Popup;
using AccountManager.Views.Popup;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ModernWpf.Controls;

namespace AccountManager.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {
        public ICommand LoadedCommand
        {
            get
            {
                return new AsyncRelayCommand(async () =>
                {
                    try
                    {
                        await SetPassword();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                });
            }
        }

        private async Task SetPassword()
        {
            var popup = new PasswordAskDialog();
            if (await popup.ShowAsync(ContentDialogPlacement.Popup) == ContentDialogResult.Primary)
            {
                if (popup.DataContext is PasswordAskDialogViewModel viewModel)
                {
                    AppSetting.Instance.Password = viewModel.Password;
                    try
                    {
                        AppSetting.Instance.Load();
                        Accounts = new ObservableCollection<Account>(AppSetting.Instance.Accounts);
                    }
                    catch (Exception)
                    {
                        await SetPassword();
                    }
                }
            }
        }

        private void SaveAccount()
        {
            AppSetting.Instance.Accounts = Accounts.ToList();
            AppSetting.Instance.Save();
        }

        private ObservableCollection<Account> _accounts = new ObservableCollection<Account>();
        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }

        private ObservableCollection<Account> _selectedAccounts = new ObservableCollection<Account>();
        public ObservableCollection<Account> SelectedAccounts
        {
            get => _selectedAccounts;
            set => SetProperty(ref _selectedAccounts, value);
        }

        public ICommand AccountSelectionChanged
        {
            get
            {
                return new RelayCommand<SelectionChangedEventArgs>(args =>
                {
                    foreach (Account item in args.AddedItems)
                    {
                        SelectedAccounts.Add(item);
                    }

                    foreach (Account item in args.RemovedItems)
                    {
                        SelectedAccounts.Remove(item);
                    }
                    OnPropertyChanged(nameof(ChangeAccount));
                    OnPropertyChanged(nameof(DeleteAccount));
                    OnPropertyChanged(nameof(CopyIdCommand));
                    OnPropertyChanged(nameof(CopyPasswordCommand));
                });
            }
        }

        public ICommand ChangeAccount
        {
            get
            {
                return new AsyncRelayCommand(async () =>
                {
                    var selectedAccount = SelectedAccounts.First();
                    var viewModel = selectedAccount.Clone();
                    var dialog = new AccountView
                    {
                        DataContext = viewModel
                    };
                    if (await dialog.ShowAsync(ContentDialogPlacement.InPlace) == ContentDialogResult.Primary)
                    {
                        selectedAccount.CopyFrom(viewModel);
                    }
                    SaveAccount();
                }, () => SelectedAccounts.Count == 1);
            }
        }

        public ICommand DeleteAccount
        {
            get
            {
                return new AsyncRelayCommand(async () =>
                {
                    if (await new DeleteAskDialog().ShowAsync() == ContentDialogResult.Primary)
                    {
                        foreach (var account in SelectedAccounts.ToArray())
                        {
                            Accounts.Remove(account);
                        }
                    }
                    SaveAccount();
                }, () => SelectedAccounts.Any());
            }
        }

        public ICommand AddAccount
        {
            get
            {
                return new AsyncRelayCommand(async () =>
                {
                    var viewModel = new Account();
                    var dialog = new AccountView
                    {
                        DataContext = viewModel
                    };
                    if (await dialog.ShowAsync(ContentDialogPlacement.InPlace) == ContentDialogResult.Primary)
                    {
                        Accounts.Add(viewModel);
                    }
                    SaveAccount();
                });
            }
        }

        public ICommand CopyIdCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Clipboard.SetText(SelectedAccounts.First().Id);
                }, () => SelectedAccounts.Count == 1);
            }
        }

        public ICommand CopyPasswordCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Clipboard.SetText(SelectedAccounts.First().Password);
                }, () => SelectedAccounts.Count == 1);
            }
        }
    }
}
