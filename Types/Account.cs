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
        public List<Transaction> History { get; set; }

        public Account(string name, AccountType type, string balance)
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));
            
            this.Name = name;
            this.Type = type;
            this.Balance = balance;
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

        public void EditBalance(string balance)
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));
            
            File.Delete(Path.Join(dat.AccountDirectory, this.Name, this.Name + ".json"));

            this.Balance = balance;

            CreateAccount(this);
        }

        public List<Transaction> ImportHistory()
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));
            string path = Path.Join(dat.AccountDirectory, this.Name, "history.txt");

            List<Transaction> ret = new();
            
            foreach (string val in File.ReadLines(path))
            {
                Transaction x = JsonConvert.DeserializeObject<Transaction>(val);
                ret.Add(x);
            }
            
            return ret;
        }
        public void EditHistory(Transaction transaction)
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));
            this.History = ImportHistory();
            string path = Path.Join(dat.AccountDirectory, this.Name, "history.txt");
            
            if (transaction != null)
            {
                string towrite = JsonConvert.SerializeObject(transaction);
                File.WriteAllText(path, towrite);
            }
        }
    }
}