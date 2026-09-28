using BankAPI.Data;
using BankAPI.DTOs.Phones;
using BankAPI.Models;
using BankAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class PhoneService : IPhoneService
{
    private readonly BankDbContext _context;

    public PhoneService(BankDbContext context)
    {
        _context = context;
    }

    public async Task<List<PhoneResponse>> GetAllAsync()
    {
        return await _context.Phones
            .Select(x => new PhoneResponse
            {
                Id = x.Id,
                Number = x.Number,
                Type = x.Type,
                ClientId = x.ClientId
            })
            .ToListAsync();
    }

    public async Task<PhoneResponse?> GetByIdAsync(int id)
    {
        return await _context.Phones
            .Where(x => x.Id == id)
            .Select(x => new PhoneResponse
            {
                Id = x.Id,
                Number = x.Number,
                Type = x.Type,
                ClientId = x.ClientId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PhoneResponse?> CreateAsync(
        CreatePhoneRequest request)
    {
        var clientExists = await _context.Clients
            .AnyAsync(x => x.Id == request.ClientId);

        if (!clientExists)
            return null;

        var phone = new Phone
        {
            Number = request.Number.Trim(),
            Type = request.Type,
            ClientId = request.ClientId
        };

        _context.Phones.Add(phone);

        await _context.SaveChangesAsync();

        return new PhoneResponse
        {
            Id = phone.Id,
            Number = phone.Number,
            Type = phone.Type,
            ClientId = phone.ClientId
        };
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdatePhoneRequest request)
    {
        var phone = await _context.Phones
            .FirstOrDefaultAsync(x => x.Id == id);

        if (phone == null)
            return false;

        phone.Number = request.Number.Trim();
        phone.Type = request.Type;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var phone = await _context.Phones
            .FirstOrDefaultAsync(x => x.Id == id);

        if (phone == null)
            return false;

        _context.Phones.Remove(phone);

        await _context.SaveChangesAsync();

        return true;
    }
}