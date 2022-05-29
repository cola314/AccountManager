using Prism.Mvvm;

namespace AccountManager.Models
{
    public class Account : BindableBase
    {
        private string _site;
        public string Site
        {
            get => _site;
            set => SetProperty(ref _site, value);
        }

        private string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
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

        public Account Clone()
        {
            return (Account)MemberwiseClone();
        }

        public void CopyFrom(Account account)
        {
            this.Site = account.Site;
            this.Id = account.Id;
            this.Password = account.Password;
            this.Description = account.Description;
        }
    }
}
