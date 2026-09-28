using BankAPI.Models;

public class PhoneResponse
{
    public int Id { get; set; }

    public string Number { get; set; } = string.Empty;

    public PhoneType Type { get; set; }

    public int ClientId { get; set; }
}