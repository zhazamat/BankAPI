using BankAPI.Data;
using BankAPI.DTOs.Accounts;
using BankAPI.DTOs.Cards;
using BankAPI.Models;
using BankAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class AccountService : IAccountService
{
    private readonly BankDbContext _context;

    public AccountService(BankDbContext context)
    {
        _context = context;
    }

    public async Task<List<AccountResponse>> GetAllAsync()
    {
        return await _context.Accounts
            .Select(x => new AccountResponse
            {
                Id = x.Id,
                AccountNumber = x.AccountNumber,
                Currency = x.Currency,
                Balance = x.Balance,
                IsClosed = x.IsClosed,
                ClientId = x.ClientId,

                Cards = x.Cards
                    .Select(c => new CardResponse
                    {
                        Id = c.Id,
                        CardNumber = c.CardNumber,
                        ExpirationDate = c.ExpirationDate,
                        Type = c.Type,
                        IsBlocked = c.IsBlocked,
                        AccountId = c.AccountId
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<AccountResponse?> GetByIdAsync(int id)
    {
        return await _context.Accounts
            .Where(x => x.Id == id)
            .Select(x => new AccountResponse
            {
                Id = x.Id,
                AccountNumber = x.AccountNumber,
                Currency = x.Currency,
                Balance = x.Balance,
                IsClosed = x.IsClosed,
                ClientId = x.ClientId,

                Cards = x.Cards
                    .Select(c => new CardResponse
                    {
                        Id = c.Id,
                        CardNumber = c.CardNumber,
                        ExpirationDate = c.ExpirationDate,
                        Type = c.Type,
                        IsBlocked = c.IsBlocked,
                        AccountId = c.AccountId
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AccountResponse?> CreateAsync(CreateAccountRequest request)
    {
        var clientExists = await _context.Clients
            .AnyAsync(x => x.Id == request.ClientId);

        if (!clientExists)
            return null;

        var accountExists = await _context.Accounts
            .AnyAsync(x => x.AccountNumber == request.AccountNumber);

        if (accountExists)
            return null;

        var account = new Account
        {
            AccountNumber = request.AccountNumber.Trim(),
            Currency = request.Currency.Trim().ToUpper(),
            Balance = request.Balance,
            IsClosed = false,
            ClientId = request.ClientId
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return new AccountResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            Currency = account.Currency,
            Balance = account.Balance,
            IsClosed = account.IsClosed,
            ClientId = account.ClientId,
            Cards = new List<CardResponse>()
        };
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateAccountRequest request)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == id);

        if (account == null || account.IsClosed)
            return false;

        account.Currency = request.Currency.Trim().ToUpper();

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<(bool Success, string? Error)> CloseAsync(int id)
    {
        var account = await _context.Accounts
            .Include(x => x.Cards)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (account == null)
            return (false, "Account not found.");

        if (account.IsClosed)
            return (false, "Account is already closed.");

        if (account.Balance != 0)
            return (false, "Account balance must be zero.");

        var hasActiveCards = account.Cards
            .Any(x => !x.IsBlocked);

        if (hasActiveCards)
            return (false, "Account has active cards.");

        account.IsClosed = true;

        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == id);

        if (account == null || !account.IsClosed)
            return false;

        _context.Accounts.Remove(account);

        await _context.SaveChangesAsync();

        return true;
    }
}