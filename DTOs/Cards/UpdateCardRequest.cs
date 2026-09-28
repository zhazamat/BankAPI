using BankAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.DTOs.Cards;

public class UpdateCardRequest
{
    [Required]
    [DataType(DataType.Date)]
    public DateTime ExpirationDate { get; set; }

    [Required]
    public CardType Type { get; set; }
}