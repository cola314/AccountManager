using AccountManager.Model;
using AccountManager.Util;
using AccountManager.View;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AccountManager.ViewModel
{
    public class MainWindowViewModel : ObservableRecipient
    {
        public MainWindowViewModel()
        {
        }

        public ICommand LoadedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        await SetPassword();
                    }
                    catch(Exception ex)
                    {

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
                    catch (Exception ex)
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

        private ObservableCollection<Account> accounts_ = new ObservableCollection<Account>();
        public ObservableCollection<Account> Accounts
        {
            get => accounts_;
            set => SetProperty(ref accounts_, value);
        }

        private ObservableCollection<Account> selectedAccounts_ = new ObservableCollection<Account>();
        public ObservableCollection<Account> SelectedAccounts
        {
            get => selectedAccounts_;
            set => SetProperty(ref selectedAccounts_, value);
        }

        public ICommand AccountSelectionChanged
        {
            get
            {
                return new RelayCommand<SelectionChangedEventArgs>(args =>
                {
                    foreach(Account item in args.AddedItems)
                    {
                        SelectedAccounts.Add(item);
                    }

                    foreach(Account item in args.RemovedItems)
                    {
                        SelectedAccounts.Remove(item);
                    }
                    OnPropertyChanged(nameof(ChangeAccount));
                    OnPropertyChanged(nameof(DeleteAccount));
                });
            }
        }

        public ICommand ChangeAccount
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var selectedAccount = SelectedAccounts.First();
                    var viewModel = selectedAccount.Clone();
                    var dialog = new AccountView();
                    dialog.DataContext = viewModel;
                    if (await dialog.ShowAsync(ModernWpf.Controls.ContentDialogPlacement.InPlace) == ContentDialogResult.Primary)
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
                return new RelayCommand(async () =>
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
                return new RelayCommand(async () =>
                {
                    var viewModel = new Account();
                    var dialog = new AccountView();
                    dialog.DataContext = viewModel;
                    if (await dialog.ShowAsync(ModernWpf.Controls.ContentDialogPlacement.InPlace) == ContentDialogResult.Primary)
                    {
                        Accounts.Add(viewModel);
                    }
                    SaveAccount();
                });
            }
        }
    }
}
