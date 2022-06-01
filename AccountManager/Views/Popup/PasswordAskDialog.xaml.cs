using AccountManager.Models;
using AccountManager.ViewModels.Popup;
using ModernWpf.Controls;

namespace AccountManager.Views.Popup
{
    /// <summary>
    /// PasswordAskDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PasswordAskDialog : ContentDialog
    {
        public PasswordAskDialogViewModel ViewModel => DataContext as PasswordAskDialogViewModel;

        public PasswordAskDialog()
        {
            InitializeComponent();
        }
    }
}
