using Microsoft.VisualBasic;

namespace BankAPI.Models
{
    public class Card
    {
        public int Id { get; set; }

        public string CardNumber { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; }

        public CardType Type { get; set; }

        public bool IsBlocked { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; } = null!;
    }
}
