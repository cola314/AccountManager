using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AccountManager.Utils;
using Newtonsoft.Json;

namespace AccountManager.Models
{
    public class AccountRepository
    {
        private class AccountJson
        {
            public string Id { get; set; }
            public string Site { get; set; }
            public string Password { get; set; }
            public string Description { get; set; }

            public AccountJson()
            {
            }

            public AccountJson(Account account)
            {
                this.Id = account.Id;
                this.Site = account.Site;
                this.Password = account.Password;
                this.Description = account.Description;
            }

            public Account ToAccount() => new Account(Id, Site, Password, Description);
        }

        private class BackupJson
        {
            public BackupJson()
            {
            }

            public List<AccountJson> Accounts { get; set; } = new List<AccountJson>();
        }

        /// <summary>
        /// 파일에 계정 정보를 암호화 해서 저장
        /// 실패시 예외 발생
        /// </summary>
        /// <param name="file"></param>
        /// <param name="password"></param>
        public void SaveToFile(string file, string password, List<Account> accounts)
        {
            var backupData = new BackupJson()
            {
                Accounts = accounts.Select(x => new AccountJson(x)).ToList()
            };
            string encryptedData = AesTool.Encrypt(JsonConvert.SerializeObject(backupData), password);
            File.WriteAllText(file, encryptedData);
        }

        public List<Account> Load(string file, string password)
        {
            string text = File.ReadAllText(file);
            var backupData = JsonConvert.DeserializeObject<BackupJson>(AesTool.Decrypt(text, password));
            return backupData.Accounts.Select(x => x.ToAccount()).ToList();
        }
    }
}