using AccountManager.Models;
using Prism.Mvvm;

namespace AccountManager.ViewModels.Common
{
    public class AccountViewModel : BindableBase
    {
        private string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _site;
        public string Site
        {
            get => _site;
            set => SetProperty(ref _site, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public AccountViewModel() {}

        public AccountViewModel(Account account)
        {
            Id = account.Id;
            Site = account.Site;
            Password = account.Password;
            Description = account.Description;
        }

        public Account ToAccount() => new Account(Id, Site, Password, Description);

        public AccountViewModel Clone()
        {
            return (AccountViewModel)MemberwiseClone();
        }

        public void CopyFrom(AccountViewModel account)
        {
            Id = account.Id;
            Site = account.Site;
            Password = account.Password;
            Description = account.Description;
        }
    }
}