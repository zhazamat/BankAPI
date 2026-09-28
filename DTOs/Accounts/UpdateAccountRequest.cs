using System.ComponentModel.DataAnnotations;

namespace BankAPI.DTOs.Accounts;

public class UpdateAccountRequest
{
    [Required]
    [StringLength(3)]
    public string Currency { get; set; } = string.Empty;
}