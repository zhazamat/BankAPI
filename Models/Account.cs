namespace BankAPI.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public bool IsClosed { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; } = null!;

        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
