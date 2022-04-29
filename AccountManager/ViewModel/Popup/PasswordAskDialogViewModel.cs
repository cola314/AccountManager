using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace AccountManager.ViewModel.Popup
{
    public class PasswordAskDialogViewModel : ObservableRecipient
    {
        public string Password { get; private set; } = "";

        public ICommand PasswordChanged
        {
            get
            {
                return new RelayCommand<PasswordBox>(passwordBox =>
                {
                    Password = passwordBox.Password;
                });
            }
        }
    }
}
