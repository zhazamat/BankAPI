using BankAPI.DTOs.Cards;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardsController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cards = await _cardService.GetAllAsync();

        return Ok(cards);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var card = await _cardService.GetByIdAsync(id);

        if (card == null)
            return NotFound("Card not found.");

        return Ok(card);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCardRequest request)
    {
        var result = await _cardService.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result.Error);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Card!.Id },
            result.Card);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCardRequest request)
    {
        var updated = await _cardService.UpdateAsync(id, request);

        if (!updated)
            return BadRequest(
                "Card not found or card is blocked.");

        return NoContent();
    }

    [HttpPost("{id:int}/block")]
    public async Task<IActionResult> Block(int id)
    {
        var result = await _cardService.BlockAsync(id);

        if (!result.Success)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpPost("{id:int}/unblock")]
    public async Task<IActionResult> Unblock(int id)
    {
        var result = await _cardService.UnblockAsync(id);

        if (!result.Success)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _cardService.DeleteAsync(id);

        if (!deleted)
            return NotFound("Card not found.");

        return NoContent();
    }
}