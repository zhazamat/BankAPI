using BankAPI.DTOs.Accounts;

namespace BankAPI.Services.Interfaces;

public interface IAccountService
{
    Task<List<AccountResponse>> GetAllAsync();
    Task<AccountResponse?> GetByIdAsync(int id);
    Task<AccountResponse?> CreateAsync(CreateAccountRequest request);
    Task<bool> UpdateAsync(int id, UpdateAccountRequest request);
    Task<(bool Success, string? Error)> CloseAsync(int id);
    Task<bool> DeleteAsync(int id);
}