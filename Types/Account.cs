using System;
using System.Collections.Generic;
using System.IO;
using Clidget.Core.ProgramData;
using Newtonsoft.Json;

namespace Clidget.Core.Types
{
    public class Account
    {
        public string Name { get; set; }
        public AccountType Type { get; set; }
        public string Balance { get; set; }
        public string[]? History { get; set; }

        public Account(string name, AccountType type, string balance, string[]? history)
        {
            this.Name = name;
            this.Type = type;
            this.Balance = balance;
            this.History = history;
        }

        public static List<Account> ImportAccounts()
        {
            Console.WriteLine("Importing Accounts...");
            
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));

            DirectoryInfo dirinf = new DirectoryInfo(dat.AccountDirectory);

            List<Account> AccountList = new();
            
            foreach (DirectoryInfo dir in dirinf.GetDirectories())
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (!AccountList.Contains(JsonConvert.DeserializeObject<Account>(File.ReadAllText(file.FullName))))
                    {
                        AccountList.Add(JsonConvert.DeserializeObject<Account>(File.ReadAllText(file.FullName)));
                    }
                }
            }

            return AccountList;
        }
   
        public static void CreateAccount(Account account)
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));

            Directory.CreateDirectory(Path.Combine(dat.AccountDirectory, account.Name));
            string toWrite = JsonConvert.SerializeObject(account);
            FileStream fs = File.Create(Path.Combine(dat.AccountDirectory, account.Name, $"{account.Name}.json"));
            fs.Close();
            File.WriteAllText(Path.Combine(dat.AccountDirectory, account.Name, $"{account.Name}.json"), toWrite);
        }

        public void EditAccount(string balance, string[]? history)
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));
            
            File.Delete(Path.Join(dat.AccountDirectory, this.Name, this.Name + ".json"));

            this.Balance = balance;
            this.History = history;
            
            CreateAccount(this);
        }
    }
}