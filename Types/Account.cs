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
        public List<Transaction>? History { get; set; }
        public Budget? Budget { get; set; }

        public Account(string name, AccountType type, string balance)
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));
            
            this.Name = name;
            this.Type = type;
            this.Balance = balance;
            this.History = new List<Transaction>();
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
                    if (file.FullName.EndsWith(".json"))
                    {
                        if (!AccountList.Contains(JsonConvert.DeserializeObject<Account>(File.ReadAllText(file.FullName))) && File.Exists(file.FullName))
                        {
                            AccountList.Add(JsonConvert.DeserializeObject<Account>(File.ReadAllText(file.FullName)));
                        }
                    }
                }
            }

            return AccountList;
        }
   
        public static void CreateAccount(Account account, bool dohistory = true)
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));

            Directory.CreateDirectory(Path.Combine(dat.AccountDirectory, account.Name));
            string toWrite = JsonConvert.SerializeObject(account);
            FileStream fs = File.Create(Path.Combine(dat.AccountDirectory, account.Name, $"{account.Name}.json"));
            fs.Close();
            File.WriteAllText(Path.Combine(dat.AccountDirectory, account.Name, $"{account.Name}.json"), toWrite);

            if (dohistory == true)
            {
                FileStream fs2= File.Create(Path.Join(dat.AccountDirectory, account.Name, "history.txt"));
                fs2.Close();
            }
        }

        public void EditBalance(Transaction transaction)
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));
            
            File.Delete(Path.Join(dat.AccountDirectory, this.Name, this.Name + ".json"));

            if (transaction.Type == TransactionType.Addition)
            {
                long newbal = Convert.ToInt64(this.Balance) + transaction.Amount;
            
                this.Balance = newbal.ToString();

                CreateAccount(this, false);
            }

            if (transaction.Type == TransactionType.Subtraction)
            {
                long newbal = Convert.ToInt64(this.Balance) - transaction.Amount;
            
                this.Balance = newbal.ToString();
            
                CreateAccount(this, false);
            }
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
        public void AddToHistory(Transaction transaction)
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));
            this.History = ImportHistory();
            string path = Path.Join(dat.AccountDirectory, this.Name, "history.txt");
            
            if (transaction != null)
            {
                EditBalance(transaction);
                transaction.RunningBalance = this.Balance.ToString();
                string towrite = JsonConvert.SerializeObject(transaction);
                File.AppendAllText(path, towrite + Environment.NewLine);
            }
            
            this.History = ImportHistory();
            
        }

        public void AddBudget(Budget budget)
        {
            this.Budget = budget;
            CreateAccount(this, false);
        }

        public void RemoveBudget()
        {
            this.Budget = null;
            CreateAccount(this, false);
        }
    }
}