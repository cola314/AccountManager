using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AccountManager.ViewModel
{
    public class PasswordAskDialogViewModel : ViewModelBase
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
