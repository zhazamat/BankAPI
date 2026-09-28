using BankAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.DTOs.Cards;

public class CreateCardRequest
{
    [Required]
    [StringLength(20)]
    public string CardNumber { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime ExpirationDate { get; set; }

    [Required]
    public CardType Type { get; set; }

    [Required]
    public int AccountId { get; set; }
}