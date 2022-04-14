using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Model
{
    public class Account : ObservableObject 
    {
        private string site_;
        public string Site
        {
            get => site_;
            set => SetProperty(ref site_, value);
        }

        public string id_;
        public string Id
        {
            get => id_;
            set => SetProperty(ref id_, value);
        }

        public string password_;
        public string Password
        {
            get => password_;
            set => SetProperty(ref password_, value);
        }

        public string description_;
        public string Description
        {
            get => description_;
            set => SetProperty(ref description_, value);
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
