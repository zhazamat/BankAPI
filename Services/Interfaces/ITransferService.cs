using BankAPI.DTOs.Transfers;

namespace BankAPI.Services.Interfaces;

public interface ITransferService
{
    Task<(bool Success, string? Error)> TransferAsync(
        TransferRequest request);
}