using BankAPI.DTOs.Transfers;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransfersController : ControllerBase
{
    private readonly ITransferService _transferService;

    public TransfersController(ITransferService transferService)
    {
        _transferService = transferService;
    }

    [HttpPost]
    public async Task<IActionResult> Transfer(
        TransferRequest request)
    {
        var result = await _transferService.TransferAsync(request);

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(new
        {
            message = "Transfer completed successfully."
        });
    }
}