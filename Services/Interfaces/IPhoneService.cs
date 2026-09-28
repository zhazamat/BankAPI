using BankAPI.DTOs.Phones;

namespace BankAPI.Services.Interfaces;

public interface IPhoneService
{
    Task<List<PhoneResponse>> GetAllAsync();

    Task<PhoneResponse?> GetByIdAsync(int id);

    Task<PhoneResponse?> CreateAsync(CreatePhoneRequest request);

    Task<bool> UpdateAsync(int id, UpdatePhoneRequest request);

    Task<bool> DeleteAsync(int id);
}