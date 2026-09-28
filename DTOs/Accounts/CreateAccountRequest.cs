using System.ComponentModel.DataAnnotations;

namespace BankAPI.DTOs.Accounts;

public class CreateAccountRequest
{
    [Required]
    [StringLength(50)]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(3)]
    public string Currency { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Balance { get; set; }

    [Required]
    public int ClientId { get; set; }
}