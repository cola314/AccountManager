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
            [JsonProperty("id")]
            private string Id { get; }

            [JsonProperty("site")]
            private string Site { get; }

            [JsonProperty("password")]
            private string Password { get; }

            [JsonProperty("description")]
            private string Description { get; }
            
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
            public BackupJson(IReadOnlyList<Account> accounts)
            {
                if (accounts == null)
                    Accounts = Array.Empty<AccountJson>();
                else
                    Accounts = accounts.Select(x => new AccountJson(x)).ToList();
            }

            [JsonProperty("accounts")]
            private IReadOnlyList<AccountJson> Accounts { get; }

            public List<Account> ToAccounts() => Accounts.Select(x => x.ToAccount()).ToList();
        }

        /// <summary>
        /// 파일에 계정 정보를 암호화 해서 저장
        /// 실패시 예외 발생
        /// </summary>
        /// <param name="file"></param>
        /// <param name="password"></param>
        public void SaveToFile(string file, string password, List<Account> accounts)
        {
            var backupData = new BackupJson(accounts);
            string encryptedData = AesTool.Encrypt(JsonConvert.SerializeObject(backupData), password);
            File.WriteAllText(file, encryptedData);
        }

        public List<Account> Load(string file, string password)
        {
            string text = File.ReadAllText(file);
            var backupData = JsonConvert.DeserializeObject<BackupJson>(AesTool.Decrypt(text, password));
            return backupData.ToAccounts();
        }
    }
}