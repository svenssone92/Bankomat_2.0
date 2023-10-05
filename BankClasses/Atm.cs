using System.Text.Json;

namespace BankClasses
{
    public class Atm
    {
        List<BankAccount> bankAccounts = new List<BankAccount> { };

        public void GetAccounts()
        {
            if (File.Exists("accounts.json"))
            {
                string toGetAccounts = System.IO.File.ReadAllText("accounts.json");
                bankAccounts = JsonSerializer.Deserialize<List<BankAccount>>(toGetAccounts) ?? new List<BankAccount>();
            }
        }

        public void SaveAccounts()
        {
            string toSaveAccounts = JsonSerializer.Serialize(bankAccounts);
            System.IO.File.WriteAllText("accounts.json", toSaveAccounts);
        }


        public void CreateAccount()
        {
            if (bankAccounts.Count < 10)
            {
                string kontoNr = GenerateAccountNo();
                BankAccount konto = new BankAccount(kontoNr, 0);
                bankAccounts.Add(konto);
            }
            else
            {
                Console.WriteLine("A maximum of ten accounts are allowed...");
                Console.ReadLine();
            }
        }

        public void DeleteAccount()
        {
            int index = ChooseAccount();
            if (index == -1) { return; }

            Console.WriteLine($"\nAccount: {bankAccounts[index].AccountNr} will be removed... type REMOVE to confirm.");
            string? input = Console.ReadLine();

            if (input == "REMOVE")
            {
                bankAccounts.RemoveAt(index);
            }

            else
            {
                Console.WriteLine("\nNo account was removed...");
                Console.ReadLine();
            }
        }


        private string GenerateAccountNo()
        {
            while (true)
            {
                Random random = new Random();
                string kontoNr = Convert.ToString(random.Next(10000000, 99999999));

                if (!FinnskontoNr(kontoNr))
                {
                    return kontoNr;
                }
            }
        }


        public void SeeAccounts()
        {
            Console.WriteLine("Account".PadRight(20) + "Balance".PadLeft(20) + "\n");
            foreach (BankAccount account in bankAccounts)
            {
                string accountNumber = account.AccountNr.PadRight(20);
                string Balance = account.Balance.ToString("C").PadLeft(20);

                Console.WriteLine($"{accountNumber}{Balance}");
            }
        }


        public int ChooseAccount()
        {
            while (true)
            {
                Console.Clear();
                SeeAccounts();

                Console.Write("\nAccountNr. To select an account.\n9.         To go back.\n\nAccountNr:");
                string? input = Console.ReadLine();
                if (input == "9")
                {
                    return -1;
                }
                int index = bankAccounts.FindIndex(a => a.AccountNr == input);
                if (index != -1)
                {
                    return index;
                }
                else
                {
                    Console.WriteLine("no account number matches your input");
                    Console.ReadLine();
                }

            }
        }

        public double GetSum()
        {
            Console.Write("Enter sum:");
            double.TryParse(Console.ReadLine(), out double sum);

            return sum;

        }

        public void Withdraw(int i, double summa)
        {
            if (bankAccounts[i].Balance >= summa)
            {
                bankAccounts[i].Take(summa);
            }
            else
            {
                Console.WriteLine("You do not have that balance available");
                Console.ReadLine();
            }
        }

        public void Deposit(int i, double sum)
        {
            bankAccounts[i].Add(sum);
        }


        public bool FinnskontoNr(string input)
        {
            return bankAccounts.Any(b => b?.AccountNr == input);
        }

        private void Menu()
        {
            Console.Clear();
            Console.WriteLine("Cash machine\n\n1. Withdraw money\n2. Deposit money\n3. Open new account\n4. Delete Account\n");
            SeeAccounts();
            Console.WriteLine("\n9. Quit");
        }

        private void MenuChoice()
        {
            string? input = Console.ReadLine();

            if (input == "1" && bankAccounts != null)
            {
                int index = ChooseAccount();
                if (index == -1) { return; }
                double sum = GetSum();
                Withdraw(index, sum);
            }
            else if (input == "2" && bankAccounts != null)
            {
                int index = ChooseAccount();
                if (index == -1) { return; }
                double sum = GetSum();
                Deposit(index, sum);
            }
            else if (input == "3")
            {
                CreateAccount();
            }

            else if (input == "4")
            {
                DeleteAccount();
            }
            else if (input == "9")
            {
                SaveAccounts();
                Environment.Exit(0);
            }
        }



        public void GoGoCashMachine()
        {
            while (true)
            {
                GetAccounts();
                Menu();
                MenuChoice();
                SaveAccounts();
            }
        }
    }
}

