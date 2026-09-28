using BankAPI.DTOs.Accounts;
using BankAPI.DTOs.Phones;

namespace BankAPI.DTOs.Clients;

public class ClientResponse
{
    public int Id { get; set; }

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public string Email { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public List<PhoneResponse> Phones { get; set; } = new();

    public List<AccountResponse> Accounts { get; set; } = new();
}