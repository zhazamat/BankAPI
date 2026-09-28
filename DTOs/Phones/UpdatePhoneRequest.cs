using BankAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.DTOs.Phones;

public class UpdatePhoneRequest
{
    [Required]
    [StringLength(30)]
    public string Number { get; set; } = string.Empty;

    [Required]
    public PhoneType Type { get; set; }
}