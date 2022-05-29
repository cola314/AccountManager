using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AccountManager.Models;
using AccountManager.ViewModels.Popup;
using AccountManager.Views.Popup;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;

namespace AccountManager.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IContainerExtension _container;

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

        public DelegateCommand LoadedCommand { get; }
        public DelegateCommand<SelectionChangedEventArgs> AccountSelectionChangedCommand { get; }
        public DelegateCommand DeleteAccountCommand { get; }
        public DelegateCommand AddAccountCommand { get; }
        public DelegateCommand ChangeAccountCommand { get; }
        public DelegateCommand CopyIdCommand { get; }
        public DelegateCommand CopyPasswordCommand { get; }

        public MainWindowViewModel(IContainerExtension container)
        {
            _container = container;

            LoadedCommand = new DelegateCommand(Loaded);
            AccountSelectionChangedCommand = new DelegateCommand<SelectionChangedEventArgs>(AccountSelectionChanged);
            DeleteAccountCommand = new DelegateCommand(DeleteAccount, CanDeleteAccount);
            AddAccountCommand = new DelegateCommand(AddAccount);
            ChangeAccountCommand = new DelegateCommand(ChangedAccount, CanChangeAccount);
            CopyIdCommand = new DelegateCommand(CopyId, CanCopyId);
            CopyPasswordCommand = new DelegateCommand(CopyPassword, CanCopyPassword);

            SelectedAccounts.CollectionChanged += (sender, args) =>
            {
                ChangeAccountCommand.RaiseCanExecuteChanged();
                DeleteAccountCommand.RaiseCanExecuteChanged();
                CopyIdCommand.RaiseCanExecuteChanged();
                CopyPasswordCommand.RaiseCanExecuteChanged();
            };
        }

        private async void Loaded()
        {
            try
            {
                await SetPassword();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private async Task SetPassword()
        {
            var popup = _container.Resolve<PasswordAskDialog>();
            if (await popup.ShowAsync(ContentDialogPlacement.Popup) != ContentDialogResult.Primary)
                return;

            if (!(popup.DataContext is PasswordAskDialogViewModel viewModel))
                return;

            try
            {
                AppSetting.Instance.Password = viewModel.Password;
                AppSetting.Instance.Load();
                Accounts = new ObservableCollection<Account>(AppSetting.Instance.Accounts);
            }
            catch (Exception)
            {
                await SetPassword();
            }
        }

        private void SaveAccount()
        {
            AppSetting.Instance.Accounts = Accounts.ToList();
            AppSetting.Instance.Save();
        }

        private void AccountSelectionChanged(SelectionChangedEventArgs args)
        {
            foreach (Account item in args.AddedItems)
            {
                SelectedAccounts.Add(item);
            }

            foreach (Account item in args.RemovedItems)
            {
                SelectedAccounts.Remove(item);
            }
        }

        private async void ChangedAccount()
        {
            var dialog = _container.Resolve<AccountView>();
            dialog.DataContext = SelectedAccounts.First().Clone();

            if (await dialog.ShowAsync(ContentDialogPlacement.InPlace) == ContentDialogResult.Primary)
            {
                SelectedAccounts.First().CopyFrom(dialog.DataContext as Account);
            }

            SaveAccount();
        }

        private bool CanChangeAccount()
        {
            return SelectedAccounts.Count == 1;
        }

        private async void DeleteAccount()
        {
            if (await _container.Resolve<DeleteAskDialog>().ShowAsync() != ContentDialogResult.Primary)
                return;

            foreach (var account in SelectedAccounts.ToArray())
            {
                Accounts.Remove(account);
            }

            SaveAccount();
        }

        private bool CanDeleteAccount()
        {
            return SelectedAccounts.Any();
        }

        private async void AddAccount()
        {
            var dialog = _container.Resolve<AccountView>();
            dialog.DataContext = new Account();
            if (await dialog.ShowAsync(ContentDialogPlacement.InPlace) == ContentDialogResult.Primary)
            {
                Accounts.Add(dialog.DataContext as Account);
            }

            SaveAccount();
        }

        private void CopyId()
        {
            Clipboard.SetText(SelectedAccounts.First().Id);
        }

        public bool CanCopyId()
        {
            return SelectedAccounts.Count == 1;
        }

        private void CopyPassword()
        {
            Clipboard.SetText(SelectedAccounts.First().Password);
        }

        private bool CanCopyPassword()
        {
            return SelectedAccounts.Count == 1;
        }
    }
}