﻿using System;
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

            foreach (Account val in Accounts)
            {
                val.History = val.ImportHistory();
            }
            
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

                    Account accntToCreate = new Account(name, type, balance);
                    Account.CreateAccount(accntToCreate);
                    Accounts.Add(accntToCreate);
                }

                if (UserData == "select")
                {
                    Console.Write("Enter the name of the account you want to select > ");
                    string accntSelected = Console.ReadLine();
                    
                    foreach (Account val in Accounts)
                    {
                        if (val.Name == accntSelected)
                        {
                            AccountManager(val);
                        }
                        else
                        {
                            try
                            {
                                Accounts = Account.ImportAccounts();
                                if (!Accounts.Contains(val))
                                {
                                    throw new Exception();
                                }
                                else
                                {
                                    AccountManager(val);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Account isn't in AccountName instance, try restarting the program");
                            }
                        }
                    }
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

        public static void AccountManager(Account account)
        {
            Console.WriteLine($"{account.Name}, {account.Balance}, {account.Type.ToString()}");
        }

        public static Transaction TransactionFactory()
        {
            Console.Write("Enter type (Addition, Subtraction) > ");
            string typein = Console.ReadLine();
            
            TransactionType type = Enum.Parse<TransactionType>(typein);
            
            Console.Write("Enter an amount > ");
            int amnt = Convert.ToInt32(Console.ReadLine());
            
            Console.Write("Enter a date > ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            Transaction ret = new Transaction(type, amnt, date);

            return ret;
        }
    }
}