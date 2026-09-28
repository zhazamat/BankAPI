using System.ComponentModel.DataAnnotations;

namespace BankAPI.DTOs.Clients;

public class UpdateClientRequest
{
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? MiddleName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }
}