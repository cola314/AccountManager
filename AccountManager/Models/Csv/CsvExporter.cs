using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AccountManager.Utils;

namespace AccountManager.Models.Csv
{
    public class CsvExporter
    {
        public void Export(IEnumerable<Account> accounts, string filePath)
        {
            using (var file = File.Open(filePath, FileMode.CreateNew, FileAccess.Write))
            {
                using (var writer = new StreamWriter(file))
                {
                    foreach (var account in accounts)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(Base64Tool.Encode(account.Id ?? ""));
                        sb.Append(",");
                        sb.Append(Base64Tool.Encode(account.Site ?? ""));
                        sb.Append(",");
                        sb.Append(Base64Tool.Encode(account.Password ?? ""));
                        sb.Append(",");
                        sb.Append(Base64Tool.Encode(account.Description ?? "" ));

                        writer.WriteLine(sb.ToString());
                    }
                }
            }
        }
    }
}