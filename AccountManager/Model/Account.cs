using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Model
{
    public class Account : ViewModelBase
    {
        private string site_;
        public string Site
        {
            get => site_;
            set
            {
                site_ = value;
                RaisePropertyChanged(() => Site);
            }
        }

        public string id_;
        public string Id
        {
            get => id_;
            set
            {
                id_ = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string password_;
        public string Password
        {
            get => password_;
            set
            {
                password_ = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public string description_;
        public string Description
        {
            get => description_;
            set
            {
                description_ = value;
                RaisePropertyChanged(() => Description);
            }
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
