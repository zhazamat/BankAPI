using BankAPI.DTOs.Accounts;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var accounts = await _accountService.GetAllAsync();

        return Ok(accounts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var account = await _accountService.GetByIdAsync(id);

        if (account == null)
            return NotFound("Account not found.");

        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountRequest request)
    {
        var account = await _accountService.CreateAsync(request);

        if (account == null)
            return BadRequest(
                "Client not found or account number already exists.");

        return CreatedAtAction(
            nameof(GetById),
            new { id = account.Id },
            account);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateAccountRequest request)
    {
        var updated = await _accountService.UpdateAsync(id, request);

        if (!updated)
            return BadRequest(
                "Account not found or account is closed.");

        return NoContent();
    }

    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(int id)
    {
        var result = await _accountService.CloseAsync(id);

        if (!result.Success)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _accountService.DeleteAsync(id);

        if (!deleted)
            return BadRequest(
                "Account not found or account is not closed.");

        return NoContent();
    }
}