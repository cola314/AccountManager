using System;
using System.Threading.Tasks;
using AccountManager.Models;
using AccountManager.ViewModels.Popup;
using AccountManager.Views;
using AccountManager.Views.Popup;
using ModernWpf.Controls;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace AccountManager.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IContainerExtension _container;
        private readonly IRegionManager _regionManager;

        public DelegateCommand LoadedCommand { get; }

        public MainWindowViewModel(IContainerExtension container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;

            LoadedCommand = new DelegateCommand(Loaded);
        }

        private async void Loaded()
        {
            try
            {
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
    }
}