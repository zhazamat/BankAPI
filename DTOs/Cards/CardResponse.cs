using BankAPI.Models;

namespace BankAPI.DTOs.Cards;

public class CardResponse
{
    public int Id { get; set; }

    public string CardNumber { get; set; } = string.Empty;

    public DateTime ExpirationDate { get; set; }

    public CardType Type { get; set; }

    public bool IsBlocked { get; set; }

    public int AccountId { get; set; }
}