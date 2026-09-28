using BankAPI.Data;
using BankAPI.DTOs.Transfers;
using BankAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class TransferService : ITransferService
{
    private readonly BankDbContext _context;

    public TransferService(BankDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string? Error)> TransferAsync(
        TransferRequest request)
    {
        if (request.Amount <= 0)
            return (false, "Transfer amount must be greater than zero.");

        if (request.FromAccountId == request.ToAccountId)
            return (false, "Source and destination accounts must be different.");

        var fromAccount = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == request.FromAccountId);

        if (fromAccount == null)
            return (false, "Source account not found.");

        var toAccount = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == request.ToAccountId);

        if (toAccount == null)
            return (false, "Destination account not found.");

        if (fromAccount.IsClosed)
            return (false, "Source account is closed.");

        if (toAccount.IsClosed)
            return (false, "Destination account is closed.");

        if (!string.Equals(
                fromAccount.Currency,
                toAccount.Currency,
                StringComparison.OrdinalIgnoreCase))
        {
            return (false, "Accounts must have the same currency.");
        }

        if (fromAccount.Balance < request.Amount)
            return (false, "Insufficient funds.");

        fromAccount.Balance -= request.Amount;
        toAccount.Balance += request.Amount;

        await _context.SaveChangesAsync();

        return (true, null);
    }
}