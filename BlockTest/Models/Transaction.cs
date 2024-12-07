namespace BlockTest.Models
{
    public class Transaction
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public decimal Amount { get; set; }

        public Transaction(string sender, string receiver, decimal amount)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = amount;
        }
    }
}
