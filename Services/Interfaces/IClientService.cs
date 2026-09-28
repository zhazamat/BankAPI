using BankAPI.DTOs.Clients;

namespace BankAPI.Services.Interfaces;

public interface IClientService
{
    Task<List<ClientResponse>> GetAllAsync(string? search = null);

    Task<ClientResponse?> GetByIdAsync(int id);

    Task<ClientResponse> CreateAsync(CreateClientRequest request);

    Task<bool> UpdateAsync(int id, UpdateClientRequest request);

    Task<bool> DeleteAsync(int id);
}