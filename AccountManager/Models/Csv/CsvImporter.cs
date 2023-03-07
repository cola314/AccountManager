using System.Collections.Generic;
using System.IO;
using System.Linq;
using AccountManager.Utils;

namespace AccountManager.Models.Csv
{
    public class CsvImporter
    {
        public IEnumerable<Account> Import(string filePath)
        {
            using (var file = File.OpenRead(filePath))
            {
                using (var reader = new StreamReader(file))
                {
                    while (true)
                    {
                        var line = reader.ReadLine();
                        if (line == null) break;

                        var token = line.Split(',')
                            .Select(Base64Tool.Decode)
                            .ToArray();

                        if (token.Length == 4)
                        {
                            yield return new Account(
                                id: token[0],
                                site: token[1],
                                password: token[2],
                                description: token[3]);
                        }
                    }
                }
            }
        }
    }
}