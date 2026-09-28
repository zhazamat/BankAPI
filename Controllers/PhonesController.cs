using BankAPI.DTOs.Phones;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhonesController : ControllerBase
{
    private readonly IPhoneService _phoneService;

    public PhonesController(IPhoneService phoneService)
    {
        _phoneService = phoneService;
    }

    // GET: api/phones
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var phones = await _phoneService.GetAllAsync();

        return Ok(phones);
    }

    // GET: api/phones/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var phone = await _phoneService.GetByIdAsync(id);

        if (phone == null)
            return NotFound("Phone not found.");

        return Ok(phone);
    }

    // POST: api/phones
    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePhoneRequest request)
    {
        var phone = await _phoneService.CreateAsync(request);

        if (phone == null)
            return BadRequest("Client not found.");

        return CreatedAtAction(
            nameof(GetById),
            new { id = phone.Id },
            phone);
    }

    // PUT: api/phones/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdatePhoneRequest request)
    {
        var updated = await _phoneService.UpdateAsync(
            id,
            request);

        if (!updated)
            return NotFound("Phone not found.");

        return NoContent();
    }

    // DELETE: api/phones/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _phoneService.DeleteAsync(id);

        if (!deleted)
            return NotFound("Phone not found.");

        return NoContent();
    }
}