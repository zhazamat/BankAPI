using BankAPI.DTOs.Cards;

namespace BankAPI.DTOs.Accounts;

public class AccountResponse
{
    public int Id { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public bool IsClosed { get; set; }

    public int ClientId { get; set; }

    public List<CardResponse> Cards { get; set; } = new();
}