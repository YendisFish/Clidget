using System;
using System.Collections.Generic;
using System.IO;
using Clidget.Core.Types;
using Newtonsoft.Json;
using Clidget.Core.Types;
using Clidget.Core.ProgramData;

namespace Clidget.Core
{
    class RootClass
    {
        public static void Main()
        {
            Console.WriteLine("Starting Clidget...");
            StartupChecks();
            List<Account> Accounts = Account.ImportAccounts();

            while (true)
            {
                Console.Write("Type help for a list of commands > ");
                string UserData = Console.ReadLine();

                if (UserData == "help")
                {
                    Console.WriteLine("list - Returns a list of accounts");
                }

                if (UserData == "list")
                {
                    foreach (Account acc in Accounts)
                    {
                        Console.WriteLine(acc.Name + " - Bal: " + acc.Balance + " Type: " + acc.Type);
                    }
                }

                if (UserData == "create")
                {
                    Console.Write("Name > ");
                    string name = Console.ReadLine();
                    
                    Console.WriteLine();
                    
                    Console.Write("Account Type (Checking, Savings, Wroth, Retirement, Investment) > ");
                    string accntTypeToSet = Console.ReadLine();

                    AccountType type = Enum.Parse<AccountType>(accntTypeToSet);
                    
                    Console.Write("Balance (if there is none just type 0) > ");
                    string trybal = Console.ReadLine();
                    string balance = "";
                    
                    if (trybal == "")
                    {
                        balance = "0";
                    }
                    else if(trybal != null)
                    {
                        balance = trybal;
                    }
                    else
                    {
                        Console.WriteLine("Failed to create account, please enter a valid balance!");
                        continue;
                    }
                    
                    string[]? history = {"none"};
                    
                    Account accntToCreate = new Account(name, type, balance, history);
                    Account.CreateAccount(accntToCreate);
                    Accounts.Add(accntToCreate);
                }
            }
        }

        public static void StartupChecks()
        {
            ProgramDataType dat = JsonConvert.DeserializeObject<ProgramDataType>(File.ReadAllText("./AppSettings.json"));

            if (!Directory.Exists(dat.BaseDirectory))
            {
                Directory.CreateDirectory(dat.BaseDirectory);
            }

            if (!Directory.Exists(dat.AccountDirectory))
            {
                Directory.CreateDirectory(dat.AccountDirectory);
            }
        }
    }
}