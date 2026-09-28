using BankAPI.Data;
using BankAPI.DTOs.Clients;
using BankAPI.DTOs.Accounts;
using BankAPI.DTOs.Cards;
using BankAPI.DTOs.Phones;
using BankAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class ClientService : IClientService
{
    private readonly BankDbContext _context;

    public ClientService(BankDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClientResponse>> GetAllAsync(
        string? search = null)
    {
        var query = _context.Clients
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.LastName.Contains(search) ||
                x.FirstName.Contains(search) ||
                (x.MiddleName != null &&
                 x.MiddleName.Contains(search)) ||
                x.Email.Contains(search));
        }

        return await query
            .OrderBy(x => x.LastName)
            .Select(x => new ClientResponse
            {
                Id = x.Id,
                LastName = x.LastName,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                Email = x.Email,
                BirthDate = x.BirthDate,

                Phones = x.Phones
                    .Select(p => new PhoneResponse
                    {
                        Id = p.Id,
                        Number = p.Number,
                        Type = p.Type,
                        ClientId = p.ClientId
                    })
                    .ToList(),

                Accounts = x.Accounts
                    .Select(a => new AccountResponse
                    {
                        Id = a.Id,
                        AccountNumber = a.AccountNumber,
                        Currency = a.Currency,
                        Balance = a.Balance,
                        IsClosed = a.IsClosed,
                        ClientId = a.ClientId,

                        Cards = a.Cards
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
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<ClientResponse?> GetByIdAsync(int id)
    {
        return await _context.Clients
            .Where(x => x.Id == id)
            .Select(x => new ClientResponse
            {
                Id = x.Id,
                LastName = x.LastName,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                Email = x.Email,
                BirthDate = x.BirthDate,

                Phones = x.Phones
                    .Select(p => new PhoneResponse
                    {
                        Id = p.Id,
                        Number = p.Number,
                        Type = p.Type,
                        ClientId = p.ClientId
                    })
                    .ToList(),

                Accounts = x.Accounts
                    .Select(a => new AccountResponse
                    {
                        Id = a.Id,
                        AccountNumber = a.AccountNumber,
                        Currency = a.Currency,
                        Balance = a.Balance,
                        IsClosed = a.IsClosed,
                        ClientId = a.ClientId,

                        Cards = a.Cards
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
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ClientResponse> CreateAsync(
        CreateClientRequest request)
    {
        var client = new Models.Client
        {
            LastName = request.LastName.Trim(),
            FirstName = request.FirstName.Trim(),
            MiddleName = request.MiddleName?.Trim(),
            Email = request.Email.Trim(),
            BirthDate = request.BirthDate
        };

        _context.Clients.Add(client);

        await _context.SaveChangesAsync();

        return new ClientResponse
        {
            Id = client.Id,
            LastName = client.LastName,
            FirstName = client.FirstName,
            MiddleName = client.MiddleName,
            Email = client.Email,
            BirthDate = client.BirthDate
        };
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateClientRequest request)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(x => x.Id == id);

        if (client == null)
            return false;

        client.LastName = request.LastName.Trim();
        client.FirstName = request.FirstName.Trim();
        client.MiddleName = request.MiddleName?.Trim();
        client.Email = request.Email.Trim();
        client.BirthDate = request.BirthDate;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(x => x.Id == id);

        if (client == null)
            return false;

        _context.Clients.Remove(client);

        await _context.SaveChangesAsync();

        return true;
    }
}