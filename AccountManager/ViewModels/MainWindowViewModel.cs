using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using AccountManager.Models;
using AccountManager.Properties;
using AccountManager.ViewModels.Popup;
using AccountManager.Views;
using AccountManager.Views.Popup;
using Microsoft.Extensions.Logging;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace AccountManager.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly ILogger<MainWindowViewModel> _logger;
        private readonly IContainerExtension _container;
        private readonly IRegionManager _regionManager;

        public DelegateCommand LoadedCommand { get; }
        public DelegateCommand<NavigationViewSelectionChangedEventArgs> SelectedMenuItemChangedCommand { get; }

        public MainWindowViewModel(IContainerExtension container, IRegionManager regionManager, ILogger<MainWindowViewModel> logger)
        {
            _container = container;
            _regionManager = regionManager;
            _logger = logger;

            LoadedCommand = new DelegateCommand(Loaded);
            SelectedMenuItemChangedCommand = new DelegateCommand<NavigationViewSelectionChangedEventArgs>(OnSelectedMenuItemChanged);
        }

        private async void Loaded()
        {
            try
            {
                _logger.LogInformation("Load Settings");
                await SetPassword();

                _regionManager.RequestNavigate(RegionNames.MainRegion, nameof(AccountView));
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

            try
            {
                AppSetting.Instance.Password = popup.ViewModel.Password;
                AppSetting.Instance.Load();
            }
            catch (Exception)
            {
                await SetPassword();
            }
        }

        private void OnSelectedMenuItemChanged(NavigationViewSelectionChangedEventArgs o)
        {
            if (o.IsSettingsSelected)
            {
                _regionManager.RequestNavigate(RegionNames.MainRegion, nameof(SettingsView));
            }
            else if (o.SelectedItemContainer.Content as string == Resources.ACCOUNT)
            {
                _regionManager.RequestNavigate(RegionNames.MainRegion, nameof(AccountView));
            }
        }
    }
}