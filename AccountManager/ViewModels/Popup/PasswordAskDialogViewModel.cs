using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace AccountManager.ViewModels.Popup
{
    public class PasswordAskDialogViewModel : BindableBase
    {
        public ICommand PasswordChangedCommand { get; }

        public PasswordAskDialogViewModel()
        {
            PasswordChangedCommand = new DelegateCommand<PasswordBox>(PasswordChanged);
        }

        public string Password { get; private set; } = "";

        private void PasswordChanged(PasswordBox passwordBox)
        {
            Password = passwordBox.Password;
        }
    }
}