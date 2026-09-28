using BankAPI.DTOs.Cards;

namespace BankAPI.Services.Interfaces;

public interface ICardService
{
    Task<List<CardResponse>> GetAllAsync();
    Task<CardResponse?> GetByIdAsync(int id);

    Task<(bool Success, string? Error, CardResponse? Card)> CreateAsync(CreateCardRequest request);

    Task<bool> UpdateAsync(int id, UpdateCardRequest request);

    Task<(bool Success, string? Error)> BlockAsync(int id);

    Task<(bool Success, string? Error)> UnblockAsync(int id);

    Task<bool> DeleteAsync(int id);
}