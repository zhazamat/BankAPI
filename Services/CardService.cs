using BankAPI.Data;
using BankAPI.DTOs.Cards;
using BankAPI.Models;
using BankAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class CardService : ICardService
{
    private readonly BankDbContext _context;

    public CardService(BankDbContext context)
    {
        _context = context;
    }

    public async Task<List<CardResponse>> GetAllAsync()
    {
        return await _context.Cards
            .Select(x => new CardResponse
            {
                Id = x.Id,
                CardNumber = x.CardNumber,
                ExpirationDate = x.ExpirationDate,
                Type = x.Type,
                IsBlocked = x.IsBlocked,
                AccountId = x.AccountId
            })
            .ToListAsync();
    }

    public async Task<CardResponse?> GetByIdAsync(int id)
    {
        return await _context.Cards
            .Where(x => x.Id == id)
            .Select(x => new CardResponse
            {
                Id = x.Id,
                CardNumber = x.CardNumber,
                ExpirationDate = x.ExpirationDate,
                Type = x.Type,
                IsBlocked = x.IsBlocked,
                AccountId = x.AccountId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<(bool Success, string? Error, CardResponse? Card)>
        CreateAsync(CreateCardRequest request)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == request.AccountId);

        if (account == null)
            return (false, "Account not found.", null);

        if (account.IsClosed)
            return (
                false,
                "Cannot add a card to a closed account.",
                null);

        var cardExists = await _context.Cards
            .AnyAsync(x => x.CardNumber == request.CardNumber);

        if (cardExists)
            return (
                false,
                "Card number already exists.",
                null);

        var card = new Card
        {
            CardNumber = request.CardNumber.Trim(),
            ExpirationDate = request.ExpirationDate,
            Type = request.Type,
            IsBlocked = false,
            AccountId = request.AccountId
        };

        _context.Cards.Add(card);

        await _context.SaveChangesAsync();

        var response = new CardResponse
        {
            Id = card.Id,
            CardNumber = card.CardNumber,
            ExpirationDate = card.ExpirationDate,
            Type = card.Type,
            IsBlocked = card.IsBlocked,
            AccountId = card.AccountId
        };

        return (true, null, response);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateCardRequest request)
    {
        var card = await _context.Cards
            .FirstOrDefaultAsync(x => x.Id == id);

        if (card == null || card.IsBlocked)
            return false;

        card.ExpirationDate = request.ExpirationDate;
        card.Type = request.Type;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<(bool Success, string? Error)> BlockAsync(int id)
    {
        var card = await _context.Cards
            .FirstOrDefaultAsync(x => x.Id == id);

        if (card == null)
            return (false, "Card not found.");

        if (card.IsBlocked)
            return (false, "Card is already blocked.");

        card.IsBlocked = true;

        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> UnblockAsync(int id)
    {
        var card = await _context.Cards
            .FirstOrDefaultAsync(x => x.Id == id);

        if (card == null)
            return (false, "Card not found.");

        if (!card.IsBlocked)
            return (false, "Card is already active.");

        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == card.AccountId);

        if (account == null)
            return (false, "Account not found.");

        if (account.IsClosed)
            return (
                false,
                "Cannot unblock a card linked to a closed account.");

        card.IsBlocked = false;

        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var card = await _context.Cards
            .FirstOrDefaultAsync(x => x.Id == id);

        if (card == null)
            return false;

        _context.Cards.Remove(card);

        await _context.SaveChangesAsync();

        return true;
    }
}