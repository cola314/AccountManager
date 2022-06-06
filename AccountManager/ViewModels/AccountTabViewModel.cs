﻿using Prism.Commands;
using Prism.Mvvm;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AccountManager.Models;
using AccountManager.ViewModels.Common;
using AccountManager.Views.Popup;
using ModernWpf.Controls;
using Prism.Ioc;
using Prism.Regions;

namespace AccountManager.ViewModels
{
    public class AccountTabViewModel : BindableBase, INavigationAware
    {
        private readonly IContainerExtension _container;
        private readonly AppSetting _appSetting;

        private ObservableCollection<AccountViewModel> _accounts = new ObservableCollection<AccountViewModel>();
        public ObservableCollection<AccountViewModel> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }

        private ObservableCollection<AccountViewModel> _selectedAccounts = new ObservableCollection<AccountViewModel>();
        public ObservableCollection<AccountViewModel> SelectedAccounts
        {
            get => _selectedAccounts;
            set => SetProperty(ref _selectedAccounts, value);
        }

        public DelegateCommand<SelectionChangedEventArgs> AccountSelectionChangedCommand { get; }
        public DelegateCommand DeleteAccountCommand { get; }
        public DelegateCommand AddAccountCommand { get; }
        public DelegateCommand ChangeAccountCommand { get; }
        public DelegateCommand CopyIdCommand { get; }
        public DelegateCommand CopyPasswordCommand { get; }

        public AccountTabViewModel(IContainerExtension container, AppSetting appSetting)
        {
            _container = container;
            _appSetting = appSetting;

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

            Accounts = new ObservableCollection<AccountViewModel>(
                _appSetting.Accounts.Select(x => new AccountViewModel(x)));
        }

        private void SaveAccount()
        {
            _appSetting.Accounts = Accounts.Select(x => x.ToAccount()).ToList();
            _appSetting.Save();
        }

        private void AccountSelectionChanged(SelectionChangedEventArgs args)
        {
            foreach (AccountViewModel item in args.AddedItems)
            {
                SelectedAccounts.Add(item);
            }

            foreach (AccountViewModel item in args.RemovedItems)
            {
                SelectedAccounts.Remove(item);
            }
        }

        private async void ChangedAccount()
        {
            var dialog = _container.Resolve<AccountDialog>();
            dialog.DataContext = SelectedAccounts.First().Clone();

            if (await dialog.ShowAsync(ContentDialogPlacement.InPlace) == ContentDialogResult.Primary)
            {
                SelectedAccounts.First().CopyFrom(dialog.DataContext as AccountViewModel);
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
            var dialog = _container.Resolve<AccountDialog>();
            dialog.DataContext = new AccountViewModel();
            if (await dialog.ShowAsync(ContentDialogPlacement.InPlace) == ContentDialogResult.Primary)
            {
                Accounts.Add(dialog.DataContext as AccountViewModel);
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Accounts = new ObservableCollection<AccountViewModel>(_appSetting.Accounts.Select(x => new AccountViewModel(x)));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
