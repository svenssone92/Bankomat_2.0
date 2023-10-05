using System.Text.Json.Serialization;

namespace BankClasses
{
    public class BankAccount
    {
        private string? accountNr;
        public string? AccountNr { get { return accountNr; } }
        public double Balance { get; set; }

        [JsonConstructor]
        public BankAccount(string accountNr, double balance)
        {
            this.accountNr = accountNr;
            Balance = balance;
        }

        public void Add(double sum)
        {
            Balance += sum;
        }

        public void Take(double sum)
        {
            Balance -= sum;
        }
    }
}