using System.Windows;
using AccountManager.Views;
using Prism.DryIoc;
using Prism.Ioc;

namespace AccountManager
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AccountView>();
            containerRegistry.RegisterForNavigation<SettingsView>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }
}
